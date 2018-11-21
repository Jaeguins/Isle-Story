using UnityEngine;
using System.Collections.Generic;
[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class TriMesh : MonoBehaviour {
    Mesh triMesh;
    MeshCollider meshCollider;
    List<Vector3> vertices;
    List<int> triangles;
    List<Color> colors;

    void Awake() {
        GetComponent<MeshFilter>().mesh = triMesh = new Mesh();
        meshCollider = gameObject.AddComponent<MeshCollider>();
        triMesh.name = "Tri Mesh";
        vertices = new List<Vector3>();
        triangles = new List<int>();
        colors = new List<Color>();
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

        Vector3 center = cell.transform.localPosition, v1, v2;
        v1 = center + inverter * TriMetrics.GetFirstSolidCorner(direction);
        v2 = center + inverter * TriMetrics.GetSecondSolidCorner(direction);

        AddTriangle(center, v1, v2);
        AddTriangleColor(cell.color);
        if (cell.inverted) {
            if (direction != TriDirection.LEFT)
                TriangulateConnection(direction, cell, v1, v2, inverter);
        }
        else {
            if (direction == TriDirection.LEFT)
                TriangulateConnection(direction, cell, v1, v2, inverter);
        }

    }
    void TriangulateConnection(TriDirection direction, TriCell cell, Vector3 v1, Vector3 v2, int inverter) {
        Vector3 center = cell.transform.localPosition;
        TriCell neighbor = cell.GetNeighbor(direction);
        if (neighbor == null) return;
        Vector3 bridge = TriMetrics.GetBridge(direction);
        Vector3 v3 = v1 + inverter * bridge, v4 = v2 + inverter * bridge;

        AddQuad(v1, v2, v3, v4);
        AddQuadColor(cell.color, neighbor.color);

        TriCell nextNeighbor = cell.GetNeighbor(direction.Next())??cell;
        TriCell prevNeighbor = cell.GetNeighbor(direction.Previous())??cell;
        AddTriangle(v2, v4, center + inverter * TriMetrics.GetSecondCorner(direction));
        AddTriangleColor(cell.color, neighbor.color, neighbor.color);
        AddTriangle(v3, v1, center + inverter * TriMetrics.GetFirstCorner(direction));
        AddTriangleColor(neighbor.color, cell.color, neighbor.color);
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
        vertices.Add(v1);
        vertices.Add(v2);
        vertices.Add(v3);
        vertices.Add(v4);
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
        vertices.Add(v1);
        vertices.Add(v2);
        vertices.Add(v3);
        triangles.Add(vertexIndex++);
        triangles.Add(vertexIndex++);
        triangles.Add(vertexIndex);
    }
}