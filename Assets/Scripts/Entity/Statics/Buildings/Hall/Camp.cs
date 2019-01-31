using UnityEngine;
using System.Collections.Generic;
using System.IO;
public class Camp : Hall{
    
    public override void Awake() {
        base.Awake();
        camAnchorOffset = new Vector3(TriMetrics.innerRadius, 5, TriMetrics.outerRadius);
    }

    public new void Save(BinaryWriter writer) {
        base.Save(writer);
    }

    public static new Camp Load(BinaryReader reader) {
        return Instantiate((Camp)TriIsland.GetBuildingPrefabs((int)BuildingType.HALL,(int)HallType.BASE,0));
    }

    
}
