using System;

using System.Collections.Generic;
using System.Linq;
using System.IO;
using UnityEngine;

public class Farmland : Worksite{
    public static new Farmland Load(BinaryReader reader) {
        return Instantiate((Farmland)TriIsland.GetBuildingPrefabs((int)BuildingType.WORKSITE, (int)WorkType.FARMLAND, 0));
    }
    public new void Save(BinaryWriter writer) {
        base.Save(writer);
    }
    public override void OnBuilt() {
        base.OnBuilt();
        Location.TerrainTypeIndex = 1;

    }
    public new void Start() {
        base.Start();
        Capacity = 4;
    }
}

