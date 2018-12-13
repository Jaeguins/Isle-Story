using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {
    public Camera CameraView;
    public virtual bool enabled {
        get {
            return base.enabled;
        }
        set {
            base.enabled = value;
            CameraView.enabled = value;
        }
    }
}
