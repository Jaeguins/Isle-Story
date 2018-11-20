using UnityEngine;
using UnityEditor;
using System.Collections;

public class TriGrid : MonoBehaviour {
    public int width = 6;
    public int height = 6;
    public TriCell cellPrefab;
    TriMesh triMesh;
    TriCell[] cells;
    void Awake() {
        cells = new TriCell[height * width];
        triMesh = GetComponentInChildren<TriMesh>();
        for (int z = 0, i = 0; z < height; z++) {
            for (int x = 0; x < width; x++) {
                CreateCell(x, z, i++);
            }
        }
    }
    void Start() {
        triMesh.Triangulate(cells);
    }
    void CreateCell(int x, int z, int i) {
        Vector3 position;
        position.x = x * TriMetrics.innerRadius;
        position.y = 0f;
        position.z = z* TriMetrics.outerRadius*1.5f-(0.5f*TriMetrics.outerRadius*((x+z)%2));
        TriCell cell = cells[i] = Instantiate<TriCell>(cellPrefab);
        cell.x = x;
        cell.z = z;
        cell.transform.SetParent(transform, false);
        cell.transform.localPosition = position;
        
    }
}
