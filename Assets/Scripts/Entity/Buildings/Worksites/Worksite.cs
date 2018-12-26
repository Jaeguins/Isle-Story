using UnityEngine;
using System.Collections;
using System.IO;

public enum WorkType {
    FARMLAND
}
public class Worksite : Building{
    public int Capacity;
    public WorkType subType;

    public override void Save(BinaryWriter writer) {
        base.Save(writer);
        writer.Write((int)subType);
        writer.Write(Capacity);
    }
    public void AddPerson(Person p) {
        Workers.Add(p);
    }
    public void RemovePerson(Person p) {
        Workers.Remove(p);
    }
    public new static Worksite Load(BinaryReader reader) {
        WorkType subType = (WorkType)reader.ReadInt32();
        int capacity = reader.ReadInt32();
        Worksite ret = null;
        switch (subType) {

        }
        ret.subType = subType;
        ret.Capacity = capacity;
        return ret;
    }
    public override void BindOptions(CommandPanel menu) {
        base.BindOptions(menu);
        if (UnderConstruct) return;
        if (Workers.Count > 0)
            menu.BindButton(1, "Workers", ShowWorkers);
    }
    public void ShowWorkers() {
        personList.Bind(this, Workers);
    }
}
