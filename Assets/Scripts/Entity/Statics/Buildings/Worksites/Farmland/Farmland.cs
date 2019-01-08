using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

public class Farmland : Worksite{
    public static new Farmland Load(BinaryReader reader) {
        return Instantiate((Farmland)TriIsleland.GetBuildingPrefabs((int)BuildingType.WORKSITE, (int)WorkType.FARMLAND, 0));
    }
    public new void Save(BinaryWriter writer) {
        base.Save(writer);
    }
}

