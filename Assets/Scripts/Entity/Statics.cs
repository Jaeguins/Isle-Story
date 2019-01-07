using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Statics:Entity{
    public bool Working=false;
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
    public List<Unit> Insider;
    public List<Unit> Workers;

    public override void Tick() {
        base.Tick();
        if (!Clock.IsDay())
            foreach (Unit t in Workers) {
                if (Insider.Contains(t)) {
                    if (((Person)t).Company)
                        t.GoJob();
                    else t.GoHome();
                }
            }
    }
}
