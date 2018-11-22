using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TriGrid : MonoBehaviour {
    public int cellCountX = 6;
    public int cellCountZ = 6;
    public TriCell cellPrefab;
    TriCell[] cells;
    TriGridChunk[] chunks;
    public Text cellLabelPrefab;
    public TriGridChunk chunkPrefab;

    public int chunkCountX = 4, chunkCountZ = 3;

    public Color defaultColor = Color.white;

    public Texture2D noiseSource;

    public void setLabels(bool val) {
        for(int i = 0; i < chunks.Length; i++) {
            chunks[i].setLabels(val);
        }
    }

    private void OnEnable() {
        TriMetrics.noiseSource = noiseSource;
    }

    void Awake() {

        TriMetrics.noiseSource = noiseSource;

        cellCountX = chunkCountX * TriMetrics.chunkSizeX;
        cellCountZ = chunkCountZ * TriMetrics.chunkSizeZ;
        CreateChunks();
        CreateCells();
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
        cell.Color = defaultColor;
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
    public TriCell GetCell(int x,int z) {
        TriCoordinates coordinates = new TriCoordinates(x, z);
        int index = coordinates.X + coordinates.Z * cellCountX;
        return cells[index];
    }
    public void ColorCell(Vector3 position, Color color) {
        position = transform.InverseTransformPoint(position);
        TriCoordinates coordinates = TriCoordinates.FromPosition(position);
        int index = coordinates.X + coordinates.Z * cellCountX;
        TriCell cell = cells[index];
        cell.Color = color;
    }
}
