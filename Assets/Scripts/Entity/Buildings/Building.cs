using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System.IO;
using UnityEngine.UI;
public enum BuildingType {
    INN,COMPANY,WORKSITE
}

public enum SizeType {
    SINGLE,HEX
}
public class Building : Entity {
    public bool UnderConstruct = true;
    public float ConstructTime=9999f;
    public BuildingType type;
    public PersonList personList;
    public List<Person> Insider;
    public GameObject ConstructionIndicator,Model;
    TriDirection entranceDirection;
    public TriCell EntranceLocation {
        get {
            return Location.GetNeighbor(EntranceDirection);
        }
    }
    public new TriCell Location {
        get {
            return location;
        }
        set {
            location = value;
            value.Entity = this;
            transform.localPosition = value.Position;
        }
    }

    public TriDirection EntranceDirection {
        get {
            return entranceDirection;
        }
        set {
            entranceDirection = value;
            Vector3 rot = new Vector3(0, (int)entranceDirection*120+(Location.inverted?0:180), 0);
            transform.localRotation = Quaternion.Euler(rot);
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
        writer.Write(ConstructTime);
    }
    public static Building Load(BinaryReader reader) {
        BuildingType type = (BuildingType)reader.ReadInt32();
        TriDirection entDir=(TriDirection)reader.ReadInt32();
        bool underconstruct = reader.ReadBoolean();
        float constructTime = reader.ReadSingle();
        Building ret=null;
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
        ret.ConstructTime = constructTime;
        return ret;
    }
    
    public override void BindOptions(CommandPanel menu) {
        base.BindOptions(menu);
        if (UnderConstruct&&Insider.Count > 0) { }
            menu.BindButton(1, "workers", BindWorkers);
    }
    public void BindWorkers() {
        personList.Bind(this, Insider);
    }
    
    private void OnMouseDown() {
        if (EventSystem.current.IsPointerOverGameObject()) return;
        EntityMenu.Instance.BindBuilding(this);
        
    }
    public void AddInsider(Person p) {
        Insider.Add(p);
    }
    public void RemoveWorker(Person p) {
        Insider.Remove(p);
    }
    public virtual void CheckConstruction() {
        if (ConstructTime < 0) {
            UnderConstruct = false;
            for(int i = 0; i < Insider.Count; i++) {
                Insider[i].AddCommand(new ChangeWorkCommand(null));
                if (Insider[i].Company)
                    Insider[i].AddCommand(new GoJobCommand());
                else
                    Insider[i].AddCommand(new GoHomeCommand());
            }
            Insider.Clear();
            ConstructionIndicator.SetActive(false);
            Model.SetActive(true);
        }
        else
            ConstructTime -= Time.deltaTime*Insider.Count;
    }
    public virtual void DailyCycle() {

    }
    public void LateUpdate() {
        if (UnderConstruct) {
            CheckConstruction();
        }
        else {
            DailyCycle();
        }
    }
}
