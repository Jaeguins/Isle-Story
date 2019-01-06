using UnityEngine;
using System.Collections.Generic;
using System.IO;
using UnityEngine.UI;

public enum InnType {
    TENT, CAMP
}
public class Inn : Building {
    public List<Unit> Livers;

    public InnType subType;
    public int Capacity;

    public override void Save(BinaryWriter writer) {
        base.Save(writer);
        writer.Write((int)subType);
        writer.Write(Capacity);
    }
    public void AddLiver(Person p) {
        Livers.Add(p);
    }
    public void RemoveLiver(Person p) {
        Livers.Remove(p);
    }
    public new static Inn Load(BinaryReader reader) {
        InnType subType = (InnType)reader.ReadInt32();
        int capacity = reader.ReadInt32();
        Inn ret = null;
        switch (subType) {
            case InnType.TENT:
                ret = Tent.Load(reader);
                break;
            case InnType.CAMP:
                ret = Camp.Load(reader);
                break;
        }
        ret.subType = subType;
        ret.Capacity = capacity;
        return ret;
    }
    public override void BindOptions(CommandPanel menu) {
        base.BindOptions(menu);
        if (UnderConstruct) return;
        if (Livers.Count > 0)
            menu.BindButton(1, "Livers", ShowLivers);

    }
    public void ShowLivers() {
        personList.Bind(this, Livers);
    }
    public override void Tick() {
        base.Tick();
        if (Clock.IsDay())
            foreach (Unit t in Livers) {
                if (Insider.Contains(t)) {
                    if (((Person)t).Company)
                        t.GoJob();
                    else t.GoWork();
                }
            }
    }
}
