using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
public enum EntityType {
    Building, Natural, Unit
}
[Serializable]
public abstract class Entity : MonoBehaviour {
    public bool Clickable = true;
    public bool Targetable = true;
    public Animator animator;
    public bool Working = false;
    public bool Stepable = false;
    public bool Dragable = false;
    public SelectionIndicator SelectionIndicator;
    public SelectionIndicator HoverIndicator;
    public ResourceController resourceController;
    public SizeType sizeType;
    public EntityType EntityType;
    public string UIType, UIName, UIStatus = "NaN";
    protected BoxCollider col;
    public virtual void Start() {
        StartCoroutine(InternalCoroutine());
    }
    public virtual void Awake() {
        col = gameObject.GetComponent<BoxCollider>();
    }
    public int ID {
        get {
            return id;
        }
        set {
            id = value;
        }
    }
    int id;
    public Vector3 camAnchorOffset, camOffset;
    public static Entity unitPrefab;
    protected TriCell location;

    public Entity GetPrefab() {
        return unitPrefab;
    }

    public TriCell Location {
        get {
            return location;
        }
        set {
            location = value;
            transform.localPosition = value.Position;
        }
    }

    /*
     * save sequence
     * location
     */

    public virtual void Save(BinaryWriter writer) {
        if (location)
            location.coordinates.Save(writer);
        else
            new TriCoordinates(-1, -1).Save(writer);
    }


    public void ValidateLocation() {
        transform.localPosition = location.Position;
    }

    public void Die() {
        location.Statics = null;
        Destroy(gameObject);
    }
    public IEnumerator InternalCoroutine() {
        while (true) {
            if (TriIsland.Loaded) Tick();
            else Debug.Log(this.ToString() + "skipped ticking");
            yield return new WaitForSeconds(0.1f);
        }
    }
    public virtual void Tick() {
    }
    public void OnMouseEnter() {
        if (!Targetable) return;
        if (EventSystem.current.IsPointerOverGameObject()) return;
        HoverIndicator.Select();
        if(Dragable)
        Selector.Instance.DragEnd = this;
    }
    public void OnMouseDrag() {
        Selector.Instance.ShowDrag();
    }
    public void OnMouseExit() {
        if (!Targetable) return;
        HoverIndicator.Deselect();
    }
    public virtual void OnMouseDown() {
        if (!Clickable) return;
        if (EventSystem.current.IsPointerOverGameObject()) return;
        if (Targetable)
            Selector.SelectedEntity = this;
        if (Dragable)
            Selector.Instance.DragStart = this;
        if (Selector.Instance.ordering) return;
        Debug.Log(ToString() + " selected");
        SelectionIndicator.Select();
        EntityView.Instance.Clear();
        EntityView.Instance.Bind(this);
        Debug.Log("mouse down " + this);
    }
    public virtual List<BuildState> GetBuildStatus(TriCoordinates coord, TriDirection dir) {
        List<BuildState> ret = new List<BuildState>();
        TriGrid grid = TriGrid.Instance;
        TriCell cell = grid.GetCell(coord);
        int elev = cell.Elevation;
        ret.Add(new BuildState(cell.coordinates, cell.IsBuildable()));
        cell = grid.GetCell(coord).GetNeighbor(dir);
        ret.Add(new BuildState(cell.coordinates, cell.IsBuildable() && Mathf.Abs(cell.Elevation - elev) < 2));
        return ret;
    }
    public virtual void BindCells(bool flag) {

    }
}
public struct BuildState {
    public TriCoordinates coord;
    public bool value;
    public BuildState(TriCoordinates coord, bool value) {
        this.coord = coord;
        this.value = value;
    }
}
