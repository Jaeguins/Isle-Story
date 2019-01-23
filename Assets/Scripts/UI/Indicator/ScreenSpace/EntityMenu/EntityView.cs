using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EntityView : MonoBehaviour {
    Entity target;
    public static EntityView Instance;
    public List<EntityPanel> panels;

    public void Awake() {
        Instance = this;
    }
    public void Start() {
        gameObject.SetActive(false);
    }
    public void Bind(Entity target) {
        Clear();
        this.target = target;
        foreach (EntityPanel t in panels) {
            t.Bind(target);
        }
        gameObject.SetActive(true);
        Debug.Log("<color=#f37321>" + target.ToString() + "</color> binded to entityMenu.");
    }
    public void Clear() {
        if (target)
            target.SelectionIndicator.Deselect();
        target = null;
        foreach (EntityPanel t in panels) {
            t.Clear();
        }
    }

    public void Close() {
        Clear();
        gameObject.SetActive(false);
    }
}
