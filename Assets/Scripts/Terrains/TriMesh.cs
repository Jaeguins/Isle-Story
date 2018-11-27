using UnityEngine;
using System.Collections.Generic;
using System;
[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class TriMesh : MonoBehaviour {
    Mesh triMesh;
    public bool useCollider, useColors, useUVCoordinates;
    MeshCollider meshCollider;
    [NonSerialized] List<Vector3> vertices;
    [NonSerialized] List<Color> colors;
    [NonSerialized] List<int> triangles;
    [NonSerialized] List<Vector2> uvs;
    void Awake() {
        GetComponent<MeshFilter>().mesh = triMesh = new Mesh();
        meshCollider = gameObject.AddComponent<MeshCollider>();
        triMesh.name = "Tri Mesh";
    }

    public void Clear() {
        triMesh.Clear();
        vertices = ListPool<Vector3>.Get();
        if (useColors) {
            colors = ListPool<Color>.Get();
        }
        if (useUVCoordinates) {
            uvs = ListPool<Vector2>.Get();
        }
        triangles = ListPool<int>.Get();
    }

    public void Apply() {
        triMesh.SetVertices(vertices);
        ListPool<Vector3>.Add(vertices);
        if (useColors) {
            triMesh.SetColors(colors);
            ListPool<Color>.Add(colors);
        }
        if (useUVCoordinates) {
            triMesh.SetUVs(0, uvs);
            ListPool<Vector2>.Add(uvs);
        }
        triMesh.SetTriangles(triangles, 0);
        ListPool<int>.Add(triangles);
        triMesh.RecalculateNormals();
        meshCollider.sharedMesh = triMesh;
    }

    public void AddTriangleUV(Vector2 uv1, Vector2 uv2, Vector3 uv3) {
        uvs.Add(uv1);
        uvs.Add(uv2);
        uvs.Add(uv3);
    }
    public void AddQuadUV(Vector2 uv1, Vector2 uv2, Vector3 uv3, Vector3 uv4) {
        uvs.Add(uv1);
        uvs.Add(uv2);
        uvs.Add(uv3);
        uvs.Add(uv4);
    }
    public void AddTriangleUV(float uMin, float uMax, float vMin, float vMax) {
        uvs.Add(new Vector2(uMax, vMax));
        uvs.Add(new Vector2(uMax, vMin));
        uvs.Add(new Vector2(uMax, vMin));
    }
    public void AddTriangleUVReverse(float uMin, float uMax, float vMin, float vMax) {
        uvs.Add(new Vector2(uMin, vMin));
        uvs.Add(new Vector2(uMin, vMax));
        uvs.Add(new Vector2(uMin, vMax));
    }
    public void AddQuadUV(float uMin, float uMax, float vMin, float vMax) {
        uvs.Add(new Vector2(uMin, vMin));
        uvs.Add(new Vector2(uMax, vMin));
        uvs.Add(new Vector2(uMin, vMax));
        uvs.Add(new Vector2(uMax, vMax));
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
    public void AddTriangle(Vector3 v1, Vector3 v2, Vector3 v3) {
        int vertexIndex = vertices.Count;
        vertices.Add(TriMetrics.Perturb(v1));
        vertices.Add(TriMetrics.Perturb(v2));
        vertices.Add(TriMetrics.Perturb(v3));
        triangles.Add(vertexIndex++);
        triangles.Add(vertexIndex++);
        triangles.Add(vertexIndex);
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


}