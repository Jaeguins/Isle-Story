using UnityEngine;
using System.Collections.Generic;
using System.IO;
public class Tent : Inn {
    private void Start() {
        capacity = 4;
        UIName = "tent";
    }
    public new void Save(BinaryWriter writer) {
        base.Save(writer);
    }
    public static new Tent Load(BinaryReader reader) {
        return Instantiate((Tent)TriIsleland.Instance.innPrefabs[(int)InnType.TENT]);
    }
    public static new bool IsBuildable(TriDirection dir, TriCoordinates coord) {
        if (TriGrid.Instance.GetCell(coord).Building) return false;
        else return true;
    }
}
