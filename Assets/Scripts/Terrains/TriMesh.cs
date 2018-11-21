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
        for (TriDirection d = TriDirection.VERT; d <= TriDirection.LEFT; d++) {
            Triangulate(d, cell);
        }
    }
    void Triangulate(TriDirection direction, TriCell cell) {
        Vector3 center = cell.transform.localPosition, v1, v2;
        if (cell.inverted) {
            v1 = center - TriMetrics.GetFirstSolidCorner(direction, cell.inverted);
            v2 = center - TriMetrics.GetSecondSolidCorner(direction, cell.inverted);
        }
        else {
            v1 = center + TriMetrics.GetFirstSolidCorner(direction, cell.inverted);
            v2 = center + TriMetrics.GetSecondSolidCorner(direction, cell.inverted);
        }
        AddTriangle(center, v1, v2);
        AddTriangleColor(cell.color);

        Vector3 v3, v4;
        if(cell.inverted) {
            v3 = center - TriMetrics.GetFirstCorner(direction, cell.inverted);
            v4 = center - TriMetrics.GetSecondCorner(direction, cell.inverted);
        }
        else {
            v3 = center + TriMetrics.GetFirstCorner(direction, cell.inverted);
            v4 = center + TriMetrics.GetSecondCorner(direction, cell.inverted);
        }

        AddQuad(v1, v2, v3, v4);

        TriCell prevNeighbor = cell.GetNeighbor(direction.Previous(cell.inverted)) ?? cell;
        TriCell neighbor = cell.GetNeighbor(direction) ?? cell;
        TriCell nextNeighbor = cell.GetNeighbor(direction.Next(cell.inverted)) ?? cell;

        AddQuadColor(
            cell.color,
            cell.color,
            (cell.color + prevNeighbor.color + neighbor.color) / 3f,
            (cell.color + neighbor.color + nextNeighbor.color) / 3f
            );
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