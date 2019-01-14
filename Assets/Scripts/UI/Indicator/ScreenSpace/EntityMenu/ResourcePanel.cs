using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ResourcePanel : EntityPanel {
    public ResourceDiff prefab;
    public List<ResourceDiff> list;
    public Queue<ResourceDiff> pool;
    public override void Clear() {
        foreach(ResourceDiff t in list) {
            t.Clear();
            pool.Enqueue(t);
        }
        list.Clear();
        base.Clear();
    }
    public override void Bind(Entity entity) {
        base.Bind(entity);
        ResourceDiff ret;
        ResourceController controller = target.resourceController;
        for(int i = 0; i < controller.resourceTypes.Count;i++) {
             ret= pool.Count > 0 ? pool.Dequeue() : Instantiate(prefab, transform);
            ret.Bind(controller.resourceTypes[i], controller.resourceValues[i], controller.resourceMaxValues[i]);
            list.Add(ret);
        }
        for (int i = 0; i < controller.itemTypes.Count; i++) {
            ret = pool.Count > 0 ? pool.Dequeue() : Instantiate(prefab, transform);
            ret.Bind(controller.itemTypes[i], controller.itemValues[i], controller.itemMaxValues[i]);
            list.Add(ret);
        }
    }
}
