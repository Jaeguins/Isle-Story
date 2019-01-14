using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EntityView : MonoBehaviour
{
    Entity target;
    public static EntityView Instance;
    public List<EntityPanel> panels;
    
    public void Awake() {
        Instance = this;
    }
    public void Bind(Entity target) {
        this.target = target;
        foreach (EntityPanel t in panels) {
            t.Bind(target);
        }
        gameObject.SetActive(true);
        Debug.Log("<color=orange>" + target.ToString() + "</color> binded to entityMenu.");
    }
    public void Clear() {
        foreach (EntityPanel t in panels) {
            t.Clear();
        }

    }
    
    public void Close() {
        Clear();
        gameObject.SetActive(false);
    }
}
