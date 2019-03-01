using UnityEngine;
using System.Collections;
public enum ProgressDirection {
    Horizontal, Vertical
}
public class Progressbar : MonoBehaviour {
    public RectTransform back, fore;
    public ProgressDirection direction;
    public float Value {
        get {
            return value;
        }
        set {
            RefreshUI();
            this.value = value;
        }
    }
    [Range(0, 1)]
    public float value;
    public void RefreshUI() {
        fore.anchorMax = new Vector2(
            direction == ProgressDirection.Horizontal ? value : 1,
            direction == ProgressDirection.Vertical ? value : 1
            );
    }
}
