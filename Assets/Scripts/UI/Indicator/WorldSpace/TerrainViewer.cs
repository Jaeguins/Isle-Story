using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TerrainViewer : TriMesh {
    public static TerrainViewer Instance;
    Selector selector;
    TriCell k;
    public List<BuildState> result;
    public bool Buildable;
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
    public new void Clear() {
        base.Clear();
        Buildable = true;
    }
    public void CalculateTerrain() {
        Clear();
        TriGrid grid = TriGrid.Instance;
        result = selector.Prefab.GetBuildStatus(selector.nowCell.coordinates,selector.dir);
        if (selector.nowCell)
            foreach (BuildState i in result) {
                RecalculateTerrain(grid.GetCell(i.coord), i.value);
                if (!i.value) Buildable = false;
            }
                
        Apply();
    }
    public void RecalculateTerrain(TriCell cell, bool buildable) {
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
