using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Draggable : MonoBehaviour
{
    public RectTransform tp;
    public Vector3 originalLoc;
    public Vector3 originalCur;
    public void Start() {

    }
    public void OnMouseDrag() {
        if (EventSystem.current.currentSelectedGameObject != gameObject) return;
        tp.position = originalLoc + (Input.mousePosition- originalCur);
    }
    public void OnMouseDown() {
        if (EventSystem.current.currentSelectedGameObject != gameObject) return;
        originalLoc = tp.position;
        originalCur = Input.mousePosition;
    }
}
