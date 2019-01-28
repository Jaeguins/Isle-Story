using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Draggable : MonoBehaviour
{
    public RectTransform tp;
    public Vector3 originalLoc;
    public Vector3 originalCur;
    public void Start() {

    }
    public void OnMouseDrag() {
        tp.position = originalLoc + (Input.mousePosition- originalCur);
        Debug.Log("dragging " + tp.position);
    }
    public void OnMouseDown() {
        originalLoc = tp.position;
        originalCur = Input.mousePosition;
    }
    public void OnMouseUp() {
        
    }
}
