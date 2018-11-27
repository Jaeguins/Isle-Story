using UnityEngine;
using UnityEngine.EventSystems;
using System.IO;

public class TriMapEditor : MonoBehaviour {
    public TriGrid triGrid;
    bool applyElevation = false;
    bool isDrag;
    TriDirection dragDirection;
    TriCell previousCell;
    int activeTerrainTypeIndex=-1;
    enum OptionalToggle {
        Ignore, Yes, No
    }
    int activeElevation;

    public void SetElevation(float elevation) {
        activeElevation = (int)elevation;
    }

    private void Awake() {}
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

    OptionalToggle riverMode=OptionalToggle.Yes;
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
        triGrid.CreateMap(20, 20);
        TriMapCamera.ValidatePosition();
    }
}
