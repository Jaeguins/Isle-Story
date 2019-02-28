using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

public enum MiscType {
    BRIDGE
}
public class MiscBuilding : Building {
    public MiscType subType;
    public override void Save(BinaryWriter writer) {
        base.Save(writer);
        writer.Write((int)subType);
    }
    public new static MiscBuilding Load(BinaryReader reader) {
        MiscType subType = (MiscType)reader.ReadInt32();
        MiscBuilding ret = null;
        switch (subType) {
            case MiscType.BRIDGE:
                ret = Bridge.Load(reader);
                break;
        }
        ret.subType = subType;
        return ret;
    }
}