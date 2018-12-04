using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TriGridChunk : MonoBehaviour {
    TriCell[] cells;
    public TriMesh terrain, rivers;
    public void Disable() {
        terrain.enabled = false;
        rivers.enabled = false;
    }
    void Awake() {
        cells = new TriCell[TriMetrics.chunkSizeX * TriMetrics.chunkSizeZ];
    }

    public void AddCell(int index, TriCell cell) {
        cells[index] = cell;
        cell.chunk = this;
        cell.transform.SetParent(transform, false);
    }

    public void Refresh() {
        enabled = true;
    }

    private void LateUpdate() {
        if (enabled) {
            Triangulate();
            enabled = false;
        }
    }

    public void Triangulate() {
        terrain.Clear();
        rivers.Clear();
        for (int i = 0; i < cells.Length; i++) {
            if(cells[i])Triangulate(cells[i]);
        }
        rivers.Apply();
        terrain.Apply();
    }

    void Triangulate(TriCell cell) {
        for (TriDirection d = TriDirection.VERT; d <= TriDirection.RIGHT; d++) {
            Triangulate(d, cell);
        }
    }

    void Triangulate(TriDirection direction, TriCell cell) {
        int inverter = 0;
        if (cell.inverted) inverter = -1;
        else inverter = 1;

        Vector3 center = cell.Position, v1, v2;

        v1 = center + inverter * TriMetrics.GetFirstSolidCorner(direction);
        v2 = center + inverter * TriMetrics.GetSecondSolidCorner(direction);
        EdgeVertices e = new EdgeVertices(v1, v2);
        if (cell.IsUnderwater) {
            TriangulateWater(center, e);
        }
        if (cell.HasRiverThroughEdge(direction)) {
            TriangulateWithRiver(cell, direction, center, e, cell.Color);
        }
        else {
            TriangulateEdgeFan(center, e, cell.Color);
        }
        if (cell.inverted) {
            if (direction != TriDirection.LEFT)
                TriangulateConnection(direction, cell, e, inverter);
        }
        else {
            if (direction == TriDirection.LEFT)
                TriangulateConnection(direction, cell, e, inverter);
        }


    }

    void TriangulateRiverQuad(Vector3 v1, Vector3 v2, Vector3 v3, Vector3 v4, float y, bool reversed) {
        v1.y = v2.y = v3.y = v4.y = y;
        rivers.AddQuad(v1, v2, v3, v4);
        if (reversed) {
            rivers.AddQuadColor(Color.blue, Color.blue);
        }
        else {
            rivers.AddQuadColor(Color.blue, Color.blue);
        }
    }

    void TriangulateConnection(TriDirection direction, TriCell cell, EdgeVertices e1, int inverter) {
        Vector3 center = cell.transform.localPosition;
        TriCell neighbor = cell.GetNeighbor(direction);
        if (neighbor == null) return;
        Vector3 bridge = TriMetrics.GetBridge(direction);
        bridge.y = neighbor.Position.y - cell.Position.y;
        EdgeVertices e2 = new EdgeVertices(
            e1.v1 + bridge,
            e1.v5 + bridge
            );
        TriangulateEdgeStrip(terrain, e1, cell.Color, e2, neighbor.Color, cell.HasRiverThroughEdge(direction));

    }

    void TriangulateWater(Vector3 center, EdgeVertices edge) {

        Vector3 nextCorner, prevCorner, offset = new Vector3(0, TriMetrics.waterElevationOffset - TriMetrics.streamBedElevationOffset, 0);
        center += offset;
        edge += offset;
        nextCorner = (center + edge.v1) / 2f;
        prevCorner = (center + edge.v5) / 2f;
        rivers.AddTriangle(nextCorner, edge.v1, edge.v2);
        rivers.AddTriangle(prevCorner, edge.v4, edge.v5);
        rivers.AddTriangle(center, edge.v2, edge.v4);
        rivers.AddTriangle(center, edge.v4, prevCorner);
        rivers.AddTriangle(center, nextCorner, edge.v2);

        rivers.AddTriangleColor(Color.blue);
        rivers.AddTriangleColor(Color.blue);
        rivers.AddTriangleColor(Color.blue);
        rivers.AddTriangleColor(Color.blue);
        rivers.AddTriangleColor(Color.blue);
    }

    void TriangulateWithRiver(TriCell cell, TriDirection direction, Vector3 center, EdgeVertices edge, Color color) {
        Vector3 riverOffset = new Vector3(0, TriMetrics.streamBedElevationOffset - TriMetrics.waterElevationOffset, 0);
        Vector3 nextCorner, prevCorner;
        nextCorner = (center + edge.v1) / 2f;
        prevCorner = (center + edge.v5) / 2f;
        Vector3 underCenter = center, underNextCorner = nextCorner, underPrevCorner = prevCorner;
        underCenter.y += TriMetrics.streamBedElevationOffset;
        underNextCorner.y += TriMetrics.streamBedElevationOffset;
        underPrevCorner.y += TriMetrics.streamBedElevationOffset;

        EdgeVertices underEdge = edge;
        underEdge.v1.y += TriMetrics.streamBedElevationOffset;
        underEdge.v2.y += TriMetrics.streamBedElevationOffset;
        underEdge.v3.y += TriMetrics.streamBedElevationOffset;
        underEdge.v4.y += TriMetrics.streamBedElevationOffset;
        underEdge.v5.y += TriMetrics.streamBedElevationOffset;

        terrain.AddTriangle(nextCorner, edge.v1, edge.v2);
        terrain.AddTriangle(prevCorner, edge.v4, edge.v5);
        terrain.AddTriangle(underCenter, underEdge.v2, underEdge.v4);
        terrain.AddTriangleColor(color);
        terrain.AddTriangleColor(color);
        terrain.AddTriangleColor(color);

        if (!cell.IsUnderwater) {
            rivers.AddTriangle(underCenter - riverOffset, underEdge.v2 - riverOffset, underEdge.v4 - riverOffset);
            rivers.AddTriangleColor(Color.blue);
        }

        if (cell.HasRiverThroughEdge(direction.Next())) {
            terrain.AddTriangle(underEdge.v4, underPrevCorner, underCenter);
            terrain.AddTriangleColor(color);
            if (!cell.IsUnderwater) {
                rivers.AddTriangle(underEdge.v4 - riverOffset, underPrevCorner - riverOffset, underCenter - riverOffset);
                rivers.AddTriangleColor(Color.blue);
            }

            terrain.AddQuad(underPrevCorner, prevCorner, underEdge.v4, edge.v4);
            terrain.AddQuadColor(color, color);
        }
        else {
            terrain.AddTriangle(center, edge.v4, prevCorner);
            terrain.AddQuad(underCenter, center, underEdge.v4, edge.v4);
            terrain.AddTriangleColor(color);
            terrain.AddQuadColor(color, color);

        }
        if (cell.HasRiverThroughEdge(direction.Previous())) {
            terrain.AddTriangle(underEdge.v2, underCenter, underNextCorner);
            terrain.AddTriangleColor(color);

            if (!cell.IsUnderwater) {
                rivers.AddTriangle(underEdge.v2 - riverOffset, underCenter - riverOffset, underNextCorner - riverOffset);
                rivers.AddTriangleColor(Color.blue);
            }
            terrain.AddQuad(underEdge.v2, edge.v2, underNextCorner, nextCorner);
            terrain.AddQuadColor(color, color);
        }
        else {
            terrain.AddTriangle(center, nextCorner, edge.v2);
            terrain.AddQuad(underEdge.v2, edge.v2, underCenter, center);
            terrain.AddTriangleColor(color);
            terrain.AddQuadColor(color, color);

        }
    }

    void TriangulateEdgeFan(Vector3 center, EdgeVertices edge, Color color1) {
        Vector3 nextCorner, prevCorner;
        nextCorner = (center + edge.v1) / 2f;
        prevCorner = (center + edge.v5) / 2f;
        terrain.AddTriangle(nextCorner, edge.v1, edge.v2);
        terrain.AddTriangle(prevCorner, edge.v4, edge.v5);
        terrain.AddTriangle(center, edge.v2, edge.v4);
        terrain.AddTriangle(center, edge.v4, prevCorner);
        terrain.AddTriangle(center, nextCorner, edge.v2);
        terrain.AddTriangleColor(color1);
        terrain.AddTriangleColor(color1);
        terrain.AddTriangleColor(color1);
        terrain.AddTriangleColor(color1);
        terrain.AddTriangleColor(color1);

    }

    void TriangulateEdgeStrip(TriMesh mesh, EdgeVertices e1, Color c1, EdgeVertices e2, Color c2, bool isRiverInDir) {
        mesh.AddQuad(e1.v1, e1.v2, e2.v1, e2.v2);
        mesh.AddQuadColor(c1, c2);


        if (isRiverInDir) {
            Vector3 t = new Vector3(0, TriMetrics.waterElevationOffset, 0);
            rivers.AddQuad(e1.v2 + t, e1.v4 + t, e2.v2 + t, e2.v4 + t);
            rivers.AddQuadColor(Color.blue, Color.blue);

        }
        else {
            mesh.AddQuad(e1.v2, e1.v4, e2.v2, e2.v4);
            mesh.AddQuadColor(c1, c2);

        }
        mesh.AddQuad(e1.v4, e1.v5, e2.v4, e2.v5);
        mesh.AddQuadColor(c1, c2);

    }
}
