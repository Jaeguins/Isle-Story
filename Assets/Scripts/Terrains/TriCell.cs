using UnityEngine;
using System.Collections;

public class TriCell : MonoBehaviour {
    public TriCoordinates coordinates;
    public bool inverted=false;
    public RectTransform uiRect;
    public TriGridChunk chunk;
    public Color Color {
        get {
            return color;
        }
        set {
            if (color == value) {
                return;
            }
            color = value;
            Refresh();
        }
    }
    Color color;
    public Vector3 Position {
        get {
            return transform.localPosition;
        }
    }
    public int Elevation {
        get {
            return elevation;
        }
        set {
            if (elevation == value) return;
            elevation = value;
            Vector3 position = transform.localPosition;
            position.y = value * TriMetrics.elevationStep;
            transform.localPosition = position;

            Vector3 uiPosition = uiRect.localPosition;
            uiPosition.z = -position.y;
            uiRect.localPosition = uiPosition;
            Refresh();
        }
    }
    int elevation=int.MinValue;
    [SerializeField]
    TriCell[] neighbors;
    public TriCell GetNeighbor(TriDirection direction) {
        return neighbors[(int)direction];
    }
    public void SetNeighbor(TriDirection direction, TriCell cell) {
        neighbors[(int)direction] = cell;
        cell.neighbors[(int)direction] = this;
    }
    void Refresh() {
        if (chunk) {
            chunk.Refresh();
            for (int i = 0; i < neighbors.Length; i++) {
                TriCell neighbor = neighbors[i];
                if (neighbor != null && neighbor.chunk != chunk) {
                    neighbor.chunk.Refresh();
                }
            }
        }
        
    }
}
