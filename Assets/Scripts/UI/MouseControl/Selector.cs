using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
public class Selector : MonoBehaviour {
    public CameraManager camManager;
    public Entity selected;
    public TriCell nowCell;
    TriCell tCell;
    public BuildingMenu buildingMenu;
    public TriGrid grid;
    public bool ordering = false;
    public CommandType commandType;
    public TriMesh terrainSelectionViewer;
    public TriDirection dir = TriDirection.VERT;


    private void LateUpdate() {
        if (ordering) {

        }
        else {
            tCell = GetRay();
            if (tCell) {
                if (tCell != nowCell) {
                    nowCell = tCell;
                    StartCalculateTerrain();
                }
            }
        }
    }

    public void StartCalculateTerrain() {
        terrainSelectionViewer.Clear();
        TriCell k = nowCell;
        int elev = nowCell.Elevation;
        TriDirection tDir = dir.Previous();
        for (int i = 0; i < 6; i++) {
            if (!k) break;
            RecalculateTerrain(k);
            k = k.GetNeighbor(tDir);
            tDir = tDir.Next();
        }
        terrainSelectionViewer.Apply();
    }

    public void RecalculateTerrain(TriCell cell) {
        Vector3 nextCorner, prevCorner;
        EdgeVertices edge;
        for (TriDirection direction = TriDirection.VERT; direction <= TriDirection.RIGHT; direction++) {
            Vector3 center = cell.Position, v1, v2;
            buildingMenu.transform.localPosition = center + new Vector3(0, 20, 0);
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
            return grid.GetCell(camManager.GetNowActive().cam.ScreenPointToRay(Input.mousePosition));
        else return null;
    }
}
public class Selector_ : MonoBehaviour {
    public TriCell nowCell;
    TriCell tCell;
    public TriGrid grid;
    public Building nowBuilding;
    public BuildingMenu buildingMenu;
    public TriMesh terrainSelectionViewer;
    public GameObject Panel;
    public WorldSpaceCanvas BuildingOptionPanel;
    public CameraManager camManager;
    public GameObject buildingCamAxis;
    
    public bool showTerrain = true;
    public bool clicked = false;
    public TriDirection dir=TriDirection.VERT;
    bool inTopView = true;
    public float buildingCamRotSpeed=1f;
    Vector3 buildingCamAxisRot=Vector3.zero;
    
    void LateUpdate() {
        


        if (nowCell&&showTerrain)
            StartCalculateTerrain(dir, nowCell);
        else if (!showTerrain) {
            terrainSelectionViewer.Clear();
            terrainSelectionViewer.Apply();
        }
         tCell = GetRay();
        if (!clicked) {
            nowCell = tCell;
            Panel.SetActive(false);
            nowCell = tCell;
            if (nowCell) {


                nowBuilding = nowCell.Building;
                if (nowBuilding) {
                    buildingMenu.enabled = true;
                    buildingMenu.Bind(nowBuilding);
                }
                else {
                    buildingMenu.enabled = false;
                }
            }
            buildingMenu.transform.rotation = camManager.GetNowActive().transform.rotation;
            if (Input.GetMouseButton(0) && nowCell) {
                clicked = true;
            }
        }
        else {
            Panel.SetActive(true);
            if (tCell && nowCell && tCell.coordinates.DistanceTo(nowCell.coordinates) > 3) {
                clicked = false;
            }
        }
    }
    public void RotateDir() {
        dir = dir.Next();
    }
    TriCell GetRay() {
        if (camManager.GetNowActive())
            return grid.GetCell(camManager.GetNowActive().cam.ScreenPointToRay(Input.mousePosition));
        else return null;
    }
    public void StartCalculateTerrain(TriDirection dir,TriCell cell) {
        terrainSelectionViewer.Clear();
        TriCell k = cell;
        int elev = cell.Elevation;
        TriDirection tDir = dir.Previous();
        for(int i = 0; i < 6; i++) {
            if (!k) break;
            RecalculateTerrain(k);
            k = k.GetNeighbor(tDir);
            tDir = tDir.Next();
        }
        terrainSelectionViewer.Apply();
    }
    public void RecalculateTerrain(TriCell cell) {
        
        Vector3 nextCorner, prevCorner;
        EdgeVertices edge;
        for (TriDirection direction = TriDirection.VERT; direction <= TriDirection.RIGHT; direction++) {
            Vector3 center = cell.Position, v1, v2;
            buildingMenu.transform.localPosition = center + new Vector3(0, 20, 0);
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
}
