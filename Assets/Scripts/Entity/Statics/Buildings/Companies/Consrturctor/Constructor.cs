using UnityEngine;
using System.Collections;
using System.IO;

public class Constructor : Company{
    public override void Start() {
        base.Start();
    }
    public override void Save(BinaryWriter writer) {
        base.Save(writer);
    }
    public new static LoggingFacility Load(BinaryReader reader) {
        LoggingFacility ret = (LoggingFacility)Instantiate(TriIsleland.GetBuildingPrefabs((int)BuildingType.COMPANY,(int)CompType.CONSTRUCTOR,0));
        return ret;
    }
}
