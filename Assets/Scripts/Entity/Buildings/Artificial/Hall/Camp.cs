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
}
