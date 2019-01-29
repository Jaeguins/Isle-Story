using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
public class TopViewCam : CameraController{
   
    public bool Locked {
        set {
            enabled = !value;
        }
    }
    
    void Awake() {
        swivel = transform.GetChild(0);
        stick = swivel.GetChild(0);
        
    }

    void Update() {
        float zoomDelta = Input.GetAxis("Mouse ScrollWheel");
        float zoomDelta_ = Input.GetAxis("Scroll PageUD");
        if (zoomDelta != 0f) AdjustZoom(zoomDelta);
        if (zoomDelta_ != 0f) AdjustZoom(zoomDelta_);
        float rotationDelta = Input.GetAxis("Rotation");
        if (rotationDelta != 0f) {
            AdjustRotation(rotationDelta);
        }

        float xDelta = Input.GetAxis("Horizontal");
        float zDelta = Input.GetAxis("Vertical");
        if (xDelta != 0f || zDelta != 0f) {
            AdjustPosition(xDelta, zDelta);
        }
        Vector3 t=transform.localPosition;
        t.y = grid.GetCell(TriCoordinates.FromPosition(t))?grid.GetCell(TriCoordinates.FromPosition(t)).Elevation*TriMetrics.elevationStep:0;
        transform.localPosition = t;
        

    }

    
}
