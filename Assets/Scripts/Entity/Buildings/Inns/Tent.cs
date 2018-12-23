using UnityEngine;
using System.Collections.Generic;
using System.IO;
public class Tent : Inn {
    private void Start() {
        ConstructTime = 20f;
        Capacity = 4;
        UIName = "tent";
        camAnchorOffset = Vector3.zero;
        camOffset = new Vector3(5, 5, -20);
        sizeType = SizeType.SINGLE;
    }
    public override void Save(BinaryWriter writer) {
        base.Save(writer);
    }
    public static new Tent Load(BinaryReader reader) {
        return Instantiate((Tent)TriIsleland.Instance.innPrefabs[(int)InnType.TENT]);
    }
}
