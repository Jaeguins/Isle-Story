using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public enum HallType {
    BASE
}
public class Hall : Building, Buildable {
    public List<Unit> Homeless;
    public HallType subType;

    public override void Save(BinaryWriter writer) {
        base.Save(writer);
        writer.Write((int)subType);
    }

    public new static Hall Load(BinaryReader reader) {
        HallType subType = (HallType)reader.ReadInt32();
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
        ReceiveableMan = null;
        if (Clock.IsDay())
            foreach (Person t in Homeless) {
                if (Insider.Contains(t) && !t.acting)
                    ReceiveableMan = t;
            }
    }
    public Person ReceiveableMan;
    public bool HasCommandReceiver() {
        return ReceiveableMan;
    }

    public int GetTech() {
        return 0;
    }

    public void SendCommand(Command c) {
        ReceiveableMan.AddCommand(c);
    }

    public Unit GetCommandReceiver() {
        return ReceiveableMan;
    }
}
