using UnityEngine;
using System.Collections;

public class IndivViewCam : CameraController{
    public Transform Rotator;
    public bool activated = false;
    public static float speed = 0.3f;
    Vector3 rot = Vector3.zero;
    public override bool enabled {
        get {
            return base.enabled;
        }
        set {
            base.enabled = value;
            
            if (value) StartRot();
            else EndRot();
        }
    }
    public void SetOffset(Entity entity) {
        CameraView.transform.localPosition = entity.camOffset;
    }
    public void SetAnchor(Entity entity) {
        transform.localPosition = entity.Location.transform.position+entity.camAnchorOffset;
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
