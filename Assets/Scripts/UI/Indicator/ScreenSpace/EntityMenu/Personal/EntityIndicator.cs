using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class EntityIndicator : MonoBehaviour {
    public Selector selector;
    public Text nameText, gender;
    public Button SelectButton;
    public Entity Target;
    public void PeekUnit() {
        if (Target)
        CameraManager.Instance.GetNowActive().transform.localPosition = Target.transform.localPosition;
    }
    public void SelectUnit() {
        if(Target)
        EntityView.Instance.Bind(Target);
    }
    public void Clear() {
        //TODO Clearing Target if needed
        Target = null;
        gameObject.SetActive(false);
    }
    public void Bind(Entity target) {
        Target = target;
        if (target)
            nameText.text = target.UIName + target.ID;
        else
            nameText.text = Strings.None;
        SelectButton.gameObject.SetActive(target);
    }
}
