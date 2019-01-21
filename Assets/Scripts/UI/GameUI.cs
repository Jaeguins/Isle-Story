using UnityEngine;
using UnityEngine.EventSystems;

public class GameUI : MonoBehaviour {
    public static GameUI Instance;
    TriCell currentCell;
    Unit selectedUnit;
    public TriMapEditor mapEditor;
    public GameObject buildingPanel;
    public TriGrid grid;
    public BuildingMenu buildMenu;
    private void Start() {
        Instance = this;
    }

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
}
