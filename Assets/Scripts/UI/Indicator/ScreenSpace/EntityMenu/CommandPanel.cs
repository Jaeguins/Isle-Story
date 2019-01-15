using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CommandPanel : EntityPanel {
    public Toggle StatusButton;
    public Text StatusText;
    public void ViewTarget() {
        CameraManager.Instance.GetNowActive().transform.localPosition = target.transform.localPosition;
        Debug.Log("seeing target");
    }
    public void SetActivateTarget(bool status) {
        if (!target) return;
        if (status)
            StatusText.text = Strings.Deactivate;
        else
            StatusText.text = Strings.Activate;
        target.Working = status;
        Debug.Log("target working set to "+status);
    }
    public override void Bind(Entity entity) {
        base.Bind(entity);
        StatusButton.gameObject.SetActive(target is Building);
        SetActivateTarget(target.Working);
        StatusButton.isOn = target.Working;
    }
}
