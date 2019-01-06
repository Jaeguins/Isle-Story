using UnityEngine;
using System.Collections.Generic;

public class ResourceManager : MonoBehaviour {
    public List<TotalResource> totalResources;
    public List<ItemResource> itemResources;
    public EntityManager manager;
    public void Start() {
        StartCoroutine(Routine());
    }
    IEnumerator<WaitForSeconds> Routine() {
        while (true) {
            Refresh();
            yield return new WaitForSeconds(1f);
        }
    }
    public void Refresh() {
        ResourceController target;
        foreach (Resource t in totalResources)
            t.Amount = 0;
        foreach (Resource t in itemResources)
            t.Amount = 0;
        foreach (KeyValuePair<int, Natural> pair in manager.naturals) {
            if (!pair.Value.Working) continue;
            target = pair.Value.resourceController;
            for (int i = 0; i < target.itemTypes.Count; i++) {
                itemResources[(int)target.itemTypes[i]].Amount += target.itemValues[i];
            }
            for (int i = 0; i < target.resourceTypes.Count; i++) {
                totalResources[(int)target.resourceTypes[i]].Amount += target.resourceValues[i];
            }
        }
        foreach (KeyValuePair<int, Building> pair in manager.buildings) {
            if (!pair.Value.Working) continue;
            target = pair.Value.resourceController;
            for (int i = 0; i < target.itemTypes.Count; i++) {
                itemResources[(int)target.itemTypes[i]].Amount += target.itemValues[i];
            }
            for (int i = 0; i < target.resourceTypes.Count; i++) {
                totalResources[(int)target.resourceTypes[i]].Amount += target.resourceValues[i];
            }
        }
        foreach (KeyValuePair<int, Unit> pair in manager.units) {
            target = pair.Value.resourceController;
            for (int i = 0; i < target.itemTypes.Count; i++) {
                itemResources[(int)target.itemTypes[i]].Amount += target.itemValues[i];
            }
            for (int i = 0; i < target.resourceTypes.Count; i++) {
                totalResources[(int)target.resourceTypes[i]].Amount += target.resourceValues[i];
            }
        }
        foreach (ItemResource t in itemResources) {
            target = t.totController;
            for (int i = 0; i < target.itemTypes.Count; i++) {
                itemResources[(int)target.itemTypes[i]].Amount += t.Amount*target.itemValues[i];
            }
            for (int i = 0; i < target.resourceTypes.Count; i++) {
                totalResources[(int)target.resourceTypes[i]].Amount += t.Amount*target.resourceValues[i];
            }
        }
    }
}
