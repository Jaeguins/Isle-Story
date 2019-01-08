using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public enum HallType {
    BASE
}
public class Hall : Building{
    public List<Unit> Homeless;
    public HallType subType;
    public override void Save(BinaryWriter writer) {
        base.Save(writer);
        writer.Write((int)subType);
    }

    public new static Hall Load(BinaryReader reader) {
        HallType subType = (HallType)reader.ReadInt32();
        int capacity = reader.ReadInt32();
        Hall ret = null;
        switch (subType) {
            case HallType.BASE:
                ret = Camp.Load(reader);
                break;
        }
        ret.subType = subType;
        return ret;
    }
    public override void Tick() {
        base.Tick();
        if (Clock.IsDay())
            foreach (Unit t in Homeless) {
                if (Insider.Contains(t)) {
                    if (((Person)t).Company)
                        t.GoJob();
                    else t.GoWork();
                }
            }
    }
}
