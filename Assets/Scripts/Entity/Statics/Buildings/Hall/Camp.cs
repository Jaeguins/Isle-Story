using UnityEngine;
using System.Collections.Generic;
using System.IO;
public class Camp : Hall {
    private void Awake() {
        camAnchorOffset = new Vector3(TriMetrics.innerRadius, 5, TriMetrics.outerRadius);
    }

    public new void Save(BinaryWriter writer) {
        base.Save(writer);
    }

    public static new Camp Load(BinaryReader reader) {
        return Instantiate((Camp)TriIsleland.GetBuildingPrefabs((int)BuildingType.HALL,(int)HallType.BASE,0));
    }

    public override void BindOptions(CommandPanel menu) {
        base.BindOptions(menu);
    }
}
