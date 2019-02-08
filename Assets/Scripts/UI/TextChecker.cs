using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextChecker : MonoBehaviour
{
    public List<string> CheckTargets = new List<string>();
    public Selectable Limitator;
    public Text Indicator;
    public InputField target;
    public void Refresh(List<string> targetList) {
        CheckTargets.Clear();
        CheckTargets.AddRange(targetList);
    }
    public bool IsDuplicate(string text) {
        foreach(string t in CheckTargets) {
            if (t == text) return true;
        }
        return false;
    }
    public void Check() {
        if (IsDuplicate(target.text)) {
            Limitator.interactable = false;
            Indicator.text = Strings.DuplicateCheckNotOK;
        }
        else {
            Limitator.interactable = true;
            Indicator.text = Strings.DuplicateCheckOK;
        }
    }
}
