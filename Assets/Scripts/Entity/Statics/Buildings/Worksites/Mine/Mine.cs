using UnityEngine;
using System.Collections;
using System.IO;

public class Mine : Worksite {
    public override void Start() {
        base.Start();
    }
    public override void Save(BinaryWriter writer) {
        base.Save(writer);
    }
    public new static Mine Load(BinaryReader reader) {
        Mine ret = (Mine)Instantiate(TriIsland.GetBuildingPrefabs((int)BuildingType.WORKSITE, (int)WorkType.MINE, 0));
        return ret;
    }
}
