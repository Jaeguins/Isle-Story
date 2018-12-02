using UnityEngine;
using System.Collections.Generic;
using System.IO;

public class Entities : MonoBehaviour {
    public static Entities unitPrefab;
    List<TriCell> pathToTravel;
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
    TriCell location;
    public float Orientation {
        get {
            return orientation;
        }
        set {
            orientation = value;
            transform.localRotation = Quaternion.Euler(0f, value, 0f);
        }
    }
    float orientation;
    public void ValidateLocation() {
        transform.localPosition = location.Position;
    }

    public void Die() {
        location.Entity = null;
        Destroy(gameObject);
    }

    public bool IsValidDestination(TriCell cell) {
        return !cell.IsUnderwater && !cell.Entity;
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

    public void Travel(List<TriCell> path) {
        Location = path[path.Count - 1];
        pathToTravel = path;
    }

    void OnDrawGizmos() {
        if (pathToTravel == null || pathToTravel.Count == 0) {
            return;
        }

        for (int i = 1; i < pathToTravel.Count; i++) {
            Vector3 a = pathToTravel[i - 1].Position;
            Vector3 b = pathToTravel[i].Position;
            for (float t = 0f; t < 1f; t += 0.1f) {
                Gizmos.DrawSphere(Vector3.Lerp(a, b, t), 2f);
            }
        }
    }




}
