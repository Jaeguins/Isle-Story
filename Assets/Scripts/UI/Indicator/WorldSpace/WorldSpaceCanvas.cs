using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class WorldSpaceCanvas : MonoBehaviour {
    public CameraManager camManager;
    public Canvas canvas;

    public new bool enabled {
        get {
            return canvas.enabled;
        }
        set {
            canvas.enabled = value;
            active= value;
        }
    }
    bool active=true;
    // Update is called once per frame
    void LateUpdate() {
        if (active) {
            transform.rotation = camManager.GetNowActive().transform.rotation;
        }
    }
    
}
