using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
public class TopViewCam : CameraController {
    Coroutine rotator;
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
        float xDelta = Input.GetAxis("Horizontal");
        float zDelta = Input.GetAxis("Vertical");
        if (xDelta != 0f || zDelta != 0f) {
            AdjustPosition(xDelta, zDelta);
        }
        if (Input.GetKeyDown(KeyCode.Q)) {
            if (rotator != null) StopCoroutine(rotator);
            rotator = StartCoroutine(AdjustRotation(false));
        }
        if (Input.GetKeyDown(KeyCode.E)) {
            if (rotator != null) StopCoroutine(rotator);
            rotator = StartCoroutine(AdjustRotation(true));
        }
        //Vector3 t=transform.localPosition;
        //t.y = grid.GetCell(TriCoordinates.FromPosition(t))?grid.GetCell(TriCoordinates.FromPosition(t)).Elevation*TriMetrics.elevationStep:0;
        //transform.localPosition = t;

    }


}
