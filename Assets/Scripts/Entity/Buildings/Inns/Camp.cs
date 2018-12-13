using UnityEngine;
using System.Collections.Generic;
using System.IO;
public class Camp : Inn {
    private void Start() {
        capacity = 4;
        UIName = "camp";
        sizeType = SizeType.HEX;
        camAnchorOffset = new Vector3(TriMetrics.innerRadius, 5, TriMetrics.outerRadius);
        camOffset = new Vector3(10, 10, -40);
    }
    public new void Save(BinaryWriter writer) {
        base.Save(writer);
    }
    public static new Camp Load(BinaryReader reader) {
        return Instantiate((Camp)TriIsleland.Instance.innPrefabs[(int)InnType.CAMP]);
    }
}
