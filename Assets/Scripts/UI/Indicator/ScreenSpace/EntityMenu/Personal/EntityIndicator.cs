using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class EntityIndicator : MonoBehaviour {
    public Selector selector;
    public Text nameText, gender;
    public Entity Target;
    public void PeekUnit() {
        CameraManager.Instance.GetNowActive().transform.localPosition = Target.transform.localPosition;
    }
    public void SelectUnit() {
        EntityView.Instance.Bind(Target);
    }
    public void Clear() {
        //TODO Clearing Target if needed
        Target = null;
        gameObject.SetActive(false);
    }
    public void Bind(Entity target) {
        Target = target;
        nameText.text = target.UIName+target.ID;
    }
}
