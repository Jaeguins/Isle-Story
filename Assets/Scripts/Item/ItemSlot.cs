using UnityEngine;
using System.Collections;
using System.IO;
public class ItemSlot {
    public Item Content;
    public int Quantity;

    public ItemSlot() {
        Clear();
    }

    public bool CheckItem(int targetId) {
        return targetId == Content.Id ? true : false;
    }
    public void Save(BinaryWriter writer) {
        writer.Write(Quantity);
        if (Quantity != 0) {
            writer.Write(Content.Id);
        }
    }
    public void Clear() {
        Content = null;
        Quantity = 0;
    }
    public int ChangeQuantity(int value) {
        if (!Content) return -1;
        if (Quantity + value < Content.stackSize) {
            Quantity += value;
            return 0;
        }
        else {
            value -= Content.stackSize - Quantity;
            Quantity = Content.stackSize;
            return value;
        }
    }
    public bool Add(ItemSlot target) {
        if (!Content) {
            Content = target.Content;
            return true;
        }
        if (!CheckItem(target.Content.Id))
            return false;
        if (Content.stackSize < Quantity + target.Quantity) {
            Quantity = Content.stackSize;
            target.Quantity += (Quantity - Content.stackSize);
            return false;
        }
        Quantity += target.Quantity;
        return true;
    }

    public ItemSlot Divide(ItemSlot target, int quantity) {
        ItemSlot ret = new ItemSlot();
        if (Quantity <= quantity) {
            ret.Content = Content;
            ret.Quantity = Quantity;
            Clear();
        }
        else {

            ret.Content = Content;
            ret.Quantity = quantity;
            Quantity -= quantity;
            return ret;
        }
        return ret;
    }

    public void Load(BinaryReader reader) {
        Quantity = reader.ReadInt32();
        if (Quantity != 0) {
            Content = new Item(reader.ReadInt32());
        }
    }
}
