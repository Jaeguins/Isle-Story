using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System.IO;
using UnityEngine.UI;
public enum BuildingType {
    HALL, INN, COMPANY, WORKSITE,MISC
}

public enum SizeType {
    SINGLE, HEX
}
public class Building : Statics{
    public EntitySummary summaryViewer;
    public override void Awake() {
        base.Awake();
        EntityType = EntityType.Building;
        NowConstructTime = ConstructTime;
    }
    public Human ReceiveableMan;
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
        writer.Write(NowConstructTime);
    }
    public static Building Load(BinaryReader reader) {
        BuildingType type = (BuildingType)reader.ReadInt32();
        TriDirection entDir = (TriDirection)reader.ReadInt32();
        bool underconstruct = reader.ReadBoolean();
        bool working = reader.ReadBoolean();
        float constructTime = reader.ReadSingle();
        Building ret = null;
        switch (type) {
            case BuildingType.HALL:
                ret = Hall.Load(reader);
                break;
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
        ret.NowConstructTime = constructTime;
        return ret;
    }
    public void BindWorkers() {
        personList.Bind(this, Insider);
    }
    public override void Tick() {
        base.Tick();
        ReceiveableMan = null;
    }
    public void OnMouseUp() {
        if (Dragable) Selector.Instance.FinishDrag();
    }
    public override void CheckConstruction() {
        base.CheckConstruction();
        if (NowConstructTime < 0) {
            ConstructionIndicator.SetActive(false);
            Model.SetActive(true);
        }
    }
}
