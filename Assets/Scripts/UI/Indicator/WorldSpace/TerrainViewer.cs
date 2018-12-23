using UnityEngine;
using System.Collections;

public class TerrainViewer : TriMesh{
    public static TerrainViewer Instance;
    Selector selector;
    TriCell k;
    new void Awake() {
        base.Awake();
        Instance = this;
    }
    private void Start() {
        selector = Selector.Instance;
    }
    void LateUpdate() {
        CalculateTerrain();
    }
    public void CalculateTerrain() {
        Clear();
        
        if (selector.nowCell) {
            TriCell neighbor = selector.nowCell.GetNeighbor(selector.dir);
            if (neighbor) RecalculateTerrain(neighbor,neighbor.IsBuildable()&&neighbor.Elevation==selector.nowCell.Elevation);
            switch (selector.sizeType) {
                case SizeType.HEX:
                    k = selector.nowCell;
                    int elev = selector.nowCell.Elevation;
                    TriDirection tDir = selector.dir.Previous();
                    for (int i = 0; i < 6; i++) {
                        if (!k) break;
                        RecalculateTerrain(k, k.IsBuildable() && k.Elevation == elev);
                        k = k.GetNeighbor(tDir);
                        tDir = tDir.Next();
                    }
                    break;
                case SizeType.SINGLE:
                    RecalculateTerrain(selector.nowCell, selector.nowCell.IsBuildable());
                    break;
            }
        }
        Apply();
    }
    public void RecalculateTerrain(TriCell cell,bool buildable) {
        Vector3 nextCorner, prevCorner;
        EdgeVertices edge;
        for (TriDirection direction = TriDirection.VERT; direction <= TriDirection.RIGHT; direction++) {
            Vector3 center = cell.Position, v1, v2;
            //entityMenu.transform.localPosition = center + new Vector3(0, 20, 0);
            v1 = center + (cell.inverted ? -1 : 1) * TriMetrics.GetFirstSolidCorner(direction);
            v2 = center + (cell.inverted ? -1 : 1) * TriMetrics.GetSecondSolidCorner(direction);
            edge = new EdgeVertices(v1, v2);
            nextCorner = (center + edge.v1) / 2f;
            prevCorner = (center + edge.v5) / 2f;
            AddTriangle(nextCorner, edge.v1, edge.v2);
            AddTriangle(prevCorner, edge.v4, edge.v5);
            AddTriangle(center, edge.v2, edge.v4);
            AddTriangle(center, edge.v4, prevCorner);
            AddTriangle(center, nextCorner, edge.v2);
            Color c = buildable ? Color.blue : Color.red;
            AddTriangleColor(c);
            AddTriangleColor(c);
            AddTriangleColor(c);
            AddTriangleColor(c);
            AddTriangleColor(c);
        }
    }
}
