using UnityEngine;
using System.Collections.Generic;
using System.IO;
public class Camp : Hall {
    private void Start() {
        capacity = 4;
        UIName = "camp";
    }
    public new void Save(BinaryWriter writer) {
        base.Save(writer);
    }
    public static new Camp Load(BinaryReader reader) {
        return Instantiate((Camp)TriIsleland.Instance.hallPrefabs[(int)HallType.CAMP]);
    }
    public static new bool IsBuildable(TriDirection dir, TriCoordinates coord) {
        TriCell cell= TriGrid.Instance.GetCell(coord);
        int elevation = cell.Elevation;
        TriCell k = cell;
        int elev = cell.Elevation;
        TriDirection tDir = dir.Previous();
        for (int i = 0; i < 6; i++) {
            if (!k||!k.IsBuildable()) return false;
            if (elev != k.Elevation) return false;
            k = k.GetNeighbor(tDir);
            tDir = tDir.Next();
        }
        return true;
    }
}
