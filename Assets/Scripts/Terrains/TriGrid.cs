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
        
        cell.coordinates = TriCoordinates.FromOffsetCoordinates(x, z);
        cell.transform.SetParent(transform, false);
        cell.transform.localPosition = position;

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
        Debug.Log("touched at " + coordinates.ToString());
    }
}
