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
        EntityMenu.Instance.BindUnit(Target);
        PersonList.Instance.Close();
    }
    public void Clear() {
        //TODO Clearing Target if needed
        Target = null;
        gameObject.SetActive(false);
    }
    public void Bind(Person target,int count) {
        Target = target;
        nameText.text = target.UIName+target.ID;
        RectTransform rect = gameObject.GetComponent<RectTransform>();
        rect.anchoredPosition = new Vector3(0, -30 * count - 30, 0);
        
        
    }
}
