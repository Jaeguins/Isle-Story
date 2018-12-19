using UnityEngine;
using System.Collections;


public class UnitCommander : MonoBehaviour {
    public CameraManager camManager;
    public TriGrid grid;
    public TriCell selectedCell;
    public Building selectedBuilding;
    public Unit selectedUnit;
    public bool requested=false;
    public Unit subject;
    public TriMesh terrainSelectionViewer;
    public Command command;
    public SizeType terrainType;
    public TriDirection dir = TriDirection.VERT;
    public void requestOrder(Unit subject,Command command) {
        requested = true;
        this.subject = subject;
        this.command = command;
    }
    public void sendOrder() {
        requested = false;
        switch (command.type) {
            case CommandType.BUILD:
                if (subject.buildingPos)
                    //subject.AddCommand(new Command(CommandType.GETOUT));
                //subject.AddCommand(new Command(CommandType.MOVE, selectedCell));
                subject.AddCommand(command);
                break;
        }

    }
    public void LateUpdate() {
        if (!requested) {
            terrainSelectionViewer.Clear();
            terrainSelectionViewer.Apply();
            return;
        }
        TriCell tCell = GetRay();
        if (tCell)
            if (tCell != selectedCell) {
                selectedCell = tCell;
                StartCalculateTerrain();
            }
        if (Input.GetMouseButtonDown(0)) {
            sendOrder();
        }
    }
    TriCell GetRay() {
        if (camManager.GetNowActive())
            return grid.GetCell(camManager.GetNowActive().CameraView.ScreenPointToRay(Input.mousePosition));
        else return null;
    }

    public void StartCalculateTerrain() {
        terrainSelectionViewer.Clear();
        switch (terrainType) {
            case SizeType.HEX:
                TriCell k = selectedCell;
                int elev = selectedCell.Elevation;
                TriDirection tDir = dir.Previous();
                for (int i = 0; i < 6; i++) {
                    if (!k) break;
                    RecalculateTerrain(k);
                    k = k.GetNeighbor(tDir);
                    tDir = tDir.Next();
                }
                break;
            case SizeType.SINGLE:
                RecalculateTerrain(selectedCell);
                break;
        }
        
        terrainSelectionViewer.Apply();
    }

    public void RecalculateTerrain(TriCell cell) {

        Vector3 nextCorner, prevCorner;
        EdgeVertices edge;
        for (TriDirection direction = TriDirection.VERT; direction <= TriDirection.RIGHT; direction++) {
            Vector3 center = cell.Position, v1, v2;
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
