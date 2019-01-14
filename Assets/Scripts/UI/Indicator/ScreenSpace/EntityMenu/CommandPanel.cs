using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CommandPanel : EntityPanel {
    public Button StatusButton;
    public Text StatusText;
    public void ViewTarget() {
        CameraManager.Instance.GetNowActive().AdjustPosition(target.Location);
    }
    public void SetActivateTarget(bool status) {
        if (status)
            StatusText.text = Strings.Deactivate;
        else
            StatusText.text = Strings.Activate;
        target.Working = status;
    }
    public override void Bind(Entity entity) {
        base.Bind(entity);
        StatusButton.gameObject.SetActive(target is Statics);
    }
}
