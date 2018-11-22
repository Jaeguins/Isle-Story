using UnityEngine;
using System.Collections.Generic;
[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class TriMesh : MonoBehaviour {
    Mesh triMesh;
    MeshCollider meshCollider;
    static List<Vector3> vertices = new List<Vector3>();
    static List<Color> colors = new List<Color>();
    static List<int> triangles = new List<int>();

    void Awake() {
        GetComponent<MeshFilter>().mesh = triMesh = new Mesh();
        meshCollider = gameObject.AddComponent<MeshCollider>();
        triMesh.name = "Tri Mesh";
    }
    public void Triangulate(TriCell[] cells) {
        triMesh.Clear();
        vertices.Clear();
        triangles.Clear();
        colors.Clear();
        for (int i = 0; i < cells.Length; i++) {
            Triangulate(cells[i]);
        }
        triMesh.vertices = vertices.ToArray();
        triMesh.triangles = triangles.ToArray();
        triMesh.colors = colors.ToArray();
        triMesh.RecalculateNormals();
        meshCollider.sharedMesh = triMesh;
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
        TriangulateEdgeFan(center, e, cell.Color);
        if (cell.inverted) {
            if (direction != TriDirection.LEFT)
                TriangulateConnection(direction, cell, e, inverter);
        }
        else {
            if (direction == TriDirection.LEFT)
                TriangulateConnection(direction, cell, e, inverter);
        }

    }
    void TriangulateConnection(TriDirection direction, TriCell cell, EdgeVertices e1, int inverter) {
        Vector3 center = cell.transform.localPosition;
        TriCell neighbor = cell.GetNeighbor(direction);
        if (neighbor == null) return;
        Vector3 bridge = TriMetrics.GetBridge(direction);
        bridge.y = neighbor.Position.y - cell.Position.y;
        EdgeVertices e2 = new EdgeVertices(
            e1.v1+bridge,
            e1.v4+bridge
            );
        TriangulateEdgeStrip(e1, cell.Color, e2, neighbor.Color);

    }
    void AddTriangleColor(Color c1) {
        colors.Add(c1);
        colors.Add(c1);
        colors.Add(c1);
    }
    void AddTriangleColor(Color c1, Color c2, Color c3) {
        colors.Add(c1);
        colors.Add(c2);
        colors.Add(c3);
    }

    void AddQuad(Vector3 v1, Vector3 v2, Vector3 v3, Vector3 v4) {
        int vertexIndex = vertices.Count;
        vertices.Add(Perturb(v1));
        vertices.Add(Perturb(v2));
        vertices.Add(Perturb(v3));
        vertices.Add(Perturb(v4));
        triangles.Add(vertexIndex);
        triangles.Add(vertexIndex + 2);
        triangles.Add(vertexIndex + 1);
        triangles.Add(vertexIndex + 1);
        triangles.Add(vertexIndex + 2);
        triangles.Add(vertexIndex + 3);
    }
    void AddQuadColor(Color c1, Color c2) {
        colors.Add(c1);
        colors.Add(c1);
        colors.Add(c2);
        colors.Add(c2);
    }
    void AddQuadColor(Color c1, Color c2, Color c3, Color c4) {
        colors.Add(c1);
        colors.Add(c2);
        colors.Add(c3);
        colors.Add(c4);
    }

    void AddTriangle(Vector3 v1, Vector3 v2, Vector3 v3) {
        int vertexIndex = vertices.Count;
        vertices.Add(Perturb(v1));
        vertices.Add(Perturb(v2));
        vertices.Add(Perturb(v3));
        triangles.Add(vertexIndex++);
        triangles.Add(vertexIndex++);
        triangles.Add(vertexIndex);
    }

    Vector3 Perturb(Vector3 position) {
        Vector4 sample = TriMetrics.SampleNoise(position);
        position.x += (sample.x * 2f - 1f) * TriMetrics.cellPerturbStrength;
        //position.y += (sample.y * 2f - 1f) * TriMetrics.cellPerturbStrength;
        position.z += (sample.z * 2f - 1f) * TriMetrics.cellPerturbStrength;
        return position;
    }

    void TriangulateEdgeFan(Vector3 center, EdgeVertices edge, Color color) {
        AddTriangle(center, edge.v1, edge.v2);
        AddTriangleColor(color);
        AddTriangle(center, edge.v2, edge.v3);
        AddTriangleColor(color);
        AddTriangle(center, edge.v3, edge.v4);
        AddTriangleColor(color);
    }
    void TriangulateEdgeStrip(
        EdgeVertices e1, Color c1,
        EdgeVertices e2, Color c2
    ) {
        AddQuad(e1.v1, e1.v2, e2.v1, e2.v2);
        AddQuadColor(c1, c2);
        AddQuad(e1.v2, e1.v3, e2.v2, e2.v3);
        AddQuadColor(c1, c2);
        AddQuad(e1.v3, e1.v4, e2.v3, e2.v4);
        AddQuadColor(c1, c2);
    }
}