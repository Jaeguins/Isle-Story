using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TriGridChunk : MonoBehaviour {
    TriCell[] cells;

    TriMesh triMesh;
    Canvas gridCanvas;

    void Awake() {
        gridCanvas = GetComponentInChildren<Canvas>();
        triMesh = GetComponentInChildren<TriMesh>();

        cells = new TriCell[TriMetrics.chunkSizeX * TriMetrics.chunkSizeZ];
    }

    void Start() {
        triMesh.Triangulate(cells);
    }

    public void AddCell(int index, TriCell cell) {
        cells[index] = cell;
        cell.chunk = this;
        cell.transform.SetParent(transform, false);
        cell.uiRect.SetParent(gridCanvas.transform, false);
    }
    public void Refresh() {
        enabled = true;
    }
    private void LateUpdate() {
        triMesh.Triangulate(cells);
        enabled = false;
    }
}
