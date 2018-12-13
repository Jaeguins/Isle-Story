using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;

public class EntityMenu : WorldSpaceCanvas{
    public Entity nowEntity;
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

    public void Bind(Entity entity) {
        nowEntity = entity;
        tooltip.text = nowEntity.UIName;
        Vector3 t=nowEntity.Location.transform.localPosition;
        t.y += 20;
        transform.localPosition = t;
        nowEntity.BindOptions(this);
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
        StartCoroutine(BuildingOption());
    }

    public void outBuildingOption() {
        StartCoroutine(OutBuildingOption());
    }

    public IEnumerator OutBuildingOption() {
        yield return StartCoroutine(cameraManager.SwitchCamera(CamType.TOPVIEW));
        enabled = false;
        ((IndivViewCam)cameraManager.cameras[1]).enabled = false;
    }

    public IEnumerator BuildingOption() {
        yield return StartCoroutine(cameraManager.SwitchCamera(CamType.BUILDINGVIEW));
        enabled = false;
        ((IndivViewCam)cameraManager.cameras[1]).SetAnchor(nowEntity);
        ((IndivViewCam)cameraManager.cameras[1]).SetOffset(nowEntity);
    }

    public void UnitStatus() {
        Selector.Instance.Deselect();
    }

    private void Update() {
        if (enabled)
            transform.rotation = cameraManager.GetNowActive().CameraView.transform.rotation;
        
    }

    public void switchBuildMenu() {
        GameUI.Instance.buildMenu.gameObject.SetActive(!GameUI.Instance.buildMenu.gameObject.activeInHierarchy);
    }
}
