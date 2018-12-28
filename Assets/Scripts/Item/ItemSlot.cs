using UnityEngine;
using System.Collections;

public class ItemSlot {
    public Item Content;
    public int Quantity;
    public ItemSlot() {
        Content = null;
        Quantity = 0;
    }

    public bool CheckItem(int targetId) {
        return targetId == Content.Id ? true : false;
    }

}
