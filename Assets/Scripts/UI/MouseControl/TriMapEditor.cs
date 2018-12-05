using UnityEngine;
using UnityEngine.EventSystems;
using System.IO;

public class TriMapEditor : MonoBehaviour {
    public TriGrid triGrid;
    public Isleland isleland;
    public int x, z;
    public TriMapGenerator mapGenerator;
    bool applyElevation = false;
    bool isDrag;
    TriDirection dragDirection;
    TriCell previousCell;
    int activeTerrainTypeIndex = -1;
    enum OptionalToggle {
        Ignore, Yes, No
    }
    int activeElevation;

    public void SetElevation(float elevation) {
        activeElevation = (int)elevation;
    }

    private void Awake() {
        SetEditMode(false);
    }

    void Update() {
        if (!EventSystem.current.IsPointerOverGameObject()) {
            if (Input.GetMouseButton(0)) {
                HandleInput();
                return;
            }
            if (Input.GetKeyDown(KeyCode.U)) {
                if (Input.GetKey(KeyCode.LeftShift)) {
                    DestroyUnit();
                }
                else {
                    CreateUnit();
                }
            }
        }
        previousCell = null;
    }

    TriCell GetCellUnderCursor() {
        return
            triGrid.GetCell(Camera.main.ScreenPointToRay(Input.mousePosition));
    }

    void HandleInput() {
        TriCell currentCell = GetCellUnderCursor();
        if (currentCell) {
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
            previousCell = null;
        }
    }

    public void SetEditMode(bool toggle) {
        enabled = toggle;
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

    OptionalToggle riverMode = OptionalToggle.Yes;

    public void SetRiverMode(int mode) {
        riverMode = (OptionalToggle)mode;
    }

    public void SetTerrainTypeIndex(int index) {
        activeTerrainTypeIndex = index;
    }

    public void SetApplyElevation(bool toggle) {
        applyElevation = toggle;
    }

    void EditCell(TriCell cell) {
        if (activeTerrainTypeIndex >= 0) {
            cell.TerrainTypeIndex = activeTerrainTypeIndex;
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

    public void Save() {
        isleland.Save();
    }

    public void Load() {
        isleland.Load();
    }

    public void NewMap() {
        triGrid.CreateMap(x, z);
        mapGenerator.GenerateMap(x, z);
        TriMapCamera.ValidatePosition();
    }

    void CreateUnit() {
        TriCell cell = GetCellUnderCursor();
        if (cell && !cell.Entity) {
            Unit ret=Instantiate(isleland.unitPrefabs[0]);
            ret.location = cell;
            ret.Orientation = Random.Range(0f, 360f);
            isleland.AddUnit(ret);
        }
    }

    void DestroyUnit() {
        TriCell cell = GetCellUnderCursor();
        if (cell && cell.Entity) {
            isleland.RemoveUnit(cell.Entity.ID);
        }
    }

}
