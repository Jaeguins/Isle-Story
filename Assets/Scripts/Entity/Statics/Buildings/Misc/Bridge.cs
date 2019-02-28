using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Bridge : MiscBuilding {
    public override List<BuildState> GetBuildStatus(TriCoordinates coord, TriDirection dir) {
        List<BuildState> ret = new List<BuildState>();
        TriCell cell = TriGrid.Instance.GetCell(coord);
        int elev = cell.Elevation;
        ret.Add(new BuildState(coord, cell.HasRiver));
        TriCell entrance = cell.GetNeighbor(dir);
        ret.Add(new BuildState(entrance.coordinates, entrance.IsBuildable() && Mathf.Abs(elev - entrance.Elevation) < 2));
        cell = cell.GetNeighbor(dir.Previous());
        ret.Add(new BuildState(cell.coordinates, cell.HasRiver&& Mathf.Abs(elev - cell.Elevation) < 2));
        cell = cell.GetNeighbor(dir);
        ret.Add(new BuildState(cell.coordinates, cell.IsBuildable()&& Mathf.Abs(elev - cell.Elevation) < 2));
        return ret;
    }
    public override void BindCells(bool flag) {
        Location.Statics = flag ? this : null;
        Location.IsBridged = flag;
        Location.GetNeighbor(entranceDirection.Previous()).Statics= Location.Statics = flag ? this : null;
        Location.GetNeighbor(entranceDirection.Previous()).IsBridged = flag;
    }
}
