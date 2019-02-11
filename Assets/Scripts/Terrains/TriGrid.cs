using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Collections.Generic;

public class TriGrid : MonoBehaviour {
    public int cellCountX = 20;
    public int cellCountZ = 15;
    public TriCell cellPrefab;
    public Entity unitPrefab;
    public TriCell[] cells;
    List<TriGridChunk> chunks = new List<TriGridChunk>();
    public Text cellLabelPrefab;
    public TriGridChunk chunkPrefab;
    int chunkCountX, chunkCountZ;
    public Color[] colors;
    public bool useTerrainTypes;
    public TriMapGenerator mapGenerator;
    public Texture2D noiseSource;
    public static TriGrid Instance;
    int searchFrontierPhase;
    TriCellPriorityQueue searchFrontier;
    TriCell currentPathFrom, currentPathTo;
    bool currentPathExists;
    public int searchPhase;
    
    void ClearEntities() {
        TriIsland.Instance.entities.ClearEntities();
    }

    private void OnEnable() {
        TriMetrics.noiseSource = noiseSource;
        TriMetrics.colors = colors;
        Entity.unitPrefab = unitPrefab;
    }

    void Awake() {
        Time.timeScale = 0;
        TriMetrics.colors = colors;
        TriMetrics.noiseSource = noiseSource;
        Entity.unitPrefab = unitPrefab;
        //StartCoroutine(CreateMap(cellCountX, cellCountZ));
        Instance = this;
    }

    public bool HasPath {
        get {
            return currentPathExists;
        }
    }

    public void FindPath(TriCell fromCell, TriCell toCell, bool entityCheck = true) {
        ClearPath();
        currentPathFrom = fromCell;
        currentPathTo = toCell;
        currentPathExists = Search(fromCell, toCell, entityCheck);
        Debug.Log("pathFindResult : " + currentPathExists);
    }

    public List<TriCell> GetPath() {
        if (!currentPathExists) {
            return null;
        }
        List<TriCell> path = ListPool<TriCell>.Get();
        for (TriCell c = currentPathTo; c != currentPathFrom; c = c.PathFrom) {
            path.Add(c);
        }
        path.Add(currentPathFrom);
        path.Reverse();
        return path;
    }

    public void ClearPath() {
        if (currentPathExists) {
            TriCell current = currentPathTo;
            while (current != currentPathFrom) {
                current = current.PathFrom;
            }
            currentPathExists = false;
            searchFrontier.Clear();
        }
        currentPathFrom = currentPathTo = null;
    }

    bool Search(TriCell fromCell, TriCell toCell, bool entityCheck) {
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
                    neighbor.SearchPhase > searchFrontierPhase ||
                    neighbor.Distance != int.MaxValue ||
                    neighbor.IsUnderwater ||
                    neighbor.HasRiver ||
                    (entityCheck && !neighbor.Stepable) ||
                    Mathf.Abs(neighbor.Elevation - current.Elevation) > 1)
                    continue;
                int distance = current.Distance;
                if (current.IsRoad) distance += 1;
                else distance += 10;
                distance += current.Elevation != neighbor.Elevation ? 1 : 0;
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
    public IEnumerator<WaitForEndOfFrame> DestroyChunk(int i, bool flag = false) {
        chunks[i].Disable();
        Destroy(chunks[i].gameObject);
        if (flag)
            yield return null;
    }
    public IEnumerator<Coroutine> CreateMap(int x, int z) {
        ClearPath();
        ClearEntities();
        if (
            x <= 0 || x % TriMetrics.chunkSizeX != 0 ||
            z <= 0 || z % TriMetrics.chunkSizeZ != 0
        ) {
            Debug.LogError("Unsupported map size.");
        }
        else {
            cellCountX = x;
            cellCountZ = z;
            chunkCountX = cellCountX / TriMetrics.chunkSizeX;
            chunkCountZ = cellCountZ / TriMetrics.chunkSizeZ;
            if (chunks == null || chunks.Count != chunkCountX * chunkCountZ) {
                for (int i = 0; i < chunks.Count; i++) {
                    //yield return StartCoroutine(DestroyChunk(i, i % Strings.refreshLimit == 0));
                    DestroyChunk(i, i % Strings.refreshLimit == 0);
                }
                yield return StartCoroutine(CreateChunks());
                yield return StartCoroutine(CreateCells());
            }
            else {
                foreach (TriCell cell in Instance.cells) {
                    cell.ReInit();
                }
            }
            
        }
        yield return null;
    }

