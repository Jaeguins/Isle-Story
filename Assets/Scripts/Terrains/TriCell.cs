using UnityEngine;
using System.Collections;

public class TriCell : MonoBehaviour {
    public TriCoordinates coordinates;
    public Color color;
    public bool inverted=false;
    public RectTransform uiRect;
    public int Elevation {
        get {
            return elevation;
        }
        set {
            elevation = value;
            Vector3 position = transform.localPosition;
            position.y = value * TriMetrics.elevationStep;
            transform.localPosition = position;

            Vector3 uiPosition = uiRect.localPosition;
            uiPosition.z = elevation * -TriMetrics.elevationStep;
            uiRect.localPosition = uiPosition;
        }
    }
    int elevation;
    [SerializeField]
    TriCell[] neighbors;
    public TriCell GetNeighbor(TriDirection direction) {
        return neighbors[(int)direction];
    }
    public void SetNeighbor(TriDirection direction, TriCell cell) {
        neighbors[(int)direction] = cell;
        cell.neighbors[(int)direction] = this;
    }
}
