using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ResourceView : MonoBehaviour {
    public Text foodNow, foodTot, enNow, enTot, matNow, matTot;
    public ResourceManager manager;
    public bool refreshed = true;
    private void Update() {
        if (refreshed) Refresh();
    }
    public void Refresh() {
        foodNow.text = manager.totalResources[0].Amount.ToString();
        foodTot.text = manager.totalResources[0].MaxAmount.ToString();
        enNow.text = manager.totalResources[1].Amount.ToString();
        enTot.text = manager.totalResources[1].MaxAmount.ToString();
        matNow.text = manager.totalResources[2].Amount.ToString();
        matTot.text = manager.totalResources[2].MaxAmount.ToString();
    }
}
