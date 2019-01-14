using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ResourceDiff : MonoBehaviour {
    public Sprite Icon;
    public Text Name, Value,MaxValue;
    public int type, cat;
    public void Bind(ItemType type, int value,int maxValue) {
        cat = 1;
        BindInternal((int)type, value,maxValue);
    }
    public void Bind(ResourceType type,int value, int maxValue) {
        cat = 2;
        BindInternal((int)type, value, maxValue);
    }
    public void BindInternal(int type,int value, int maxValue) {
        KeyValuePair<Sprite, string> pair;
        if (cat == 1)
            pair = ResourceManager.GetItemAsset((ItemType)type);
        else
            pair = ResourceManager.GetTotalAsset((ResourceType)type);
        Icon = pair.Key;
        Name.text = pair.Value;
        Value.text = string.Format("{0:+#;-#;+0}", value);
        MaxValue.text= string.Format("{0:+#;-#;+0}", maxValue);
        gameObject.SetActive(true);
    }
    public void Clear() {
        Name.text = Strings.NaN;
        Value.text = Strings.NaN;
        MaxValue.text = Strings.NaN;
        gameObject.SetActive(false);
    }
}
