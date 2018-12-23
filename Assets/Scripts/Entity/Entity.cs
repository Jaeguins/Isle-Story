﻿using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System;
[Serializable]
public class Entity : MonoBehaviour {
    public SizeType sizeType;
    public List<Unit> insider;
    public string UIName;
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
    public virtual void BindOptions(CommandPanel menu) {
        menu.BindButton(0, "Status", menu.UnitStatus);
    }
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
        location.Entity = null;
        Destroy(gameObject);
    }
    public static bool IsBuildable(TriDirection dir, TriCoordinates coord, SizeType sizeType) {
        switch (sizeType) {
            case SizeType.SINGLE:
                if (TriGrid.Instance.GetCell(coord).Entity) return false;
                else return true;
            case SizeType.HEX:
                TriCell cell = TriGrid.Instance.GetCell(coord);
                int elevation = cell.Elevation;
                TriCell k = cell;
                int elev = cell.Elevation;
                TriDirection tDir = dir.Previous();
                for (int i = 0; i < 6; i++) {
                    if (!k || !k.IsBuildable()) return false;
                    if (elev != k.Elevation) return false;
                    k = k.GetNeighbor(tDir);
                    tDir = tDir.Next();
                }
                return true;
            default:
                return false;
        }
    }
}
