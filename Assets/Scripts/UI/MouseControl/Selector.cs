using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Selector : MonoBehaviour {

    public static Selector Instance;
    public CameraManager camManager;
    public TriGrid grid;
    public static Entity SelectedEntity;
    public SizeType? sizeType;
    TerrainViewer terrainSelectionViewer;
    public TriCell nowCell;
    public bool ordering = false;
    public TriDirection dir = TriDirection.VERT;
    TriCell tCell;
    Unit subject;
    Command command;
    public void RequestTarget(Unit subject, Command c) {
        this.subject = subject;
        command = c;
        ordering = true;
        sizeType = null;
    }
    public void RequestLocation(Unit subject, SizeType size, Command c) {
        this.subject = subject;
        command = c;
        ordering = true;
        sizeType = size;
        terrainSelectionViewer.enabled = true;
    }
    bool IsBuildable() {
        if (nowCell) {
            if (!nowCell.GetNeighbor(dir).IsBuildable()) return false;
            if (sizeType != null) {
                switch (sizeType) {
                    case SizeType.HEX:
                        TriCell k = nowCell;
                        int elev = nowCell.Elevation;
                        TriDirection tDir = dir.Previous();
                        for (int i = 0; i < 6; i++) {
                            if (!k || !k.IsBuildable()) return false;
                            k = k.GetNeighbor(tDir);
                            tDir = tDir.Next();
                        }
                        return true;
                    case SizeType.SINGLE:
                        return nowCell.IsBuildable();
                    default:
                        return false;
                }
            }
            return true;
        }
        else return false;

    }
    public void SendCommand() {
        if (subject)
            subject.AddCommand(command);
        else {
            GameUI.Instance.mapEditor.CreateHall(dir, nowCell);
            StartCoroutine(DelayedSave());
        }
        ordering = false;
        terrainSelectionViewer.Clear();
        terrainSelectionViewer.Apply();
        terrainSelectionViewer.enabled = false;
    }
    public IEnumerator DelayedSave() {
        yield return new WaitForSeconds(1);
        TriIsland.Instance.Save();
    }

    public void CancelCommand() {
        ordering = false;
        terrainSelectionViewer.Clear();
        terrainSelectionViewer.Apply();
        terrainSelectionViewer.enabled = false;
    }

    private void LateUpdate() {
        if (ordering) {
            tCell = GetRay();
            if (tCell) {
                nowCell = tCell;
            }
            if (Input.GetKeyDown(KeyCode.R)) {
                dir = dir.Next();
            }
            if (subject && Input.GetKeyDown(KeyCode.Escape)) {
                CancelCommand();
            }
            if (Input.GetMouseButtonDown(0)) {
                switch (command.type) {
                    case CommandType.CHANGEHOME:
                        if (SelectedEntity is Inn) {
                            ((ChangeHomeCommand)command).target = SelectedEntity as Inn;
                            SendCommand();
                        }
                        else
                            Debug.LogWarning("Invalid Target");
                        break;
                    case CommandType.CHANGEJOB:
                        if (SelectedEntity is Company) {
                            ((ChangeJobCommand)command).target = SelectedEntity as Company;
                            SendCommand();
                        }
                        else
                            Debug.LogWarning("Invalid Target");
                        break;
                    case CommandType.CHANGEWORK:
                        if (SelectedEntity is Building) {
                            ((ChangeWorkCommand)command).target = SelectedEntity as Building;
                            SendCommand();
                        }
                        else
                            Debug.LogWarning("Invalid Target");
                        break;
                    case CommandType.BUILD:
                        if (!IsBuildable()) {
                            Debug.Log("invalid build site.");
                            return;
                        }
                        ((BuildCommand)command).dir = dir;
                        ((BuildCommand)command).location = nowCell;
                        SendCommand();
                        break;
                    case CommandType.DESTROY:
                        if (SelectedEntity is Statics) {
                            ((DestroyCommand)command).target = SelectedEntity as Statics;
                            SendCommand();
                        }
                        else
                            Debug.LogWarning("Invalid Target");
                        break;

                }

            }
        }
    }

    private void Awake() {
        Instance = this;
    }

    private void Start() {
        terrainSelectionViewer = TerrainViewer.Instance;
    }

    TriCell GetRay() {
        if (camManager.GetNowActive())
            return grid.GetCell(camManager.GetNowActive().CameraView.ScreenPointToRay(Input.mousePosition));
        else return null;
    }
}