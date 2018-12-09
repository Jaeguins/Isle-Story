using UnityEngine;
using System.Collections.Generic;
using System.IO;
public class Tree : Natural{
    public new void Save(BinaryWriter writer) {
        base.Save(writer);
    }

    public new static Tree Load(BinaryReader reader) {
        Tree ret = Instantiate((Tree)TriIsleland.Instance.naturalPrefabs[(int)NaturalType.TREE]);
        return ret;
    }
}
