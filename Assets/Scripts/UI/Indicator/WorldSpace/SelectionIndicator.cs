using UnityEngine;
using System.Collections;

public class SelectionIndicator : MonoBehaviour {
    public SpriteRenderer Renderer;
    public void Select() {
        gameObject.SetActive(true);
    }
    public void Deselect() {
        gameObject.SetActive(false);
    }
}
