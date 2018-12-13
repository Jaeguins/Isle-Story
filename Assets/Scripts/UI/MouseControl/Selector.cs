using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
public enum SelectedType {
    NONE,UNIT,BUILDING,NATURAL
}
public class Selector : MonoBehaviour {
    public static Selector Instance;
    public CameraManager camManager;
    public Entity selected;
    public Entity target;
    public TriCell nowCell;
    TriCell tCell;
    public EntityMenu entityMenu;
    public TriGrid grid;
    public bool ordering = false;
    public bool selectCheck = false;
    public bool terrainView = false;
    public SelectedType selectedType;
    public CommandType commandType;
    public TriMesh terrainSelectionViewer;
    public TriDirection dir = TriDirection.VERT;
    public SizeType sizeType;
    bool terrainCleared = true;
    private void Start() {
        Instance = this;
    }

    private void LateUpdate() {
        tCell = GetRay();
        if (tCell) {
            if (selectedType==SelectedType.UNIT||!selectCheck&&tCell != nowCell) {
                nowCell = tCell;
            }
        }
        if (selectCheck) {
            if (selectedType==SelectedType.BUILDING&&tCell && nowCell && tCell.coordinates.DistanceTo(nowCell.coordinates) > 3) {
                Deselect();
            }
        }
        if (Input.GetMouseButtonDown(1)) {
            GameUI.Instance.mapEditor.CreateHall(dir, nowCell);
        }
        if (ordering) {
            entityMenu.enabled = false;
            switch (commandType) {
                case CommandType.BUILD:
                    terrainView = true;
                    break;
            }
            if (Input.GetMouseButtonDown(0)&&selectedType==SelectedType.UNIT) {
                switch (commandType) {
                    case CommandType.BUILD:
                        ((Unit)selected).AddCommand(new Command(commandType, dir, target));
                        Debug.Log(selected.ID);
                        ordering = false;
                        terrainView = false;
                        terrainCleared = true;
                        Deselect();
                        break;
                }
                
            }

        }
        else {
            if (camManager.camStatus==CamType.TOPVIEW&&nowCell&&nowCell.Entity && Input.GetMouseButton(0)&&selectedType==SelectedType.NONE) {
                selected = nowCell.Entity;
                selectedType = SelectedType.BUILDING;
                selectCheck = true;
                entityMenu.enabled = true;
                entityMenu.Bind(selected);
            }
        }

        if (terrainView) {
            StartCalculateTerrain();
        }
        else if (terrainCleared) {
            terrainSelectionViewer.Clear();
            terrainSelectionViewer.Apply();
            terrainCleared = false;
        }
        if (Input.GetKeyDown(KeyCode.R)) {
            dir = dir.Next();
        }

    }

    public void SelectUnit(Unit unit) {
        selected = unit;
        selectedType = SelectedType.UNIT;
        selectCheck = true;
        entityMenu.enabled = true;
        entityMenu.Bind(selected);

    }
    public void Deselect() {
        entityMenu.enabled = false;
        selectCheck = false;
        selectedType = SelectedType.NONE;
    }

    public void StartCalculateTerrain() {
        terrainSelectionViewer.Clear();
        switch (sizeType) {
            case SizeType.HEX:
                TriCell k = nowCell;
                int elev = nowCell.Elevation;
                TriDirection tDir = dir.Previous();
                for (int i = 0; i < 6; i++) {
                    if (!k) break;
                    RecalculateTerrain(k);
                    k = k.GetNeighbor(tDir);
                    tDir = tDir.Next();
                }
                break;
            case SizeType.SINGLE:
                RecalculateTerrain(nowCell);
                break;
        }

        terrainSelectionViewer.Apply();
    }

    public void RecalculateTerrain(TriCell cell) {
        Vector3 nextCorner, prevCorner;
        EdgeVertices edge;
        for (TriDirection direction = TriDirection.VERT; direction <= TriDirection.RIGHT; direction++) {
            Vector3 center = cell.Position, v1, v2;
            entityMenu.transform.localPosition = center + new Vector3(0, 20, 0);
            v1 = center + (cell.inverted ? -1 : 1) * TriMetrics.GetFirstSolidCorner(direction);
            v2 = center + (cell.inverted ? -1 : 1) * TriMetrics.GetSecondSolidCorner(direction);
            edge = new EdgeVertices(v1, v2);
            nextCorner = (center + edge.v1) / 2f;
            prevCorner = (center + edge.v5) / 2f;
            terrainSelectionViewer.AddTriangle(nextCorner, edge.v1, edge.v2);
            terrainSelectionViewer.AddTriangle(prevCorner, edge.v4, edge.v5);
            terrainSelectionViewer.AddTriangle(center, edge.v2, edge.v4);
            terrainSelectionViewer.AddTriangle(center, edge.v4, prevCorner);
            terrainSelectionViewer.AddTriangle(center, nextCorner, edge.v2);

            terrainSelectionViewer.AddTriangleColor(Color.blue);
            terrainSelectionViewer.AddTriangleColor(Color.blue);
            terrainSelectionViewer.AddTriangleColor(Color.blue);
            terrainSelectionViewer.AddTriangleColor(Color.blue);
            terrainSelectionViewer.AddTriangleColor(Color.blue);
        }
    }

    TriCell GetRay() {
        if (camManager.GetNowActive())
            return grid.GetCell(camManager.GetNowActive().CameraView.ScreenPointToRay(Input.mousePosition));
        else return null;
    }
}