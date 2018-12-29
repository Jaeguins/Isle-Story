using UnityEngine;
using System.Collections;

public class ItemSlot {
    public Item Content;
    public int Quantity;
    public static implicit operator bool(ItemSlot value) {
        return value != null ? true : false;
    }
    public ItemSlot() {
        Content = null;
        Quantity = 0;
    }

    public bool CheckItem(int targetId) {
        return targetId == Content.Id ? true : false;
    }

}
