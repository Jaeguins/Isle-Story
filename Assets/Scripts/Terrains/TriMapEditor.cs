using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class TriMapEditor : MonoBehaviour {
    public Color[] colors;
    public TriGrid triGrid;
    private Color activeColor;

    int activeElevation;

    public void SetElevation(float elevation) {
        activeElevation = (int)elevation;
    }

    private void Awake() {
        SelectColor(0);
    }    // Update is called once per frame
    void Update() {
        if (Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject()) {
            HandleInput(0);
        }
        else if (Input.GetMouseButton(1) && !EventSystem.current.IsPointerOverGameObject()) {
            HandleInput(1);
        }
    }
    void HandleInput(int type) {
        Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(inputRay, out hit)) {
            if (type == 0) {
                EditCell(triGrid.GetCell(hit.point));
            }
            else {
                EditHex(triGrid.GetCell(hit.point));
            }

        }
    }
    public void SelectColor(int index) {
        activeColor = colors[index];
    }
    void EditCell(TriCell cell) {
        cell.Color = activeColor;
        cell.Elevation = activeElevation;
    }
    void EditHex(TriCell cell) {
        TriCell k = cell;
        bool inverted = cell.inverted;
        TriDirection d = TriDirection.VERT;
        for (int i = 0; i < 6; i++) {
            if (!k) break;
            EditCell(k);
            k = k.GetNeighbor(d);
            if (inverted)
                d = d.Next();
            else
                d = d.Previous();
        }
    }

    void InitColor() {
        for (int i = 2; i < triGrid.cellCountX; i += 3) {
            for (int j = 1; j < triGrid.cellCountZ; j += 2) {
                activeColor = Random.ColorHSV();
                activeElevation = (int)(Random.value * 6f);
                EditHex(triGrid.GetCell(i, j));
            }
        }
    }
    private void Start() {
        InitColor();
    }
}
