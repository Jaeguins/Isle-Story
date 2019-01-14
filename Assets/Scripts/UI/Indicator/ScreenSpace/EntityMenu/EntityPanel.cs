using UnityEngine;
using System.Collections;

public class EntityPanel : MonoBehaviour {
    public Entity target;
    public virtual void Clear() {

    }
    public void ReInit() {
        Entity entity = target;
        Clear();
        Bind(entity);
    }
    public virtual void Bind(Entity entity) {
        target = entity;
    }
}
