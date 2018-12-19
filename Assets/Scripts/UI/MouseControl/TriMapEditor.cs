using UnityEngine;
using UnityEngine.EventSystems;
using System.IO;

public class TriMapEditor : MonoBehaviour {
    public PersonList personList;
    public TriGrid triGrid;
    public TriIsleland isleland;
    public EntityManager entities;
    public TriDirection buildDirection;
    public int x, z;
    public TriMapGenerator mapGenerator;
    public static TriMapEditor Instance;
    bool applyElevation = false;
    bool isDrag;
    TriDirection dragDirection;
    TriCell previousCell;
    int activeTerrainTypeIndex = -1;
    private void Awake() {
        Instance = this;
    }
    enum OptionalToggle {
        Ignore, Yes, No
    }
    int activeElevation;

    public void SetElevation(float elevation) {
        activeElevation = (int)elevation;
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
        entities.ClearEntities();
        triGrid.CreateMap(x, z);
        mapGenerator.GenerateMap(x, z);
        isleland.topCam.ValidatePosition();
        Selector.Instance.RequestLocation(null,SizeType.HEX, new BuildCommand(null));
    }
    public Building CreateBuilding(TriDirection dir,TriCell cell,Building prefab) {
        if(cell&& Entity.IsBuildable(dir, cell.coordinates, prefab.sizeType)) {
            Building ret = Instantiate(prefab);
            ret.ID = entities.BuildingCount;
            ret.Location = cell;
            cell.Entity = ret;
            ret.EntranceDirection = dir;
            ret.liverList = personList;
            entities.AddBuilding(ret);
            Debug.Log("camp built");
            return ret;
        }
        else {
            Debug.Log("building failed");
            return null;
        }
    }
    public void CreateHall(TriDirection dir,TriCell cell) {
        Inn ret = (Inn)CreateBuilding(dir, cell, isleland.innPrefabs[1]);
        if(ret)
            for (int i = 0; i < 4; i++) {
                Unit t = CreateUnit(cell, isleland.unitPrefabs[0]);
                ret.addPerson((Person)t);
                ((Person)t).home = ret;
            }
    }
    public Unit CreateUnit(TriCell cell,Unit prefab) {
        if (cell) {
            Unit ret=Instantiate(prefab);
            ret.ID = entities.UnitCount;
            ret.Location = cell;
            ret.Orientation = Random.Range(0f, 360f);
            entities.AddUnit(ret);
            if (cell.Entity) ret.AddCommand(new GetInCommand((Building)cell.Entity));
            return ret;
        }
        return null;
    }

    void DestroyUnit() {
        TriCell cell = GetCellUnderCursor();
        if (cell && cell.Entity) {
            entities.RemoveUnit(cell.Entity.ID);
        }
    }
    void DestroyBuilding() {
        TriCell cell = GetCellUnderCursor();
        if (cell && cell.Entity) {
            entities.RemoveBuilding(cell.Entity.ID);
        }
    }

}
