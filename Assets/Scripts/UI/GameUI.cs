using UnityEngine;
using UnityEngine.EventSystems;

public class GameUI : MonoBehaviour {
    TriCell currentCell;
    Unit selectedUnit;

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
            selectedUnit = (Unit)currentCell.Entity;
        }
    }
    void Update() {
        if (!EventSystem.current.IsPointerOverGameObject()) {
            if (Input.GetMouseButtonDown(0)) {
                DoSelection();
            }
            else if (selectedUnit) {
                if (Input.GetMouseButtonDown(1)) {
                    UpdateCurrentCell();
                    if (Input.GetKey(KeyCode.Z)) {
                        selectedUnit.CancelAllAct();
                    }
                    selectedUnit.AddCommand(new Command(CommandType.MOVE, currentCell));
                }
            }
        }
    }

    
}
