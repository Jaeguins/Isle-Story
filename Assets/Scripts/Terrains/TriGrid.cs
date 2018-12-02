using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Collections.Generic;

public class TriGrid : MonoBehaviour {
    public int cellCountX = 20;
    public int cellCountZ = 15;
    public TriCell cellPrefab;
    List<Entities> units = new List<Entities>();
    TriCell[] cells;
    TriGridChunk[] chunks;
    public Text cellLabelPrefab;
    public TriGridChunk chunkPrefab;
    int chunkCountX, chunkCountZ;
    public Color[] colors;
    public bool useTerrainTypes;
    public TriMapGenerator mapGenerator;
    public Texture2D noiseSource;
    public List<Text> labels;
    public static TriGrid Instance;
    int searchFrontierPhase;
    TriCellPriorityQueue searchFrontier;

    public int searchPhase;

    public void setLabels(bool val) {
        for (int i = 0; i < chunks.Length; i++) {
            chunks[i].setLabels(val);
        }
    }

    void ClearUnits() {
        for (int i = 0; i < units.Count; i++) {
            units[i].Die();
        }
        units.Clear();
    }

    private void OnEnable() {
        TriMetrics.noiseSource = noiseSource;
        TriMetrics.colors = colors;
    }

    void Awake() {
        TriMetrics.colors = colors;
        TriMetrics.noiseSource = noiseSource;
        CreateMap(cellCountX, cellCountZ);
        Instance = this;
    }

    TriCell currentPathFrom, currentPathTo;
    bool currentPathExists;

    public void FindPath(TriCell fromCell,TriCell toCell) {
        ClearPath();
        currentPathFrom = fromCell;
        currentPathTo = toCell;
        currentPathExists = Search(fromCell, toCell);
    }

    void ClearPath() {
        if (currentPathExists) {
            TriCell current = currentPathTo;
            while (current != currentPathFrom) {
                current = current.PathFrom;
            }
            currentPathExists = false;
        }
        currentPathFrom = currentPathTo = null;
    }

    bool Search(TriCell fromCell,TriCell toCell) {
        searchFrontierPhase += 2;
        if (searchFrontier == null) {
            searchFrontier = new TriCellPriorityQueue();
        }
        else {
            searchFrontier.Clear();
        }

        for (int i = 0; i < cells.Length; i++) {
            cells[i].Distance = int.MaxValue;
        }
        fromCell.SearchPhase = searchFrontierPhase;
        fromCell.Distance = 0;
        searchFrontier.Enqueue(fromCell);
        while (searchFrontier.Count > 0) {
            TriCell current = searchFrontier.Dequeue();
            current.SearchPhase += 1;
            if (current == toCell) {
                return true;
            }
            int currentTurn = current.Distance;

            for (TriDirection d = TriDirection.VERT; d <= TriDirection.RIGHT; d++) {
                TriCell neighbor = current.GetNeighbor(d);
                if (neighbor == null ||
                    neighbor.SearchPhase > searchFrontierPhase||
                    neighbor.Distance != int.MaxValue||
                    neighbor.IsUnderwater||
                    neighbor.HasRiver||
                    Mathf.Abs(neighbor.Elevation-fromCell.Elevation)>1)
                    continue;
                int distance = current.Distance;
                if (current.IsRoad) distance+= 1;
                else distance+= 10;
                if (neighbor.SearchPhase < searchFrontierPhase) {
                    neighbor.SearchPhase = searchFrontierPhase;
                    neighbor.Distance = distance;
                    neighbor.PathFrom = current;
                    neighbor.SearchHeuristic =
                        neighbor.coordinates.DistanceTo(toCell.coordinates);
                    searchFrontier.Enqueue(neighbor);
                }
                else if (distance < neighbor.Distance) {
                    int oldPriority = neighbor.SearchPriority;
                    neighbor.Distance = distance;
                    neighbor.PathFrom = current;
                    searchFrontier.Change(neighbor, oldPriority);
                }
            }
        }
        return false;
    }

