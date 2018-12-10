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
    public GameObject BuildingOptionPanel;
    public Camera buildingCam;
    public GameObject buildingCamAxis;
    public bool showTerrain = true;
    public bool clicked = false;
    bool inTopView = true;
    public float buildingCamRotSpeed=1f;
    Vector3 buildingCamAxisRot=Vector3.zero;
    void LateUpdate() {
        if (inTopView) {
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
                if (Input.GetMouseButton(0) && nowCell) {
                    clicked = true;
                }
            }
            else {
                Panel.SetActive(true);
                if (tCell.coordinates.DistanceTo(nowCell.coordinates) > 3) {
                    clicked = false;
                }
            }
        } else {
            buildingCamAxisRot.y += buildingCamRotSpeed;
            buildingCamAxis.transform.localRotation = Quaternion.Euler(buildingCamAxisRot);
        }
    }
    TriCell GetRay() {
        if (cam)
            return grid.GetCell(cam.ScreenPointToRay(Input.mousePosition));
        else return null;
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
    public void BuildingOption() {
        StartCoroutine(ToBuildingCamera());
        buildingCamAxis.transform.localPosition = nowCell.Position;
        buildingCamAxisRot = Vector3.zero;
        BuildingOptionPanel.SetActive(true);
    }
    public void ExitBuildingOption() {
        BuildingOptionPanel.SetActive(false);
        StartCoroutine(ToMainCamera());
        buildingUI.enabled = true;
        inTopView = true;
    }
    public IEnumerator ToBuildingCamera() {
        yield return StartCoroutine(Fader.FadeOut());
        terrainSelectionViewer.gameObject.SetActive(false);
        inTopView = false;
        buildingUI.enabled = false;
        cam.enabled=false;
        buildingCam.enabled = true;
        yield return StartCoroutine(Fader.FadeIn());
    }
    public IEnumerator ToMainCamera() {
        yield return StartCoroutine(Fader.FadeOut());
        terrainSelectionViewer.gameObject.SetActive(true);
        buildingCam.enabled = false;
        cam.enabled = true;
        yield return StartCoroutine(Fader.FadeIn());
    }
}
