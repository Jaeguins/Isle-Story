using UnityEngine;
using System.Collections;

public class Entities : MonoBehaviour {
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
        location.Unit = null;
        Destroy(gameObject);
    }
}
