using UnityEngine;
using System.Collections;
using System.IO;

public class TriCell : MonoBehaviour {
    public TriCoordinates coordinates;
    public bool inverted = false;
    public RectTransform uiRect;
    public TriGridChunk chunk;
    public int Index { get; set; }
    public float StreamBedY {
        get {
            return
                (elevation + TriMetrics.streamBedElevationOffset) *
                TriMetrics.elevationStep;
        }
    }

    public int NumOfRiver() {
        int i = 0;
        for (int j = 0; j < 3; j++)
            i += isRiver[j] ? 1 : 0;
        return i;
    }

    public Color Color {
        get {
            return TriMetrics.colors[terrainTypeIndex];
        }
    }
    public int TerrainTypeIndex {
        get {
            return terrainTypeIndex;
        }
        set {
            if (terrainTypeIndex != value) {
                terrainTypeIndex = value;
                Refresh();
            }
        }
    }
    int terrainTypeIndex;
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
            RefreshPosition();
            Refresh();
        }
    }
    void RefreshPosition() { 
    Vector3 position = transform.localPosition;
    position.y = elevation* TriMetrics.elevationStep;
    transform.localPosition = position;

            Vector3 uiPosition = uiRect.localPosition;
    uiPosition.z = -position.y;
            uiRect.localPosition = uiPosition;
        }
    int elevation = int.MinValue;
    public bool[] isRiver;
    public bool IsRiver(TriDirection d) {
        return isRiver[(int)d];
    }
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
            waterLevel = value ;
            Refresh();
        }
    }

    int waterLevel=1;

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

    public void Save(BinaryWriter writer) {
        writer.Write(terrainTypeIndex);
        writer.Write(elevation);
        writer.Write(waterLevel);
        writer.Write(isRiver[0]);
        writer.Write(isRiver[1]);
        writer.Write(isRiver[2]);
    }

    public void Load(BinaryReader reader) {
        terrainTypeIndex = reader.ReadInt32();
        elevation = reader.ReadInt32();
        waterLevel = reader.ReadInt32();
        isRiver[0] = reader.ReadBoolean();
        isRiver[1] = reader.ReadBoolean();
        isRiver[2] = reader.ReadBoolean();
        RefreshPosition();
    }
}
