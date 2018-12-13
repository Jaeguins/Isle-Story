using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System;
[Serializable]
public class Entity : MonoBehaviour {
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
    public virtual void BindOptions(EntityMenu menu) {
        menu.BindButton(4, "Preference", menu.ToBuildingOption);
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