    public TriCell GetCell(int xOffset, int zOffset) {
        if (xOffset < 0 || xOffset >= cellCountX || zOffset < 0 || zOffset >= cellCountZ) return null;
        if (cells == null) return null;
        return cells[xOffset + zOffset * cellCountX];
    }

    public TriCell GetCell(TriCoordinates coord) {
        return GetCell(coord.X, coord.Z);
    }

    public TriCell GetCell(int cellIndex) {
        return cells[cellIndex];
    }

    IEnumerator<WaitForEndOfFrame> CreateChunks() {
        chunks.Clear();
        for (int z = 0, i = 0; z < chunkCountZ; z++) {
            for (int x = 0; x < chunkCountX; x++) {
                TriGridChunk chunk = Instantiate(chunkPrefab);
                chunks.Add(chunk);
                chunk.transform.SetParent(transform);
            }
        }
        yield return null;
    }

    IEnumerator<WaitForEndOfFrame> CreateCells() {
        cells = new TriCell[cellCountZ * cellCountX];
        for (int z = 0, i = 0; z < cellCountZ; z++) {
            for (int x = 0; x < cellCountX; x++) {
                CreateCell(x, z, i++);
            }
        }
        yield return null;
    }

    void CreateCell(int x, int z, int i) {
        Vector3 position;
        position.x = x * TriMetrics.innerRadius;
        position.y = 0f;
        position.z = z * TriMetrics.outerRadius * 1.5f - (0.5f * TriMetrics.outerRadius * ((x + z) % 2));
        TriCell cell = cells[i] = new TriCell();
        if ((x + z) % 2 == 0) {
            cell.inverted = true;
        }
        cell.coordinates = TriCoordinates.FromOffsetCoordinates(x, z);
        cell.Index = i;
        cell.position = position;

        if (x > 0)
            if (cell.inverted) cell.SetNeighbor(TriDirection.RIGHT, cells[i - 1]);
            else cell.SetNeighbor(TriDirection.LEFT, cells[i - 1]);
        if (z > 0 && !cell.inverted) cell.SetNeighbor(TriDirection.VERT, cells[i - cellCountX]);

        cell.Elevation = 0;
        AddCellToChunk(x, z, cell);
    }

    void AddCellToChunk(int x, int z, TriCell cell) {
        int chunkX = x / TriMetrics.chunkSizeX;
        int chunkZ = z / TriMetrics.chunkSizeZ;
        TriGridChunk chunk = chunks[chunkX + chunkZ * chunkCountX];

        int localX = x - chunkX * TriMetrics.chunkSizeX;
        int localZ = z - chunkZ * TriMetrics.chunkSizeZ;
        chunk.AddCell(localX + localZ * TriMetrics.chunkSizeX, cell);
        chunk.Refresh();
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

    public TriCell GetCell(Ray ray) {
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit)) {
            return GetCell(hit.point);
        }
        return null;
    }

    public IEnumerator<Coroutine> Load(BinaryReader reader, int header) {
        TriIsland.Loaded = false;
        ClearPath();
        ClearEntities();
        yield return StartCoroutine(CreateMap(reader.ReadInt32(), reader.ReadInt32()));
        for (int i = 0; i < cells.Length; i++) {
            cells[i].Load(reader);
        }
        for (int i = 0; i < chunks.Count; i++) {
            chunks[i].Refresh();
            if (i % Strings.refreshLimit == 0) yield return null;
        }
    }
}
