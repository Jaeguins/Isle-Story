using UnityEngine;
using System.Collections;
using System.IO;

public class TriCell {
    public TriCoordinates coordinates;
    public Vector3 position;
    public Color GetColor() {
        if (IsUnderwater) return TriMetrics.FromARGB(255, 0, 0, 255);
        if (Statics) {
            if (Statics is Natural) return Color.green;
            else if (Statics is Building) return TriMetrics.FromARGB(255, 139, 69, 19);
            else return TriMetrics.FromARGB(255, 255, 248, 220);
        }
        else return Color;
    }
    public static implicit operator bool(TriCell cell) {
        return cell != null;
    }
    public static implicit operator Vector3(TriCell obj) {
        return obj.Position;
    }
    public bool IsRoad {
        get {
            return isRoad;
        }
        set {
            isRoad = value;
        }
    }
    public bool Stepable {
        get {
            return Statics ? Statics.Stepable : true;
        }
    }
    bool isRoad = false;
    public TriCell PathFrom { get; set; }
    public bool inverted = false;
    public TriGridChunk chunk;
    public int Index { get; set; }
    public TriCell NextWithSamePriority { get; set; }
    public Statics Statics { get; set; }
    public int SearchHeuristic { get; set; }
    public int SearchPhase { get; set; }
    int distance;
    public void ReInit() {
        elevation = 0;
        RefreshPosition();
        for (int i = 0; i < isRiver.Length; i++) isRiver[i] = false;
        terrainTypeIndex = 0;
        isRoad = false;
    }
    public int Distance {
        get {
            return distance;
        }
        set {
            distance = value;
        }
    }

    public bool IsBuildable() {
        return (!Statics && !IsUnderwater && !HasRiver);
    }

    public int SearchPriority {
        get {
            return distance + SearchHeuristic * 10;
        }
    }

    public Color Color {
        get {
            return TriGrid.Instance.colors[terrainTypeIndex];
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
            return position;
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
        position.y = elevation * TriMetrics.elevationStep;
    }

    int elevation = int.MinValue;

    bool[] isRiver = { false, false, false };

    public bool HasRiver {
        get {
            return isRiver[0] || isRiver[1] || isRiver[2];
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
    TriCell[] neighbors=new TriCell[3];
    public TriCell GetNeighbor(TriDirection direction) {
        return neighbors[(int)direction];
    }

    public void SetNeighbor(TriDirection direction, TriCell cell) {
        neighbors[(int)direction] = cell;
        cell.neighbors[(int)direction] = this;
    }

    void RefreshSelfOnly() {
        chunk.Refresh();
        if (Statics) {
            Statics.ValidateLocation();
        }
    }

    void Refresh() {
        if (chunk) {
            chunk.Refresh();
            if (Statics) {
                Statics.ValidateLocation();
            }
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
    public override string ToString() {
        return "Cell "+coordinates;
    }
}
