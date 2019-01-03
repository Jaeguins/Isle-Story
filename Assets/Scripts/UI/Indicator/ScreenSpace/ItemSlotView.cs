using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ItemSlotView : MonoBehaviour {
    public static int ColSize = 5;
    public ItemSlot Target;
    public Image icon;
    public Text numIndicator;
    

    public void Bind(ItemSlot target,int count) {
        Target = target;
        if (Target.Content) {
            numIndicator.text = Target.Quantity.ToString();
            icon.sprite = InventoryViewer.Instance.Sprites[Target.Content.Id];
        }
        else {
            numIndicator.text = "";
        }
        RectTransform rect = gameObject.GetComponent<RectTransform>();
        rect.anchoredPosition = new Vector3(count%ColSize*40+20,count/ColSize*-40-20, 0);
    }
}
