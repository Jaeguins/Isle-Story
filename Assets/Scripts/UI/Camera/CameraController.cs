using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {
    public float clampXMin, clampZMin;
    public float clampXMax, clampZMax;
    public int border = 30;
    public TriGrid grid;
    public Transform swivel, stick;
    public float rotationSpeed;
    public float stickMinZoom, stickMaxZoom;
    public float swivelMinZoom, swivelMaxZoom;
    public float moveSpeedMinZoom, moveSpeedMaxZoom;

    public float zoom = 1f;
    public float rotationAngle;
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
    public IEnumerator AdjustRotation(bool clockwise) {
        rotationAngle += (clockwise?1:-1) * rotationSpeed;
        rotationAngle -= rotationAngle % 60;
        if (rotationAngle < 0f) rotationAngle += 360f;
        else if (rotationAngle >= 360f) rotationAngle -= 360f;
        Quaternion newQuat= Quaternion.Euler(0f, rotationAngle, 0f);
        Quaternion pastQuat = transform.localRotation;
        for(float i = 0; i <= 10; i++) {
            transform.localRotation = Quaternion.Lerp(pastQuat, newQuat, i / 10);
            yield return new WaitForEndOfFrame();
        }
        
        
    }
    public void AdjustPosition(Vector3 position) {
        AdjustPosition(position.x, position.y,position.z);
    }
    public void AdjustPosition(Vector2 position) {
        AdjustPosition(position.x, position.y);
    }
    public void AdjustPosition(float xDelta, float zDelta) {
        Vector3 direction = transform.localRotation * new Vector3(xDelta, 0f, zDelta).normalized;
        float damping = Mathf.Max(Mathf.Abs(xDelta), Mathf.Abs(zDelta));
        float distance = Mathf.Lerp(moveSpeedMinZoom, moveSpeedMaxZoom, zoom) *
            damping * Time.deltaTime;
        Vector3 position = transform.localPosition;
        position += direction * distance;
        transform.localPosition = ClampPosition(position);
    }
    public void AdjustPosition(float xDelta,float yDelta, float zDelta) {
        Vector3 direction = transform.localRotation * new Vector3(xDelta, yDelta, zDelta).normalized;
        float damping = Mathf.Max(Mathf.Abs(xDelta), Mathf.Abs(zDelta));
        float distance = Mathf.Lerp(moveSpeedMinZoom, moveSpeedMaxZoom, zoom) *
            damping * Time.deltaTime;
        Vector3 position = transform.localPosition;
        position += direction * distance;
        transform.localPosition = ClampPosition(position);
    }

    public Vector3 ClampPosition(Vector3 position) {
        clampXMin = grid.GetCell(new TriCoordinates(border, border)).Position.x;
        clampZMin = grid.GetCell(new TriCoordinates(border / 2, border / 2)).Position.z;
        clampXMax = grid.GetCell(new TriCoordinates(grid.cellCountX - border, grid.cellCountZ - border)).Position.x;
        clampZMax = grid.GetCell(new TriCoordinates(grid.cellCountX - border / 2, grid.cellCountZ - border / 2)).Position.z;
        float xMax =
            (grid.cellCountX - 0.5f) *
            (2f * TriMetrics.innerRadius);
        position.x = Mathf.Clamp(position.x, clampXMin, clampXMax);
        float zMax =
                    (grid.cellCountZ - 1) *
                    (1.5f * TriMetrics.outerRadius);
        position.z = Mathf.Clamp(position.z, clampZMin, clampZMax);
        return position;
    }
    
    public void AdjustZoom(float delta) {
        /*
        zoom = Mathf.Clamp01(zoom + delta);
        float distance = Mathf.Lerp(stickMinZoom, stickMaxZoom, zoom);
        stick.localPosition = new Vector3(0f, 0f, distance);

        float angle = Mathf.Lerp(swivelMinZoom, swivelMaxZoom, zoom);
        swivel.localRotation = Quaternion.Euler(angle, 0f, 0f);
        */
    }

    public void ValidatePosition() {
        AdjustPosition(0f, 0f);
    }
}
