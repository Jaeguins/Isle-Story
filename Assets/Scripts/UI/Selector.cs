using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Selector : MonoBehaviour {
    public TriCell nowCell;
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
        if (Input.GetKeyDown(KeyCode.R)) {
            dir = (dir == TriDirection.RIGHT) ? TriDirection.VERT : dir + 1;
        }
        if (inTopView) {
            TriCell tCell = GetRay();
            if (!clicked) {
                nowCell = tCell;
                Panel.SetActive(false);
                nowCell = tCell;
                if (nowCell) {
                    if (showTerrain)
                        StartCalculateTerrain(dir, nowCell);
                    if (!showTerrain) {
                        terrainSelectionViewer.Clear();
                        terrainSelectionViewer.Apply();
                    }

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
                if (tCell&&nowCell&&tCell.coordinates.DistanceTo(nowCell.coordinates) > 3) {
                    clicked = false;
                }
            }
        } else {
            buildingCamAxisRot.y += buildingCamRotSpeed;
            buildingCamAxis.transform.localRotation = Quaternion.Euler(buildingCamAxisRot);
        }
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
