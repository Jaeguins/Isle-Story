using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using UnityEngine.UI;
public enum BuildingType {
    INN,HALL
}
public class Building : Entity {
    public BuildingType type;
    
    public TriDirection entranceDirection;
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
    
    public new void Save(BinaryWriter writer) {
        base.Save(writer);
        writer.Write((int)type);
        writer.Write((int)EntranceDirection);
        Location.coordinates.Save(writer);
    }
    public static Building Load(BinaryReader reader) {
        BuildingType type = (BuildingType)reader.ReadInt32();
        TriDirection entDir=(TriDirection)reader.ReadInt32();
        TriCoordinates coord = TriCoordinates.Load(reader);
        Building ret=null;
        switch (type) {
            case BuildingType.INN:
                ret = Inn.Load(reader);
                break;
            case BuildingType.HALL:
                ret = Hall.Load(reader);
                break;
        }
        
        ret.Location= TriIsleland.Instance.grid.GetCell(coord);
        ret.EntranceDirection = entDir;
        ret.type = type;
        return ret;
    }
    public virtual void BindOptions(List<Button> buttons,Selector selector) {
        Button buildingOption = buttons[4];
        buttons[4].gameObject.SetActive(true);
        buildingOption.GetComponentInChildren<Text>().text = "Building\nOptions";
        buildingOption.onClick.AddListener(selector.ToBuildingOption);
    }
    public static bool IsBuildable(TriDirection dir,TriCoordinates coord) {
        return false;
    }
}
