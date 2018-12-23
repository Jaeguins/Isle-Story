using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class PersonIndicator : MonoBehaviour {
    public Selector selector;
    public Text nameText, gender;
    public Person Target;
    public void PeekUnit() {
        CameraManager.Instance.GetNowActive().transform.localPosition = Target.transform.localPosition;
    }
    public void SelectUnit() {
        Property.Instance.Bind(Target);
        PersonList.Instance.Close();
    }
    public void Clear() {
        //TODO Clearing Target if needed
        Target = null;
        gameObject.SetActive(false);
    }
    public void Bind(Person target,int count) {
        Target = target;
        Vector3 pos = transform.localPosition;
        pos.y = 30 * count-30;
        transform.localPosition = pos;
    }
}
