﻿using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System;
[Serializable]
public class Entity : MonoBehaviour {
    public int ID {
        get {
            return id;
        }
        set {
            id = value;
        }
    }
    int id;
    public static Entity unitPrefab;
    public TriCell location;
    
    public Entity GetPrefab() {
        return unitPrefab;
    }

    public TriCell Location {
        get {
            return location;
        }
        set {
            if (location) {
                location.Entity = null;
            }
            location = value;
            value.Entity = this;
            transform.localPosition = value.Position;
        }
    }

    public void Save(BinaryWriter writer, int index) {
        writer.Write(index);
        location.coordinates.Save(writer);
    }

    public static void Load(BinaryReader reader,int id) {
        TriCoordinates coordinates = TriCoordinates.Load(reader);
        float orientation = reader.ReadSingle();
        TriGrid grid = TriGrid.Instance;
        grid.AddUnit(
            Instantiate((Unit)unitPrefab), grid.GetCell(coordinates), orientation
            );
    }
 
    
    public void ValidateLocation() {
        transform.localPosition = location.Position;
    }

    public void Die() {
        location.Entity = null;
        Destroy(gameObject);
    }
}
