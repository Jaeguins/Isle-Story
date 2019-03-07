using UnityEngine;
using System.Collections.Generic;
using System.IO;
using UnityEngine.UI;

public enum InnType {
    TENT
}
public class Inn : Building, Commandable,ISummary {
    public List<Unit> Livers;
    public InnType subType;
    public int Capacity;
    public float BirthSpeed = 1f;
    public float BirthStatus = 0f;
    public float BirthCap = 100f;
    public float BirthProgress {
        get {
            return BirthStatus / BirthCap;
        }
    }
    public Human personPrefab;
    public bool ReservLiver {
        get {
            return BirthStatus > 0f;
        }
    }
    public override void OnDeconstruct() {
        base.OnDeconstruct();
        for (int i = 0; i < (this as Inn).Livers.Count; i++) {
            (this as Inn).Livers[i].AddCommand(new ChangeHomeCommand(null));
        }
    }
    public override void Save(BinaryWriter writer) {
        base.Save(writer);
        writer.Write((int)subType);
        writer.Write(Capacity);
    }

    public new static Inn Load(BinaryReader reader) {
        InnType subType = (InnType)reader.ReadInt32();
        Inn ret = null;
        switch (subType) {
            case InnType.TENT:
                ret = Tent.Load(reader);
                break;
        }
        ret.subType = subType;
        return ret;
    }

    public void ShowLivers() {
        personList.Bind(this, Livers);
    }

    public override void Tick() {
        base.Tick();
        ReceiveableMan= null;
        if (Clock.IsDay()) {
            foreach (Human t in Livers) {
                if (Insider.Contains(t) && !t.acting) {
                    ReceiveableMan = t;
                }
            }
            if (Working && !UnderConstruct && !UnderDeconstruct) {
                if (Workers.Count > 0) {
                    BirthStatus += BirthSpeed;
                    if (BirthStatus >= BirthCap) {
                        BirthStatus = 0f;
                        Human t = Instantiate(personPrefab);
                        TriIsland.Instance.entities.AddUnit(t);
                        t.Location = Location;
                        t.Home = this;
                        t.AddCommand(new GetInCommand(this));
                        while (Workers.Count > 0) {
                            (Workers[0] as Human).Work = null;
                        }

                    }
                }
            }
        }
    }
    public bool CheckNewPerson() {
        return (Livers.Count < Capacity && HasCommandReceiver() && Workers.Count == 0);
    }
    public bool HasCommandReceiver() {
        return ReceiveableMan;
    }

    public Unit GetCommandReceiver() {
        return ReceiveableMan;
    }

    public Sprite GetProductSprite() {
        return null;
    }

    public int GetTotalPeople() {
        return Capacity;
    }

    public int GetNowPeople() {
        return Livers.Count;
    }

    public int GetSparePeople() {
        int ret = Livers.Count;
        foreach (Human t in Livers) {
            if (t.Company || t.Work) ret--;
        }
        return ret;
    }

    public float GetProdPercentage() {
        return UnderConstruct ? 1f - NowConstructTime / ConstructTime : BirthStatus;
    }

    public bool IsProducing() {
        return Working;
    }
}
