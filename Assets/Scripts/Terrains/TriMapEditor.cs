using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class TriMapEditor : MonoBehaviour {
    public Color[] colors;
    public TriGrid triGrid;
    private Color activeColor;
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
            triGrid.ColorCell(hit.point,activeColor);
        }
    }
    public void SelectColor(int index) {
        activeColor = colors[index];
    }
}
