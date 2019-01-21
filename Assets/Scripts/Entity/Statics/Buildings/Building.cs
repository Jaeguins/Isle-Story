using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System.IO;
using UnityEngine.UI;
public enum BuildingType {
    HALL,INN, COMPANY, WORKSITE
}

public enum SizeType {
    SINGLE, HEX
}
public class Building : Statics {
    public override void Awake() {
        base.Awake();
        EntityType = EntityType.Building;
    }
    public bool UnderConstruct = true;
    public float ConstructTime = 9999f;
    public BuildingType type;
    public EntityList<Unit> personList;

    public GameObject ConstructionIndicator, Model;
    public new TriCell Location {
        get {
            return location;
        }
        set {
            location = value;
            value.Statics = this;
            transform.localPosition = value.Position;
        }
    }


    /*
     * save sequence
     * superclassed saves
     * >type
     * >entrance direction
     * >under construct status
     * >remain construction time
     */


    public override void Save(BinaryWriter writer) {
        base.Save(writer);
        writer.Write((int)type);
        writer.Write((int)EntranceDirection);
        writer.Write(UnderConstruct);
        writer.Write(Working);
        writer.Write(ConstructTime);
    }
    public static Building Load(BinaryReader reader) {
        BuildingType type = (BuildingType)reader.ReadInt32();
        TriDirection entDir = (TriDirection)reader.ReadInt32();
        bool underconstruct = reader.ReadBoolean();
        bool working = reader.ReadBoolean();
        float constructTime = reader.ReadSingle();
        Building ret = null;
        switch (type) {
            case BuildingType.INN:
                ret = Inn.Load(reader);
                break;
            case BuildingType.COMPANY:
                ret = Company.Load(reader);
                break;
            case BuildingType.WORKSITE:
                ret = Worksite.Load(reader);
                break;
        }
        ret.entranceDirection = entDir;
        ret.type = type;
        ret.UnderConstruct = underconstruct;
        ret.Working = working;
        ret.ConstructTime = constructTime;
        return ret;
    }
    public void BindWorkers() {
        personList.Bind(this, Insider);
    }

    
    public virtual void CheckConstruction() {
        if (ConstructTime < 0) {
            UnderConstruct = false;
            Working = true;
            for (int i = 0; i < Insider.Count; i++) {
                Insider[i].AddCommand(new ChangeWorkCommand(null));
                if (((Person)Insider[i]).Company)
                    Insider[i].AddCommand(new GoJobCommand());
                else Insider[i].AddCommand(new GoHomeCommand());
            }
            Insider.Clear();
            ConstructionIndicator.SetActive(false);
            Model.SetActive(true);
        }
        else
            ConstructTime -= Time.deltaTime * Insider.Count;
    }
    public void LateUpdate() {
        if (UnderConstruct) {
            CheckConstruction();
        }
    }
}
