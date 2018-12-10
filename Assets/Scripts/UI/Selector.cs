using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Selector : MonoBehaviour {
    public TriCell nowCell;
    public TriGrid grid;
    public Building nowBuilding;
    public Canvas buildingUI;
    public Text buildingNameTag;
    public TriMesh terrainSelectionViewer;
    public Camera cam;
    public GameObject Panel;
    public bool showTerrain = true;
    public bool clicked = false;
    void Update() {
        TriCell tCell = GetRay();
        if (!clicked) {
            nowCell = tCell;
            Panel.SetActive(false);
            nowCell = tCell;
            if (nowCell) {
                if (showTerrain)
                    RecalculateTerrain();
                if (!showTerrain) {
                    terrainSelectionViewer.Clear();
                    terrainSelectionViewer.Apply();
                }

                nowBuilding = nowCell.Building;
                if (nowBuilding) {
                    buildingUI.enabled = true;
                    buildingNameTag.text = nowBuilding.UIName;
                }
                else {
                    buildingUI.enabled = false;
                }
            }
            buildingUI.transform.rotation = cam.transform.rotation;
            if (Input.GetMouseButton(0)&&nowCell) {
                clicked = true;
            }
        }
        else {
            Panel.SetActive(true);
            if (tCell.coordinates.DistanceTo(nowCell.coordinates)>3) {
                clicked = false;
            }
        }
        
    }
    TriCell GetRay() {
        return grid.GetCell(Camera.main.ScreenPointToRay(Input.mousePosition));
    }
    public void RecalculateTerrain() {
        terrainSelectionViewer.Clear();
        Vector3 nextCorner, prevCorner;
        EdgeVertices edge;
        for (TriDirection direction = TriDirection.VERT; direction <= TriDirection.RIGHT; direction++) {
            Vector3 center = nowCell.Position, v1, v2;
            buildingUI.transform.localPosition = center + new Vector3(0, 20, 0);
            v1 = center + (nowCell.inverted ? -1 : 1) * TriMetrics.GetFirstSolidCorner(direction);
            v2 = center + (nowCell.inverted ? -1 : 1) * TriMetrics.GetSecondSolidCorner(direction);
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
        terrainSelectionViewer.Apply();
    }
}
