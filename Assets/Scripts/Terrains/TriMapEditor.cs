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
        if (Input.GetMouseButton(0)&& !EventSystem.current.IsPointerOverGameObject()) {
            HandleInput();
        }
    }
    void HandleInput() {
        Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(inputRay, out hit)) {
            EditCell(triGrid.GetCell(hit.point));
        }
    }
    public void SelectColor(int index) {
        activeColor = colors[index];
    }
    void EditCell(TriCell cell) {
        cell.color = activeColor;
        cell.Elevation = activeElevation;
        triGrid.Refresh();
    }
}
