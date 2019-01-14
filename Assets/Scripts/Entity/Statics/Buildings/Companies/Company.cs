using UnityEngine;
using System.Collections.Generic;
using System.IO;

public enum CompType {
    WAREHOUSE,CONSTRUCTOR,FARM,LOGGING,MINE,PROCESSOR
}
public class Company : Building{
    public List<Unit> Officers;
    public CompType subType;
    public int Capacity;

    public override void Save(BinaryWriter writer) {
        base.Save(writer);
        writer.Write((int)subType);
        writer.Write(Capacity);
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
        if (Clock.IsDay())
            foreach(Unit t in Officers) {
                if (Insider.Contains(t)&&((Person)t).Work) {
                    t.GoWork();
                }
            }
        else
            foreach(Unit t in Officers) {
                if (Insider.Contains(t)) {
                    t.GoHome();
                }
            }
    }
}