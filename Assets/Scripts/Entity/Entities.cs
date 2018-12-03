using UnityEngine;
using System.Collections.Generic;
using System.IO;

public class Entities : MonoBehaviour {
    public static Entities unitPrefab;
    public Animator animator;
    List<TriCell> pathToTravel;
    const float travelSpeed = 4f;

    public Entities GetPrefab() {
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
        StopAllCoroutines();
        animator.SetBool("walking", true);
        StartCoroutine(TravelPath());
    }

    void OnDrawGizmos() {
        if (pathToTravel == null || pathToTravel.Count == 0) {
            return;
        }
        Vector3 a, b, c = pathToTravel[0].Position;
        for (int i = 1; i < pathToTravel.Count; i++) {
            a = c;
            b = pathToTravel[i - 1].Position;
            c = (b + pathToTravel[i].Position) * 0.5f;
            for (float t = 0f; t < 1f; t += 0.1f) {
                Gizmos.DrawSphere(Bezier.GetPoint(a,b,c,t), 2f);
            }
        }
        a = c;
        b = pathToTravel[pathToTravel.Count - 1].Position;
        c = b;
        for(float t = 0f; t < 1f; t += 0.1f) {
            Gizmos.DrawSphere(Bezier.GetPoint(a, b, c, t), 2f);
        }
        
    }
    IEnumerator<WaitForEndOfFrame> TravelPath() {
        Vector3 a, b, c = pathToTravel[0].Position;
        float t = Time.deltaTime * travelSpeed;
        for (int i = 1; i < pathToTravel.Count; i++) {
            a = c;
            b = pathToTravel[i - 1].Position;
            c=(b+pathToTravel[i].Position)*0.5f;
            for (; t < 1f; t += Time.deltaTime*travelSpeed) {
                transform.localPosition = Bezier.GetPoint(a, b, c, t);
                Vector3 d = Bezier.GetDerivative(a, b, c, t);
                //d.y = 0f;
                transform.localRotation = Quaternion.LookRotation(d);
                yield return null;
            }
            t -= 1f;
        }
        a = c;
        b = pathToTravel[pathToTravel.Count - 1].Position;
        c = b;
        for (; t < 1f; t += Time.deltaTime * travelSpeed) {
            transform.localPosition = Bezier.GetPoint(a, b, c, t);
            Vector3 d = Bezier.GetDerivative(a, b, c, t);
            //d.y = 0f;
            transform.localRotation = Quaternion.LookRotation(d);
            yield return null;
        }
        animator.SetBool("walking", false);
    }



}
