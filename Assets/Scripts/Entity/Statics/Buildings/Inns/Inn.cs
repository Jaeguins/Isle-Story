using UnityEngine;
using System.Collections.Generic;
using System.IO;
using UnityEngine.UI;

public enum InnType {
    TENT
}
public class Inn : Building,Commandable {
    public List<Unit> Livers;
    public InnType subType;
    public int Capacity;
    public Unit CommandReceiver;
    public float BirthSpeed=1f;
    public float BirthStatus = 0f;
    public Person personPrefab;
    public override void Save(BinaryWriter writer) {
        base.Save(writer);
        writer.Write((int)subType);
        writer.Write(Capacity);
    }

    public new static Inn Load(BinaryReader reader) {
        InnType subType = (InnType)reader.ReadInt32();
        int capacity = reader.ReadInt32();
        Inn ret = null;
        switch (subType) {
            case InnType.TENT:
                ret = Tent.Load(reader);
                break;
        }
        ret.subType = subType;
        ret.Capacity = capacity;
        return ret;
    }

    public void ShowLivers() {
        personList.Bind(this, Livers);
    }

    public override void Tick() {
        base.Tick();
        CommandReceiver = null;
        if (Clock.IsDay()) {
            foreach (Person t in Livers) {
                if (Insider.Contains(t)) {
                    if (t.Company)
                        t.GoJob();
                    else if (t.Work)
                        t.GoWork();
                    else
                        CommandReceiver = t;
                }
            }
            if (Working && !UnderConstruct) {
                if (Workers.Count > 0) {
                    BirthStatus += BirthSpeed;
                    if (BirthStatus >= 100) {
                        BirthStatus = 0f;
                        Person t=Instantiate(personPrefab);
                        t.Location = Location;
                        t.Home = this;
                        t.AddCommand(new GetInCommand(this));
                        
                        TriIsleland.Instance.entities.AddUnit(t);
                    }
                }
            }
        }
    }

    public bool HasCommandReceiver() {
        return CommandReceiver;
    }

    public Unit GetCommandReceiver() {
        return CommandReceiver;
    }
}