    public bool CreateMap(int x, int z) {
        ClearPath();
        if (chunks != null) {
            for (int i = 0; i < chunks.Length; i++) {
                Destroy(chunks[i].gameObject);
            }
        }
        if (
            x <= 0 || x % TriMetrics.chunkSizeX != 0 ||
            z <= 0 || z % TriMetrics.chunkSizeZ != 0
        ) {
            Debug.LogError("Unsupported map size.");
            return false;
        }
        cellCountX = x;
        cellCountZ = z;
        chunkCountX = cellCountX / TriMetrics.chunkSizeX;
        chunkCountZ = cellCountZ / TriMetrics.chunkSizeZ;
        CreateChunks();
        CreateCells();
        return true;
    }
    public TriCell GetCell(int xOffset, int zOffset) {
        if (xOffset < 0 || xOffset >= cellCountX || zOffset < 0 || zOffset >= cellCountZ) return null;
        return cells[xOffset + zOffset * cellCountX];
    }
    public TriCell GetCell(TriCoordinates coord) {
        return GetCell(coord.X, coord.Z);
    }
    public TriCell GetCell(int cellIndex) {
        return cells[cellIndex];
    }

    void CreateChunks() {
        chunks = new TriGridChunk[chunkCountX * chunkCountZ];
        for (int z = 0, i = 0; z < chunkCountZ; z++) {
            for (int x = 0; x < chunkCountX; x++) {
                TriGridChunk chunk = chunks[i++] = Instantiate(chunkPrefab);
                chunk.transform.SetParent(transform);
            }
        }
    }

    void CreateCells() {
        cells = new TriCell[cellCountZ * cellCountX];
        for (int z = 0, i = 0; z < cellCountZ; z++) {
            for (int x = 0; x < cellCountX; x++) {
                CreateCell(x, z, i++);
            }

        }
    }

    void CreateCell(int x, int z, int i) {
        Vector3 position;
        position.x = x * TriMetrics.innerRadius;
        position.y = 0f;
        position.z = z * TriMetrics.outerRadius * 1.5f - (0.5f * TriMetrics.outerRadius * ((x + z) % 2));
        TriCell cell = cells[i] = Instantiate<TriCell>(cellPrefab);
        if ((x + z) % 2 == 0) {
            cell.inverted = true;
        }
        cell.coordinates = TriCoordinates.FromOffsetCoordinates(x, z);
        cell.Index = i;
        cell.transform.localPosition = position;

        if (x > 0)
            if (cell.inverted) cell.SetNeighbor(TriDirection.RIGHT, cells[i - 1]);
            else cell.SetNeighbor(TriDirection.LEFT, cells[i - 1]);
        if (z > 0 && !cell.inverted) cell.SetNeighbor(TriDirection.VERT, cells[i - cellCountX]);

        cell.Elevation = 0;
        labels.Add(Instantiate<Text>(cellLabelPrefab, cell.transform));
        cell.uiRect = labels[i].rectTransform;
        labels[i].rectTransform.anchoredPosition = new Vector2(position.x, position.z);

        AddCellToChunk(x, z, cell);
    }
    void AddCellToChunk(int x, int z, TriCell cell) {
        int chunkX = x / TriMetrics.chunkSizeX;
        int chunkZ = z / TriMetrics.chunkSizeZ;
        TriGridChunk chunk = chunks[chunkX + chunkZ * chunkCountX];

        int localX = x - chunkX * TriMetrics.chunkSizeX;
        int localZ = z - chunkZ * TriMetrics.chunkSizeZ;
        chunk.AddCell(localX + localZ * TriMetrics.chunkSizeX, cell);
    }


    public TriCell GetCell(Vector3 position) {
        position = transform.InverseTransformPoint(position);
        TriCoordinates coordinates = TriCoordinates.FromPosition(position);
        int index = coordinates.X + coordinates.Z * cellCountX;
        return cells[index];
    }

    public void Save(BinaryWriter writer) {
        writer.Write(cellCountX);
        writer.Write(cellCountZ);
        for (int i = 0; i < cells.Length; i++) {
            cells[i].Save(writer);
        }
    }

    public void Load(BinaryReader reader) {
        ClearPath();
        if (!CreateMap(reader.ReadInt32(), reader.ReadInt32())) {
            return;
        }
        for (int i = 0; i < cells.Length; i++) {
            cells[i].Load(reader);
        }
        for (int i = 0; i < chunks.Length; i++) {
            chunks[i].Refresh();

        }
    }
}
