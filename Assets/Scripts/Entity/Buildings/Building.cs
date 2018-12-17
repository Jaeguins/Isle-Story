using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using UnityEngine.UI;
public enum BuildingType {
    INN
}
public enum SizeType {
    SINGLE,HEX
}
public class Building : Entity {
    
    public BuildingType type;
    public PersonList liverList;

    public TriDirection entranceDirection;

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
        }
        
        ret.Location= TriIsleland.Instance.grid.GetCell(coord);
        ret.EntranceDirection = entDir;
        ret.type = type;
        return ret;
    }
    public override void BindOptions(EntityMenu menu) {
        menu.BindButton(4, "Preference", GameUI.Instance.ToPreference);
    }
    private void OnMouseDown() {
        EntityMenu.Instance.Bind(this);
    }
}
