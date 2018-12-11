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
        bool inverted = cell.inverted;
        TriDirection d = TriDirection.VERT;
        for (int i = 0; i < 6; i++) {
            if (!k) return false;
            if (k.Elevation != elevation) return false;
            if (k.Building) return false;
            k = k.GetNeighbor(d);
            if (inverted)
                d = d.Next();
            else
                d = d.Previous();
        }
        return true;
    }
}
