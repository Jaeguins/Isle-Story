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

    public void Clear() {
        triMesh.Clear();
        vertices.Clear();
        colors.Clear();
        triangles.Clear();
    }

    public void Apply() {
        triMesh.SetVertices(vertices);
        triMesh.SetColors(colors);
        triMesh.SetTriangles(triangles, 0);
        triMesh.RecalculateNormals();
        meshCollider.sharedMesh = triMesh;
    }

    public void AddTriangleColor(Color c1) {
        colors.Add(c1);
        colors.Add(c1);
        colors.Add(c1);
    }
    public void AddTriangleColor(Color c1, Color c2, Color c3) {
        colors.Add(c1);
        colors.Add(c2);
        colors.Add(c3);
    }
    public void AddQuad(Vector3 v1, Vector3 v2, Vector3 v3, Vector3 v4) {
        int vertexIndex = vertices.Count;
        vertices.Add(TriMetrics.Perturb(v1));
        vertices.Add(TriMetrics.Perturb(v2));
        vertices.Add(TriMetrics.Perturb(v3));
        vertices.Add(TriMetrics.Perturb(v4));
        triangles.Add(vertexIndex);
        triangles.Add(vertexIndex + 2);
        triangles.Add(vertexIndex + 1);
        triangles.Add(vertexIndex + 1);
        triangles.Add(vertexIndex + 2);
        triangles.Add(vertexIndex + 3);
    }
    public void AddQuadColor(Color c1, Color c2) {
        colors.Add(c1);
        colors.Add(c1);
        colors.Add(c2);
        colors.Add(c2);
    }
    public void AddQuadColor(Color c1, Color c2, Color c3, Color c4) {
        colors.Add(c1);
        colors.Add(c2);
        colors.Add(c3);
        colors.Add(c4);
    }
    public void AddTriangle(Vector3 v1, Vector3 v2, Vector3 v3) {
        int vertexIndex = vertices.Count;
        vertices.Add(TriMetrics.Perturb(v1));
        vertices.Add(TriMetrics.Perturb(v2));
        vertices.Add(TriMetrics.Perturb(v3));
        triangles.Add(vertexIndex++);
        triangles.Add(vertexIndex++);
        triangles.Add(vertexIndex);
    }

   
   

}