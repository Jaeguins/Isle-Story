using UnityEngine;
using System.Collections;

public class TriCell : MonoBehaviour {
    public TriCoordinates coordinates;
    public Color color;
    public bool inverted=false;
    public RectTransform uiRect;

    public Vector3 Position {
        get {
            return transform.localPosition;
        }
    }
    public int Elevation {
        get {
            return elevation;
        }
        set {
            elevation = value;
            Vector3 position = transform.localPosition;
            position.y = value * TriMetrics.elevationStep;
            //position.y +=(TriMetrics.SampleNoise(position).y * 2f - 1f) * TriMetrics.elevationPerturbStrength;
            transform.localPosition = position;

            Vector3 uiPosition = uiRect.localPosition;
            uiPosition.z = -position.y;
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
