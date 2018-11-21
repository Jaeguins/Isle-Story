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
    void Triangulate(TriDirection direction,TriCell cell) { 
        Vector3 center = cell.transform.localPosition;
        for(int i = 0; i < 3; i++) {
            if (!cell.inverted)
                AddTriangle(
                    center,
                    center + TriMetrics.GetFirstCorner(direction,cell.inverted),
                    center + TriMetrics.GetSecondCorner(direction, cell.inverted)
                );
            else AddTriangle(
                    center,
                    center - TriMetrics.GetFirstCorner(direction, cell.inverted),
                    center - TriMetrics.GetSecondCorner(direction, cell.inverted)
                );
            TriCell neighbor = cell.GetNeighbor(direction) ?? cell;
            Color edgeColor = (cell.color + neighbor.color) * 0.5f;
            AddTriangleColor(cell.color, edgeColor, edgeColor);
            //AddTriangleColor(Color.red,Color.green, Color.blue);
        }
    }

    void AddTriangleColor(Color c1, Color c2, Color c3) {
        colors.Add(c1);
        colors.Add(c2);
        colors.Add(c3);
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