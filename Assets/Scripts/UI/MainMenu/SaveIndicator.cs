using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
public class SaveIndicator :MonoBehaviour{
    public SaveList list;
    public GameObject HighlightObj;
    public Text SaveName;
    public int Index=-1;
    public void Bind(Save data,int index) {
        SaveName.text = data.Name;
        Index = index;
        gameObject.SetActive(true);
    }
    public void SetHighlight(bool val) {
        HighlightObj.SetActive(val);
    }
    public void Clear() {
        SaveName.text = Strings.NaN;
        Index = -1;
        HighlightObj.SetActive(false);
        gameObject.SetActive(false);
    }
    public void Select() {
        list.Select(Index);
        HighlightObj.SetActive(true);
    }
    public void Deselect() {
        HighlightObj.SetActive(false);
    }
}
