﻿using UnityEngine;
using System.Collections.Generic;
using System.IO;

public enum CompType {
    WAREHOUSE, CONSTRUCTOR, FARM, LOGGING, MINE, PROCESSOR
}
public class Company : Building, Commandable {
    public List<Unit> Officers;
    public CompType subType;
    public int Capacity;
    public Human ReceiveableMan;
    public override void Save(BinaryWriter writer) {
        base.Save(writer);
        writer.Write((int)subType);
        writer.Write(Capacity);
    }

    public override void OnDeconstruct() {
        base.OnDeconstruct();
        for (int i = 0; i < (this as Company).Officers.Count; i++) {
            (this as Company).Officers[i].AddCommand(new ChangeJobCommand(null));
        }
    }
    public new static Company Load(BinaryReader reader) {
        CompType subType = (CompType)reader.ReadInt32();
        int capacity = reader.ReadInt32();
        Company ret = null;
        switch (subType) {
            case CompType.WAREHOUSE:
                ret = WareHouse.Load(reader);
                break;
        }
        ret.subType = subType;
        ret.Capacity = capacity;
        return ret;
    }
    public void ShowOfficers() {
        personList.Bind(this, Officers);
    }
    public override void Tick() {
        base.Tick();
        ReceiveableMan = null;
        if (Clock.IsDay())
            foreach (Human t in Officers) {
                if (Insider.Contains(t) && !t.acting)
                    ReceiveableMan = t;
            }
    }

    public bool HasCommandReceiver() {
        return ReceiveableMan;
    }

    public Unit GetCommandReceiver() {
        return ReceiveableMan;
    }
}
