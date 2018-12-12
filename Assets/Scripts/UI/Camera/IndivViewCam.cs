using UnityEngine;
using System.Collections;

public class IndivViewCam : CameraController{
    public Transform Rotator;
    public bool activated = false;
    public static float speed = 0.3f;
    Vector3 rot = Vector3.zero;
    public void SetOffset(Building building) {
        transform.localPosition = building.camOffset;
    }
    public void setAnchor(Building building) {
        Rotator.localPosition = building.Location.transform.localPosition+building.camAnchorOffset;
    }
    private void Awake() {
        cam = GetComponent<Camera>();
    }
    private void Update() {
        if (activated) {
            Rotator.rotation = Quaternion.Euler(rot);
            rot.y += speed;
        }
    }
    public void StartRot() {
        activated = true;
        rot = Vector3.zero;
    }
    public void EndRot() {
        activated = false;
    }
    
}
