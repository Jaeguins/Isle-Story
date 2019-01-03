using UnityEngine;
using System.Collections.Generic;
using System.IO;

public enum CompType {
    WAREHOUSE
}
public class Company : Building{
    public List<Person> Officers;
    public CompType subType;
    public int Capacity;

    public override void Save(BinaryWriter writer) {
        base.Save(writer);
        writer.Write((int)subType);
        writer.Write(Capacity);
    }
    public void AddPerson(Person p) {
        Officers.Add(p);
    }
    public void RemovePerson(Person p) {
        Officers.Remove(p);
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
    public override void BindOptions(CommandPanel menu) {
        base.BindOptions(menu);
        if (UnderConstruct) return;
        if (Officers.Count > 0)
            menu.BindButton(1, "Officers", ShowOfficers);
    }
    public void ShowOfficers() {
        personList.Bind(this, Officers);
    }
}