using UnityEngine;
using System.Collections;
using System.IO;
public class ItemSlot {
    public Item Content;
    public int Quantity;
    public bool refreshed = false;
    public ItemSlot() {
        Clear();
    }

    public bool CheckItem(int targetId) {
        return targetId == Content.Id ? true : false;
    }
    public bool CheckFlag(int flag) {
        return ((flag & (int)Content.flag) == flag) ? true : false;
    }
    public void Save(BinaryWriter writer) {
        if (Quantity != 0) {
            writer.Write(Content.Id);
        }
        writer.Write(Quantity);
    }
    public void Clear() {
        Content = null;
        Quantity = 0;
        refreshed = true;
    }
    public bool Add(ItemSlot target) {
        if (!Content) {
            Content = target.Content;
            Quantity = target.Quantity;
            refreshed = true;
            return true;
        }
        if (!CheckItem(target.Content.Id))
            return false;
        if (Content.stackSize < Quantity + target.Quantity) {
            Quantity = Content.stackSize;
            target.Quantity += (Quantity - Content.stackSize);
            refreshed = true;
            return false;
        }
        Quantity += target.Quantity;
        refreshed = true;
        return true;
    }

    public bool Request(ItemSlot target) {
        if (!Content) return false;
        if (!CheckItem(target.Content.Id)) return false;
        if (Quantity < target.Quantity) {
            target.Quantity -= Quantity;
            refreshed = true;
            return false;
        }
        Quantity -= target.Quantity;
        refreshed = true;
        if (Quantity == 0) Clear();
        return true;
    }

    public void Load(BinaryReader reader) {
        if (Quantity != 0) {
            Content = new Item(reader.ReadInt32());
        }
        Quantity = reader.ReadInt32();
        refreshed = true;
    }
}
