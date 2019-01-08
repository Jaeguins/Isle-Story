using UnityEngine;
using System.Collections.Generic;

public class ResourceManager : MonoBehaviour {
    public List<TotalResource> totalResources;
    public List<ItemResource> itemResources;
    public EntityManager manager;
    public ResourceView resourceView;
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
            t.Clear();
        foreach (Resource t in itemResources)
            t.Clear();
        foreach (KeyValuePair<int, Natural> pair in manager.naturals) {
            if (!pair.Value.Working) continue;
            target = pair.Value.resourceController;
            for (int i = 0; i < target.itemTypes.Count; i++) {
                itemResources[(int)target.itemTypes[i]].Amount += target.itemValues[i];
                itemResources[(int)target.itemTypes[i]].MaxAmount += target.itemMaxValues[i];
            }
            for (int i = 0; i < target.resourceTypes.Count; i++) {
                totalResources[(int)target.resourceTypes[i]].Amount += target.resourceValues[i];
                totalResources[(int)target.resourceTypes[i]].MaxAmount += target.resourceMaxValues[i];
            }
        }
        foreach (KeyValuePair<int, Building> pair in manager.buildings) {
            if (!pair.Value.Working) continue;
            target = pair.Value.resourceController;
            for (int i = 0; i < target.itemTypes.Count; i++) {
                itemResources[(int)target.itemTypes[i]].Amount += target.itemValues[i];
                itemResources[(int)target.itemTypes[i]].MaxAmount += target.itemMaxValues[i];
            }
            for (int i = 0; i < target.resourceTypes.Count; i++) {
                totalResources[(int)target.resourceTypes[i]].Amount += target.resourceValues[i];
                totalResources[(int)target.resourceTypes[i]].MaxAmount += target.resourceMaxValues[i];
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
            if (t.Amount > t.MaxAmount) t.Amount = t.MaxAmount;
            for (int i = 0; i < target.itemTypes.Count; i++) {
                itemResources[(int)target.itemTypes[i]].Amount += t.Amount*target.itemValues[i];
                //itemResources[(int)target.itemTypes[i]].MaxAmount += t.Amount*target.itemMaxValues[i];
            }
            for (int i = 0; i < target.resourceTypes.Count; i++) {
                totalResources[(int)target.resourceTypes[i]].Amount += t.Amount*target.resourceValues[i];
                //totalResources[(int)target.resourceTypes[i]].MaxAmount += t.Amount*target.resourceMaxValues[i];
            }
        }
        foreach(TotalResource t in totalResources) {
            if (t.Amount > t.MaxAmount) t.Amount = t.MaxAmount;
        }
        resourceView.refreshed=true;
    }
}
