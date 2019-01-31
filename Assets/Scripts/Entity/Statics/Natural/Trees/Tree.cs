using UnityEngine;
using System.Collections.Generic;
using System.IO;
public class Tree : Natural{
    public override void Start() {
        base.Start();
        UIName = "tree";
        sizeType = SizeType.SINGLE;
    }
    public override void Save(BinaryWriter writer) {
        base.Save(writer);
    }

    public new static Tree Load(BinaryReader reader) {
        Tree ret = Instantiate((Tree)TriIsland.GetNaturalPrefabs((int)NaturalType.TREE,0));
        return ret;
    }
}
