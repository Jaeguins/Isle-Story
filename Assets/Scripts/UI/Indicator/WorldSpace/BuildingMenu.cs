using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class BuildingMenu : WorldSpaceCanvas{
    public Building nowBuilding;
    public Text tooltip;
    public List<Button> buttons;
    public new bool enabled {
        get {
            return base.enabled;
        }
        set {
            base.enabled = value;
            Clear();
        }
    }
    private void Start() {
        canvas = GetComponent<Canvas>();
    }
    public void Bind(Building building) {
        nowBuilding = building;
        tooltip.text = nowBuilding.UIName;
        nowBuilding.BindOptions(this);
    }
    public void Clear() {
        for(int i = 0; i < buttons.Count; i++) {
            buttons[i].onClick.RemoveAllListeners();
            buttons[i].gameObject.SetActive(false);
        }
    }
    public void BindButton(int index,string tooltip,UnityEngine.Events.UnityAction action) {
        buttons[index].gameObject.SetActive(true);
        buttons[index].GetComponentInChildren<Text>().text = tooltip;
        buttons[index].onClick.AddListener(action);
    }
    public void ToBuildingOption() {
        camManager.SwitchCamera(CamType.BUILDINGVIEW);
        ((IndivViewCam)camManager.GetNowActive()).setAnchor(nowBuilding);
        ((IndivViewCam)camManager.GetNowActive()).SetOffset(nowBuilding);
    }
}
