using UnityEngine;
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
    TriCell location;
    
    public Entity GetPrefab() {
        return unitPrefab;
    }

    public TriCell Location {
        get {
            return location;
        }
        set {
            location = value;
            value.Entity = this;
            transform.localPosition = value.Position;
        }
    }

    public void Save(BinaryWriter writer) {
        location.coordinates.Save(writer);
    }
    
    public void ValidateLocation() {
        transform.localPosition = location.Position;
    }

    public void Die() {
        location.Entity = null;
        Destroy(gameObject);
    }
}
