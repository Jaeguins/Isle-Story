using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public enum WorkType {
    FARMLAND,FELLINGLAND
}
public class Worksite : Building{
    public int Capacity;
    public WorkType subType;
    public override void Save(BinaryWriter writer) {
        base.Save(writer);
        writer.Write((int)subType);
        writer.Write(Capacity);
    }
    public new static Worksite Load(BinaryReader reader) {
        WorkType subType = (WorkType)reader.ReadInt32();
        int capacity = reader.ReadInt32();
        Worksite ret = null;
        switch (subType) {
            case WorkType.FARMLAND:
                ret = Farmland.Load(reader);
                break;
        }
        ret.subType = subType;
        ret.Capacity = capacity;
        return ret;
    }
    public override void BindOptions(CommandPanel menu) {
        base.BindOptions(menu);
        if (UnderConstruct) return;
        if (Insider.Count > 0)
            menu.BindButton(1, "Workers", ShowWorkers);
    }
    public void ShowWorkers() {
        personList.Bind(this, Workers);
    }
}
