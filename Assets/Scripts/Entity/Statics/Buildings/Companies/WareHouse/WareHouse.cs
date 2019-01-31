using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class WareHouse : Company {
    /*
     * save sequence
     * supperclassed saves
     */
    public override void Start() {
        base.Start();
    }
    public override void Save(BinaryWriter writer) {
        base.Save(writer);
    }
    public new static WareHouse Load(BinaryReader reader) {
        WareHouse ret = (WareHouse)Instantiate(TriIsland.GetBuildingPrefabs((int)BuildingType.COMPANY, (int)CompType.WAREHOUSE, 0));
        return ret;
    }
}
