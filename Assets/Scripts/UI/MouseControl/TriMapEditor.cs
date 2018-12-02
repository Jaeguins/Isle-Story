using UnityEngine;
using UnityEngine.EventSystems;
using System.IO;

public class TriMapEditor : MonoBehaviour {
    public TriGrid triGrid;
    public int x, z;
    public TriMapGenerator mapGenerator;
    public Entities entityPrefab;
    bool applyElevation = false;
    bool isDrag;
    TriDirection dragDirection;
    TriCell previousCell,searchFromCell, searchToCell;
    int activeTerrainTypeIndex = -1;
    enum OptionalToggle {
        Ignore, Yes, No
    }
    int activeElevation;

    public void SetElevation(float elevation) {
        activeElevation = (int)elevation;
    }

    private void Awake() { }
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
        Ray inputRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(inputRay, out hit)) {
            return triGrid.GetCell(hit.point);
        }
        return null;
    }
    
    void HandleInput() {
        TriCell currentCell = GetCellUnderCursor();
        if (Input.GetKey(KeyCode.LeftShift)) {
            searchFromCell = currentCell;
            if (searchToCell) {
                triGrid.FindPath(searchFromCell, searchToCell);
            }
        }
        else if (currentCell) {
            if (previousCell && previousCell != currentCell) {
                ValidateDrag(currentCell);
            }
            else {
                isDrag = false;
            }
            //EditCell(currentCell);
            previousCell = currentCell;
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
        string path = Path.Combine(Application.persistentDataPath, "test.map");
        using (
            BinaryWriter writer =
                new BinaryWriter(File.Open(path, FileMode.Create))
        ) {
            writer.Write(0);
            triGrid.Save(writer);
        }
    }
    public void Load() {
        StopAllCoroutines();
        string path = Path.Combine(Application.persistentDataPath, "test.map");
        using (
            BinaryReader reader =
                new BinaryReader(File.OpenRead(path))
        ) {
            int header = reader.ReadInt32();
            if (header == 0) {
                triGrid.Load(reader);
                TriMapCamera.ValidatePosition();
            }
            else {
                Debug.LogWarning("Unknown map format " + header);
            }
        }
    }
    public void NewMap() {
        triGrid.CreateMap(x, z);
        mapGenerator.GenerateMap(x, z);
        TriMapCamera.ValidatePosition();
    }

    void CreateUnit() {
        TriCell cell = GetCellUnderCursor();
        if (cell && !cell.Entity) {
            Debug.Log(cell.coordinates);
            Entities unit = Instantiate(entityPrefab);
            unit.transform.SetParent(triGrid.transform, false);
            unit.Location = cell;
            unit.Orientation = Random.Range(0f, 360f);
        }
    }
    void DestroyUnit() {
        TriCell cell = GetCellUnderCursor();
        if (cell && cell.Entity) {
            cell.Entity.Die();
        }
    }

}
