using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
public enum TerrainModdingType {
    STOCK=1,DIG=-1,LANDFILL
}
public class TerrainModder : Worksite
{
    public TerrainModdingType workType;

    public override void Save(BinaryWriter writer) {
        base.Save(writer);
        writer.Write((int)workType);
    }
    public new static TerrainModder Load(BinaryReader reader) {
        int workType = reader.ReadInt32();
        TerrainModder ret = Instantiate((TerrainModder)TriIsland.GetBuildingPrefabs((int)BuildingType.WORKSITE, (int)WorkType.TERRAINMODDER, workType));
        return ret;
    }
    public override void OnBuilt() {
        base.OnBuilt();
        if (workType == TerrainModdingType.LANDFILL) {
            TriCell k = Location;
            for(int j = 0; j < 3; j++) {
                TriDirection tDir = (TriDirection)j;
                for (int i = 0; i < 6; i++) {
                    k.Elevation = k.Elevation < 1 ? 1 : k.Elevation;
                    k = k.GetNeighbor(tDir);
                    tDir = tDir.Next();
                }
            }
            
            
        }
        else {
            location.Elevation += (int)workType;
        }
        SelfWorking = true;
        DeconstructionStart(null);
    }
}
