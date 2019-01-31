﻿using UnityEngine;
using System.Collections;
using System.IO;

public class Farm : Company {
    public override void Start() {
        base.Start();
    }
    public override void Save(BinaryWriter writer) {
        base.Save(writer);
    }
    public new static LoggingFacility Load(BinaryReader reader) {
        LoggingFacility ret = (LoggingFacility)Instantiate(TriIsland.GetBuildingPrefabs((int)BuildingType.COMPANY, (int)CompType.FARM, 0));
        return ret;
    }
}
