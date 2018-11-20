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
            if ((cells[i].coordinates.X+cells[i].coordinates.Z) % 2 == 0)
                Triangulate(cells[i], true);
            else
                Triangulate(cells[i], false);
        }
        triMesh.vertices = vertices.ToArray();
        triMesh.triangles = triangles.ToArray();
        triMesh.RecalculateNormals();
        triMesh.colors = colors.ToArray();
        meshCollider.sharedMesh = triMesh;
    }
    void Triangulate(TriCell cell, bool inversed) {
        Vector3 center = cell.transform.localPosition;
        for(int i = 0; i < 3; i++) {
            if (!inversed)
                AddTriangle(
                    center,
                    center + TriMetrics.corners[i],
                    center + TriMetrics.corners[i+1]
                );
            else AddTriangle(
                    center,
                    center + TriMetrics.inverseCorners[i],
                    center + TriMetrics.inverseCorners[i + 1]
                );
            AddTriangleColor(cell.color);
        }
    }

    void AddTriangleColor(Color color) {
        colors.Add(color);
        colors.Add(color);
        colors.Add(color);
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