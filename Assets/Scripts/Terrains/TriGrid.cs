using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TriGrid : MonoBehaviour {
    public int width = 6;
    public int height = 6;
    public TriCell cellPrefab;
    TriMesh triMesh;
    TriCell[] cells;
    public Text cellLabelPrefab;
    Canvas gridCanvas;

    public Color defaultColor = Color.white;
    public Color touchedColor = Color.magenta;

    void Awake() {
        gridCanvas = GetComponentInChildren<Canvas>();
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
        position.z = z * TriMetrics.outerRadius * 1.5f - (0.5f * TriMetrics.outerRadius * ((x + z) % 2));
        TriCell cell = cells[i] = Instantiate<TriCell>(cellPrefab);
        if ((x + z) % 2 == 0) {
            cell.inverted = true;
        }
        cell.coordinates = TriCoordinates.FromOffsetCoordinates(x, z);
        cell.transform.SetParent(transform, false);
        cell.transform.localPosition = position;

        cell.color = defaultColor;

        if (x > 0) {
            if (cell.inverted) {
                cell.SetNeighbor(TriDirection.RIGHT, cells[i - 1]);
            }
            else {
                cell.SetNeighbor(TriDirection.LEFT, cells[i - 1]);
            }
            
        }
        if (z > 0&&!cell.inverted) {
            cell.SetNeighbor(TriDirection.VERT, cells[i - width]);
        }

        Text label = Instantiate<Text>(cellLabelPrefab);
        label.rectTransform.SetParent(gridCanvas.transform, false);
        label.rectTransform.anchoredPosition = new Vector2(position.x, position.z);
        label.text = x.ToString() + "\n" + z.ToString();


    }
    void Update() {
        if (Input.GetMouseButton(0)) {
            HandleInput();
        }
    }

    void HandleInput() {
        Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(inputRay, out hit)) {
            TouchCell(hit.point);
        }
    }

    void TouchCell(Vector3 position) {
        position = transform.InverseTransformPoint(position);
        TriCoordinates coordinates = TriCoordinates.FromPosition(position);
        int index = coordinates.X + coordinates.Z*width;
        TriCell cell = cells[index];
        cell.color = touchedColor;
        triMesh.Triangulate(cells);
    }
}
