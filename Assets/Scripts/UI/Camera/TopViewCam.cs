using UnityEngine;
using System.Collections;

public class TopViewCam : CameraController{
    public TriGrid grid;
    public int border = 30;
    float clampXMin, clampZMin;
    float clampXMax, clampZMax;
    Transform swivel, stick;
    float zoom = 1f;
    float rotationAngle;
    public bool Locked {
        set {
            enabled = !value;
        }
    }
    public float rotationSpeed;
    public float stickMinZoom, stickMaxZoom;
    public float swivelMinZoom, swivelMaxZoom;
    public float moveSpeedMinZoom, moveSpeedMaxZoom;

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
        t.y = grid.GetCell(TriCoordinates.FromPosition(t)).Elevation*TriMetrics.elevationStep;
        transform.localPosition = t;
        

    }

    void AdjustRotation(float delta) {
        rotationAngle += delta * rotationSpeed * Time.deltaTime;
        if (rotationAngle < 0f) rotationAngle += 360f;
        else if (rotationAngle >= 360f) rotationAngle -= 360f;
        transform.localRotation = Quaternion.Euler(0f, rotationAngle, 0f);
    }

    void AdjustPosition(float xDelta,float zDelta) {
        Vector3 direction = transform.localRotation*new Vector3(xDelta, 0f, zDelta).normalized;
        float damping = Mathf.Max(Mathf.Abs(xDelta), Mathf.Abs(zDelta));
        float distance = Mathf.Lerp(moveSpeedMinZoom, moveSpeedMaxZoom, zoom) *
            damping * Time.deltaTime;
        Vector3 position = transform.localPosition;
        position += direction*distance;
        transform.localPosition = ClampPosition(position);
    }

    Vector3 ClampPosition(Vector3 position) {
        clampXMin = grid.GetCell(new TriCoordinates(border, border)).Position.x;
        clampZMin = grid.GetCell(new TriCoordinates(border/2, border/2)).Position.z;
        clampXMax = grid.GetCell(new TriCoordinates(grid.cellCountX - border, grid.cellCountZ - border)).Position.x;
        clampZMax = grid.GetCell(new TriCoordinates(grid.cellCountX - border/2, grid.cellCountZ - border/2)).Position.z;
        float xMax = 
            (grid.cellCountX-0.5f) *
            (2f * TriMetrics.innerRadius);
        position.x = Mathf.Clamp(position.x,clampXMin, clampXMax);
        float zMax =
                    (grid.cellCountZ -1) *
                    (1.5f * TriMetrics.outerRadius);
        position.z = Mathf.Clamp(position.z,clampZMin, clampZMax);
        return position;
    }

    void AdjustZoom(float delta) {
        zoom = Mathf.Clamp01(zoom + delta);
        float distance = Mathf.Lerp(stickMinZoom, stickMaxZoom, zoom);
        stick.localPosition = new Vector3(0f, 0f, distance);

        float angle = Mathf.Lerp(swivelMinZoom, swivelMaxZoom, zoom);
        swivel.localRotation = Quaternion.Euler(angle, 0f, 0f);
    }

    public void ValidatePosition() {
        AdjustPosition(0f, 0f);
    }
}
