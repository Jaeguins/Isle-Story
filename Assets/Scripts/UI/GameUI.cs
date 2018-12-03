using UnityEngine;
using UnityEngine.EventSystems;

public class GameUI : MonoBehaviour {
    TriCell currentCell;
    Entities selectedUnit;
    public TriGrid grid;
    public void SetEditMode(bool toggle) {
        enabled = !toggle;
        grid.ClearPath();
    }
    bool UpdateCurrentCell() {
        TriCell cell =
            grid.GetCell(Camera.main.ScreenPointToRay(Input.mousePosition));
        if (cell != currentCell) {
            currentCell = cell;
            return true;
        }
        return false;
    }
    void DoSelection() {
        grid.ClearPath();
        UpdateCurrentCell();
        if (currentCell) {
            selectedUnit = currentCell.Entity;
        }
    }
    void Update() {
        if (!EventSystem.current.IsPointerOverGameObject()) {
            if (Input.GetMouseButtonDown(0)) {
                DoSelection();
            }
            else if (selectedUnit) {
                if (Input.GetMouseButtonDown(1)) {
                    DoMove();
                }
                else {
                    DoPathfinding();
                }
            }
        }
    }
    
    void DoPathfinding() {
        if (UpdateCurrentCell()) {
            if (currentCell && selectedUnit.IsValidDestination(currentCell)) {
                grid.FindPath(selectedUnit.Location, currentCell);
            }
            else {
                grid.ClearPath();
            }
        }
    }
    public void DoMove() {
        if (grid.HasPath) {
            selectedUnit.Travel(grid.GetPath());
            grid.ClearPath();
        }
    }
}
