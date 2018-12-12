using UnityEngine;
using System.Collections.Generic;
using System.IO;
using UnityEngine.UI;

public enum InnType {
    TENT
}
public class Inn : Building {
    List<Person> livers;
    public InnType subType;
    protected int capacity;
    void setLiver(Person person) {
        if (livers.Count < 4) {
            livers.Add(person);
        }
    }
    public new void Save(BinaryWriter writer) {
        base.Save(writer);
        writer.Write((int)subType);
        writer.Write(capacity);
    }
    public void addPerson(Person p) {
        livers.Add(p);
    }
    public void removePerson(Person p) {
        livers.Remove(p);
    }
    public new static Inn Load(BinaryReader reader) {
        InnType subType = (InnType)reader.ReadInt32();
        int capacity = reader.ReadInt32();
        Inn ret = null;
        switch (subType) {
            case InnType.TENT:
                ret=Tent.Load(reader);
                break;
        }
        ret.subType = subType;
        ret.capacity = capacity;
        return ret;
    }
    public override void BindOptions(BuildingMenu menu) {
        menu.BindButton(5, "livers", null);

    }
}
