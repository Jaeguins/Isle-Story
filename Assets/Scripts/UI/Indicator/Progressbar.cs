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
        fore.localScale = new Vector3(
            back.localScale.x * (direction == ProgressDirection.Horizontal ? value : 1),
            back.localScale.y * (direction == ProgressDirection.Vertical ? value : 1),
            back.localScale.z);
    }
}
