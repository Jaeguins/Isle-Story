﻿using UnityEngine;
using UnityEngine.EventSystems;
using System.IO;
using System.Collections;
using System.Collections.Generic;
public class TriMapEditor : MonoBehaviour {
    public EntityList<Unit> personList;
    public TriGrid triGrid;
    public TriIsland island;
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
    public void NewMap() {
        TriIsland.Instance.NewMap();
    }
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
        island.Save();
    }

    public void Load() {
        island.Load();
    }
    
    
    public Building CreateBuilding(TriDirection dir, TriCell cell, Building prefab) {
        List<BuildState> result = prefab.GetBuildStatus(cell.coordinates,dir);
        bool buildable=true;
        foreach(BuildState i in result) if (!i.value) buildable = false;
        if (cell && buildable) {
            Building ret = Instantiate(prefab);
            ret.ID = entities.BuildingCount;
            ret.Location = cell;
            switch (ret.sizeType) {
                case SizeType.SINGLE:
                    cell.Statics = ret;
                    break;
                case SizeType.HEX:
                    TriCell k = cell;
                    TriDirection tDir = dir.Previous();
                    for (int i = 0; i < 6; i++) {
                        if (!k) break;
                        k.Statics = ret;
                        k = k.GetNeighbor(tDir);
                        tDir = tDir.Next();
                    }
                    break;
            }
            ret.EntranceDirection = dir;
            ret.personList = personList;
            entities.AddBuilding(ret);
            island.ShowSummary = island.ShowSummary;
            Debug.Log(ret+" built");
            return ret;
        }
        else {
            Debug.Log("building failed");
            return null;
        }
    }
    public void CreateHall(TriDirection dir, TriCell cell) {
        Hall ret = (Hall)CreateBuilding(dir, cell, (Building)TriIsland.GetBuildingPrefabs((int)BuildingType.HALL, (int)HallType.BASE, 0));
        if (ret) {
            TriIsland.Instance.entities.camp = ret;
            for (int i = 0; i < 4; i++) {
                Unit t = CreateUnit(ret.EntranceLocation, (Unit)TriIsland.GetUnitPrefabs((int)UnitType.PERSON, 0));
                ((Human)t).Home = null;
                t.AddCommand(new GetInCommand(ret));
            }
            ret.Working = true;
        }
    }
    public Unit CreateUnit(TriCell cell, Unit prefab) {
        if (cell) {
            Unit ret = Instantiate(prefab);
            ret.ID = entities.UnitCount;
            ret.Location = cell;
            ret.Orientation = Random.Range(0f, 360f);
            entities.AddUnit(ret);
            if (cell.Statics) ret.AddCommand(new GetInCommand((Building)cell.Statics));
            return ret;
        }
        return null;
    }

    void DestroyUnit() {
        TriCell cell = GetCellUnderCursor();
        if (cell && cell.Statics) {
            entities.RemoveUnit(cell.Statics.ID);
        }
    }
    void DestroyBuilding() {
        TriCell cell = GetCellUnderCursor();
        if (cell && cell.Statics) {
            entities.RemoveBuilding(cell.Statics.ID);
        }
    }

}
