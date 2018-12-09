using UnityEngine;
using System.Collections.Generic;
using System.IO;
public class Tent : Inn {
    private void Awake() {
        capacity = 4;
    }
    public new void Save(BinaryWriter writer) {
        base.Save(writer);
    }
    public static new Tent Load(BinaryReader reader) {
        return Instantiate((Tent)TriIsleland.Instance.innPrefabs[(int)InnType.TENT]);
    }
}
