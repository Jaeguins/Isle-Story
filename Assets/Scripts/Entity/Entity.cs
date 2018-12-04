using UnityEngine;
using System.Collections.Generic;
using System.IO;

public class Entity : MonoBehaviour {
    public static Entity unitPrefab;
    public TriCell location;
    float orientation;
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

    public void Save(BinaryWriter writer) {
        location.coordinates.Save(writer);
        writer.Write(orientation);
    }

    public static void Load(BinaryReader reader) {
        TriCoordinates coordinates = TriCoordinates.Load(reader);
        float orientation = reader.ReadSingle();
        TriGrid grid = TriGrid.Instance;
        grid.AddUnit(
            Instantiate(unitPrefab), grid.GetCell(coordinates), orientation
            );
    }
    public float Orientation {
        get {
            return orientation;
        }
        set {
            orientation = value;
            transform.localRotation = Quaternion.Euler(0f, value, 0f);
        }
    }
    
    public void ValidateLocation() {
        transform.localPosition = location.Position;
    }

    public void Die() {
        location.Entity = null;
        Destroy(gameObject);
    }
}
