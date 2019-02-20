using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public enum HallType {
    BASE
}
public class Hall : Building, Buildable {
    public List<Unit> Homeless;
    public HallType subType;

    public override void Save(BinaryWriter writer) {
        base.Save(writer);
        writer.Write((int)subType);
    }

    public new static Hall Load(BinaryReader reader) {
        HallType subType = (HallType)reader.ReadInt32();
        Hall ret = null;
        switch (subType) {
            case HallType.BASE:
                ret = Camp.Load(reader);
                break;
        }
        ret.subType = subType;
        return ret;
    }
    public override void Tick() {
        base.Tick();
        ReceiveableMan = null;
        if (Clock.IsDay())
            foreach (Human t in Homeless) {
                if (Insider.Contains(t) && !t.acting)
                    ReceiveableMan = t;
            }
    }
    public Human ReceiveableMan;
    public bool HasCommandReceiver() {
        return ReceiveableMan;
    }

    public int GetTech() {
        return 0;
    }

    public void SendCommand(Command c) {
        ReceiveableMan.AddCommand(c);
    }

    public Unit GetCommandReceiver() {
        return ReceiveableMan;
    }
    public override List<BuildState> GetBuildStatus(TriCoordinates coord,TriDirection dir) {
        List<BuildState> ret = new List<BuildState>();
        TriCell k = TriGrid.Instance.GetCell(coord);
        int elev = k.Elevation;
        TriDirection tDir = dir.Previous();
        for (int i = 0; i < 6; i++) {
            if (!k) break;
            ret.Add(new BuildState() {
                coord = k.coordinates,
                value = k.IsBuildable()&&elev==k.Elevation
            });
            k = k.GetNeighbor(tDir);
            tDir = tDir.Next();
        }
        TriCell entrance = TriGrid.Instance.GetCell(coord).GetNeighbor(dir);
        ret.Add(new BuildState() {
            coord = entrance.coordinates,
            value = entrance.IsBuildable() && Mathf.Abs(entrance.GetNeighbor(dir).Elevation - elev) < 2
        });
        return ret;
    }
    public override void BindCells(bool flag) {
        TriCell k = Location;
        int elev = k.Elevation;
        TriDirection tDir = EntranceDirection.Previous();
        for (int i = 0; i < 6; i++) {
            if (!k) break;
            k.Statics = flag?this:null;
            k = k.GetNeighbor(tDir);
            tDir = tDir.Next();
        }
    }
}
