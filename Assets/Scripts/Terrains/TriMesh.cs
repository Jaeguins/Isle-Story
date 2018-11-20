using UnityEngine;
using System.Collections.Generic;
[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class TriMesh : MonoBehaviour {
    Mesh triMesh;
    List<Vector3> vertices;
    List<int> triangles;

    void Awake() {
        GetComponent<MeshFilter>().mesh = triMesh = new Mesh();
        triMesh.name = "Tri Mesh";
        vertices = new List<Vector3>();
        triangles = new List<int>();
    }
    public void Triangulate(TriCell[] cells) {
        triMesh.Clear();
        vertices.Clear();
        triangles.Clear();
        for (int i = 0; i < cells.Length; i++) {
            if ((cells[i].x+cells[i].z) % 2 == 0)
                Triangulate(cells[i], true);
            else
                Triangulate(cells[i], false);
        }
        triMesh.vertices = vertices.ToArray();
        triMesh.triangles = triangles.ToArray();
        triMesh.RecalculateNormals();
    }
    void Triangulate(TriCell cell, bool inversed) {
        Vector3 center = cell.transform.localPosition;
            if (!inversed)
                AddTriangle(
                    center+TriMetrics.corners[0],
                    center + TriMetrics.corners[1],
                    center + TriMetrics.corners[2]
                );
            else AddTriangle(
                center + TriMetrics.inverseCorners[2],
                    center + TriMetrics.inverseCorners[1],
                    center + TriMetrics.inverseCorners[0]
                );
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