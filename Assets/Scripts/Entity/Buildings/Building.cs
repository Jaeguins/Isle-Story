using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
public enum BuildingType {
    INN
}
public class Building : Entity {
    [NonSerialized]
    public BuildingType type;
    TriCell EntranceCell;
    
    TriDirection EntranceDirection;

    public new void Save(BinaryWriter writer) {
        base.Save(writer);
        writer.Write((int)type);
        writer.Write((int)EntranceDirection);
        EntranceCell.coordinates.Save(writer);
    }
    public static new Building Load(BinaryReader reader) {
        BuildingType type = (BuildingType)reader.ReadInt32();
        TriDirection entDir=(TriDirection)reader.ReadInt32();
        TriCoordinates coord = TriCoordinates.Load(reader);
        Building ret=null;
        switch (type) {
            case BuildingType.INN:
                ret = Inn.Load(reader);
                break;
        }
        ret.EntranceDirection = entDir;
        ret.EntranceCell = Isleland.Instance.grid.GetCell(coord);
        ret.type = type;
        return ret;
    }
}
