using UnityEngine;
using System.Collections.Generic;
using System.IO;
using UnityEngine.UI;

public enum InnType {
    TENT,CAMP
}
public class Inn : Building {
    public List<Person> livers;

    public InnType subType;
    protected int capacity;
    void setLiver(Person person) {
        if (livers.Count < 4) {
            livers.Add(person);
        }
    }
    public override void Save(BinaryWriter writer) {
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
            case InnType.CAMP:
                ret = Camp.Load(reader);
                break;
        }
        ret.subType = subType;
        ret.capacity = capacity;
        return ret;
    }
    public override void BindOptions(CommandPanel menu) {
        base.BindOptions(menu);
        menu.BindButton(5, "livers", ShowLivers);
    }
    public void ShowLivers() {
        if (livers.Count == 0) return;
        liverList.SetActive(true);
        liverList.ClearList();
        foreach(Person p in livers) {
            liverList.AddPerson(p);
        }
    }
}
