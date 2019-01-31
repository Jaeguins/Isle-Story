using UnityEngine;
using System.Collections;
using System.IO;

public class Processor : Company {
    public override void Start() {
        base.Start();
    }
    public override void Save(BinaryWriter writer) {
        base.Save(writer);
    }
    public new static Processor Load(BinaryReader reader) {
        Processor ret = (Processor)Instantiate(TriIsland.GetBuildingPrefabs((int)BuildingType.COMPANY, (int)CompType.PROCESSOR, 0));
        return ret;
    }
}
