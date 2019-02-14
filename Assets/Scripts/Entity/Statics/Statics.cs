using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Statics : Entity {
    public TriDirection entranceDirection;
    public TriDirection EntranceDirection {
        get {
            return entranceDirection;
        }
        set {
            entranceDirection = value;
            Vector3 rot = new Vector3(0, (int)entranceDirection * 120 + (Location.inverted ? 0 : 180), 0);
            transform.localRotation = Quaternion.Euler(rot);
        }
    }
    public TriCell EntranceLocation {
        get {
            return Location.GetNeighbor(EntranceDirection);
        }
    }
    public bool UnderConstruct = true;
    public bool UnderDeconstruct = false;
    public float NowConstructTime = 9999f;
    public float ConstructTime = 9999f;
    public List<Unit> Insider;
    public List<Unit> Workers;

    public override void Tick() {
        base.Tick();
        if (!Clock.IsDay())
            foreach (Unit t in Workers) {
            }
    }

    public virtual void DeconstructionStart(Unit Starter) {
        UnderDeconstruct = true;
        for (int i = 0; i < Workers.Count; i++) {
            if (Workers[i] != Starter) {
                Workers[i].AddCommand(new ChangeWorkCommand(null));
                if (Workers[i].Building == this)
                    Workers[i].AddCommand(new GetOutCommand());
            }
        }
        if (this is Inn) {
            for (int i = 0; i < (this as Inn).Livers.Count; i++) {
                (this as Inn).Livers[i].AddCommand(new ChangeHomeCommand(null));
            }
        }
        if (this is Company) {
            for (int i = 0; i < (this as Company).Officers.Count; i++) {
                (this as Company).Officers[i].AddCommand(new ChangeJobCommand(null));
            }
        }
    }

    public virtual void CheckConstruction() {
        if (NowConstructTime < 0) {
            UnderConstruct = false;
            Working = true;
            for (int i = 0; i < Insider.Count; i++) {
                if (Workers.Contains(Insider[i])) {
                    Insider[i].AddCommand(new ChangeWorkCommand(null));
                    Insider[i].AddCommand(new GetOutCommand());
                }
            }
        }
        else
            NowConstructTime -= Time.deltaTime * Insider.Count;
    }

    public virtual void CheckDeConstruction() {
        NowConstructTime += Time.deltaTime * (Insider.Count);
        if (NowConstructTime > .5f * ConstructTime) {
            TriIsland.Instance.entities.RemoveBuilding(ID);
        }
    }
    public virtual void LateUpdate() {
        if (UnderConstruct) {
            CheckConstruction();
        }
        if (UnderDeconstruct) {
            CheckDeConstruction();
        }
    }
}
