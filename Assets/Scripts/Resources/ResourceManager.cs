using UnityEngine;
using System.Collections.Generic;

public class ResourceManager : MonoBehaviour {
    Dictionary<string, TotalResource> totalResources=new Dictionary<string, TotalResource>();
    Dictionary<string, ItemResource> itemResources = new Dictionary<string, ItemResource>();
    private void Awake() {
        totalResources.Add("food", new Food());
        totalResources.Add("energy", new Energy());
        totalResources.Add("material", new Material());
    }
    public TotalResource GetTotalResource(string key) {
        if (totalResources.ContainsKey(key))
            return totalResources[key];
        else return null;
    }
    public ItemResource GetItemResource(string key) {
        if (itemResources.ContainsKey(key))
            return itemResources[key];
        else return null;
    }
}
