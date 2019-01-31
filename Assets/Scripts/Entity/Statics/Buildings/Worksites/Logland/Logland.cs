using UnityEngine;
using System.Collections;
using System.IO;

public class Logland: Worksite{
    public static new Logland Load(BinaryReader reader) {
        return Instantiate((Logland)TriIsland.GetBuildingPrefabs((int)BuildingType.WORKSITE,(int)WorkType.FELLINGLAND,0));
    }
    public new void Save(BinaryWriter writer) {
        base.Save(writer);
    }
}
