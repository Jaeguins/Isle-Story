using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

public class Farmland : Worksite{
    public static new Farmland Load(BinaryReader reader) {
        return Instantiate((Farmland)TriIsleland.Instance.worksitePrefabs[(int)WorkType.FARMLAND]);
    }
    public new void Save(BinaryWriter writer) {
        base.Save(writer);
    }
}

