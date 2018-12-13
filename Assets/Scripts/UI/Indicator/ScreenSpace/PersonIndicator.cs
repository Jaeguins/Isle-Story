using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class PersonIndicator : MonoBehaviour {
    public Selector selector;
    public Text name, gender;
    public Button button;
    public Person target;
    public void SelectUnit() {
        selector.SelectUnit(target);
    }
}
