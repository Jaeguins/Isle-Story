using UnityEngine;
using System.Collections.Generic;

public class ResourceManager : MonoBehaviour {
    public List<TotalResource> totalResources;
    public List<ItemResource> itemResources;
    public EntityManager manager;
    public ResourceView resourceView;
    public static ResourceManager Instance;

    public static TotalResource GetTotalAsset(ResourceType type) {
        return Instance.totalResources[(int)type];
    }
    public static ItemResource GetItemAsset(ItemType type) {
        return Instance.itemResources[(int)type];
    }

    public void Awake() {
        Instance = this;
    }

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
            for (int i = 0; i < target.productions.Count; i++) {
                itemResources[(int)target.productions[i].type].Amount += target.productions[i].prod;
                itemResources[(int)target.productions[i].type].MaxAmount += target.productions[i].max;
            }
            for (int i = 0; i < target.totals.Count; i++) {
                totalResources[(int)target.totals[i].type].Amount += target.totals[i].prod;
                totalResources[(int)target.totals[i].type].MaxAmount += target.totals[i].max;
            }
        }
        foreach (KeyValuePair<int, Building> pair in manager.buildings) {
            if (!pair.Value.Working) continue;
            target = pair.Value.resourceController;
            for (int i = 0; i < target.productions.Count; i++) {
                itemResources[(int)target.productions[i].type].Amount += target.productions[i].prod;
                itemResources[(int)target.productions[i].type].MaxAmount += target.productions[i].max;
            }
            for (int i = 0; i < target.totals.Count; i++) {
                totalResources[(int)target.totals[i].type].Amount += target.totals[i].prod;
                totalResources[(int)target.totals[i].type].MaxAmount += target.totals[i].max;
            }
        }
        foreach (KeyValuePair<int, Unit> pair in manager.units) {
            target = pair.Value.resourceController;
            for (int i = 0; i < target.productions.Count; i++) {
                itemResources[(int)target.productions[i].type].Amount += target.productions[i].prod;
            }
            for (int i = 0; i < target.totals.Count; i++) {
                totalResources[(int)target.totals[i].type].Amount += target.totals[i].prod;
            }
        }
        foreach (ItemResource t in itemResources) {
            target = t.totController;
            if (t.Amount > t.MaxAmount) t.Amount = t.MaxAmount;
            for (int i = 0; i < target.productions.Count; i++) {
                itemResources[(int)target.productions[i].type].Amount += t.Amount*target.productions[i].prod;
                //itemResources[(int)target.productions[i]].MaxAmount += t.Amount*target.productions[i];
            }
            for (int i = 0; i < target.totals.Count; i++) {
                totalResources[(int)target.totals[i].type].Amount += t.Amount*target.totals[i].prod;
                //totalResources[(int)target.totals[i]].MaxAmount += t.Amount*target.totals[i];
            }
        }
        foreach(TotalResource t in totalResources) {
            if (t.Amount > t.MaxAmount) t.Amount = t.MaxAmount;
        }
        resourceView.refreshed=true;
    }
}
