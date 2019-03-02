using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class WorldSpaceCanvas : MonoBehaviour {
    public CameraManager cameraManager;
    public Canvas canvas;
    public void Start() {
        cameraManager = CameraManager.Instance;
    }
    public new bool enabled {
        get {
            return canvas.enabled;
        }
        set {
            canvas.enabled = value;
            active= value;
        }
    }
    public bool active=true;
    // Update is called once per frame
    void Update() {
        if (active) {
            transform.rotation = cameraManager.GetNowActive().transform.rotation;
        }
    }
    
}
