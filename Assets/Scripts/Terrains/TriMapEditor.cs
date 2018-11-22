using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class TriMapEditor : MonoBehaviour {
    public Color[] colors;
    public TriGrid triGrid;
    bool applyColor;
    private Color activeColor;
    bool applyElevation = true;
    bool isDrag;
    TriDirection dragDirection;
    TriCell previousCell;

    enum OptionalToggle {
        Ignore, Yes, No
    }
    int activeElevation;

    public void SetElevation(float elevation) {
        activeElevation = (int)elevation;
    }

    private void Awake() {
        //InitColor();
    }
    void Update() {
        if (Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject()) {
            HandleInput(0);
        }
        else if (Input.GetMouseButton(1) && !EventSystem.current.IsPointerOverGameObject()) {
            HandleInput(1);
        }
        else {
            previousCell = null;
        }
    }
    void HandleInput(int type) {
        Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(inputRay, out hit)) {
            if (type == 0) {
                TriCell currentCell = triGrid.GetCell(hit.point);
                if (previousCell && previousCell != currentCell) {
                    ValidateDrag(currentCell);
                }
                else {
                    isDrag = false;
                }
                EditCell(currentCell);
                previousCell = currentCell;
            }
            else {
                EditHex(triGrid.GetCell(hit.point));
            }

        }
        else {
            previousCell = null;
        }
    }

    void ValidateDrag(TriCell currentCell) {
        for (
            dragDirection = TriDirection.VERT;
            dragDirection <= TriDirection.RIGHT;
            dragDirection++
        ) {
            if (previousCell.GetNeighbor(dragDirection) == currentCell) {
                isDrag = true;
                return;
            }
        }
        isDrag = false;
    }

    OptionalToggle riverMode;
    public void SetRiverMode(int mode) {
        riverMode = (OptionalToggle)mode;
    }

    public void SetApplyElevation(bool toggle) {
        applyElevation = toggle; 
    }

    public void SelectColor(int index) {
        applyColor = index >= 0;
        if (applyColor) {
            activeColor = colors[index];
        }
    }
    void EditCell(TriCell cell) {
        if (applyColor) {
            cell.Color = activeColor;
        }
        if (applyElevation) {
            cell.Elevation = activeElevation;
        }
        if (riverMode == OptionalToggle.No) {
            cell.RemoveRiver(dragDirection);
        }
        else if (isDrag && riverMode == OptionalToggle.Yes) {
            TriCell otherCell = cell.GetNeighbor(dragDirection);
            if (otherCell) {
                otherCell.SetRiver(dragDirection);
            }
        }
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

    
}
