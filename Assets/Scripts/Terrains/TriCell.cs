using UnityEngine;
using System.Collections;

public class TriCell : MonoBehaviour {
    public TriCoordinates coordinates;
    public bool inverted = false;
    public RectTransform uiRect;
    public TriGridChunk chunk;
    public float StreamBedY {
        get {
            return
                (elevation + TriMetrics.streamBedElevationOffset) *
                TriMetrics.elevationStep;
        }
    }
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
    int elevation = int.MinValue;
    public bool[] isRiver;
    public bool HasRiver {
        get {
            return isRiver[0]||isRiver[1]||isRiver[2];
        }
    }

    public float RiverSurfaceY {
        get {
            return
                (elevation + TriMetrics.streamBedElevationOffset) *
                TriMetrics.elevationStep;
        }
    }

    public int WaterLevel {
        get {
            return waterLevel;
        }
        set {
            if (waterLevel == value) {
                return;
            }
            waterLevel = value;
            Refresh();
        }
    }

    int waterLevel = 1;

    public bool IsUnderwater {
        get {
            return waterLevel > elevation;
        }
    }

    public bool HasRiverBeginOrEnd {
        get {
            int count = 0;
            for(int i = 0; i < isRiver.Length; i++) {
                if (isRiver[i]) count += 1;
            }
            return count==1;
        }
    }

    public bool HasRiverThroughEdge(TriDirection direction) {
        return isRiver[(int)direction];
    }

    public void RemoveRiver(TriDirection direction) {
        if (isRiver[(int)direction]) {
            isRiver[(int)direction] = false;
            RefreshSelfOnly();

            TriCell neighbor = GetNeighbor(direction);
            neighbor.isRiver[(int)direction] = false;
            neighbor.RefreshSelfOnly();
        }
    }

    public void SetRiver(TriDirection direction) {
        if (!isRiver[(int)direction]) {
            isRiver[(int)direction] = true;
            RefreshSelfOnly();

            TriCell neighbor = GetNeighbor(direction);
            neighbor.isRiver[(int)direction] = true;
            neighbor.RefreshSelfOnly();
        }
    }

   

    [SerializeField]
    TriCell[] neighbors;
    public TriCell GetNeighbor(TriDirection direction) {
        return neighbors[(int)direction];
    }
    public void SetNeighbor(TriDirection direction, TriCell cell) {
        neighbors[(int)direction] = cell;
        cell.neighbors[(int)direction] = this;
    }
    void RefreshSelfOnly() {
        chunk.Refresh();
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
