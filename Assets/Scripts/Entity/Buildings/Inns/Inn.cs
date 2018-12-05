using UnityEngine;
using System.Collections.Generic;
using System.IO;
public enum InnType {
    TENT
}
public class Inn : Building {
    List<Person> livers;
    public new BuildingType type = BuildingType.INN;
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

    public static Inn Load(BinaryReader reader) {
        InnType subType = (InnType)reader.ReadInt32();
        int capacity = reader.ReadInt32();
        Inn ret = null;
        switch (subType) {
            case InnType.TENT:
                Tent.Load(reader);
                break;
        }
        ret.subType = subType;
        ret.capacity = capacity;
        return ret;
    }
}
