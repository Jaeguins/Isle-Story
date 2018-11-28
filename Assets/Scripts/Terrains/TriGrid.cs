using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Collections.Generic;

public class TriGrid : MonoBehaviour {
    public int cellCountX = 20;
    public int cellCountZ = 15;
    public TriCell cellPrefab;
    TriCell[] cells;
    TriGridChunk[] chunks;
    public Text cellLabelPrefab;
    public TriGridChunk chunkPrefab;
    int chunkCountX, chunkCountZ;
    public Color[] colors;
    public bool useTerrainTypes;
    public TriMapGenerator mapGenerator;
    public Texture2D noiseSource;

    Queue<TriCell> searchFrontier;

    public int searchPhase;

    public void setLabels(bool val) {
        for(int i = 0; i < chunks.Length; i++) {
            chunks[i].setLabels(val);
        }
    }

    private void OnEnable() {
        TriMetrics.noiseSource = noiseSource;
        TriMetrics.colors = colors;
    }

    void Awake() {
        TriMetrics.colors = colors;
        TriMetrics.noiseSource = noiseSource;
        CreateMap(cellCountX,cellCountZ);
    }
    public bool CreateMap(int x,int z) {
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
        chunkCountZ = cellCountZ /  TriMetrics.chunkSizeZ;
        CreateChunks();
        CreateCells();
        return true;
    }
    public TriCell GetCell(int xOffset, int zOffset) {
        if (xOffset < 0 || xOffset >= cellCountX || zOffset < 0 || zOffset >= cellCountZ) return null;
        return cells[xOffset + zOffset * cellCountX];
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
        cell.transform.localPosition = position;

        if (x > 0)
            if (cell.inverted) cell.SetNeighbor(TriDirection.RIGHT, cells[i - 1]);
            else cell.SetNeighbor(TriDirection.LEFT, cells[i - 1]);
        if (z > 0 && !cell.inverted) cell.SetNeighbor(TriDirection.VERT, cells[i - cellCountX]);

        cell.Elevation = 0;
        Text label = Instantiate<Text>(cellLabelPrefab);
        cell.uiRect = label.rectTransform;
        label.rectTransform.anchoredPosition = new Vector2(position.x, position.z);
        label.text = x.ToString() + "\n" + z.ToString();

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
        if(!CreateMap(reader.ReadInt32(), reader.ReadInt32())) {
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
