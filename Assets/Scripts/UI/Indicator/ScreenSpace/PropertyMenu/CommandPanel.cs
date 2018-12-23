using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;

public class CommandPanel : MonoBehaviour{
    public static CommandPanel Instance;
    public CameraManager cameraManager;
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
    private void Awake() {
        Instance = this;
    }

    public void Bind(Entity entity) {
        enabled = true;
        nowEntity = entity;
        tooltip.text = nowEntity.UIName;
        nowEntity.BindOptions(this);
        Debug.Log("bind" + nowEntity);
    }

    public void Clear() {
        for(int i = 0; i < buttons.Count; i++) {
            buttons[i].onClick.RemoveAllListeners();
            buttons[i].gameObject.SetActive(false);
        }
    }

    public void Hide() {
        enabled = false;
        nowEntity = null;
        Clear();
        Debug.Log("unbind" + nowEntity);
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
        
    }

}
