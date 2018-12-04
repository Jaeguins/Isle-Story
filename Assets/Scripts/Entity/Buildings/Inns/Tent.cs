using UnityEngine;
using System.Collections.Generic;
using System.IO;
public class Tent : Inn {
    public new InnType subType = InnType.TENT;
    private void Awake() {
        this.capacity = 4;
    }
    public new void Save(BinaryWriter writer,int index) {
        base.Save(writer, index);
    }
    public static new Tent Load(BinaryReader reader) {
        return Instantiate((Tent)IsleLand.Instance.innPrefabs[(int)InnType.TENT]);
    }
}
