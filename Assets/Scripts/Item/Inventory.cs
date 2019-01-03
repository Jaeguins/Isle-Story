using UnityEngine;
using System.Collections;
using System.IO;

public class Inventory{
    public ItemSlot[] Slots;
    public bool refreshed;
    public int Size {
        get {
            return size;
        }
        set {
            if (resize(value))
                size = value;
            else
                Debug.LogWarning("item slot can't be resized");
        }
    }
    int size;
    public bool resize(int value) {
        ItemSlot[] prevSlot = Slots;
        Slots = new ItemSlot[value];
        int count = 0;
        for(int i = 0; i < size; i++) {
            if (prevSlot[i].Content) {
                Slots[count++] = prevSlot[i];
                if (count == Slots.Length) {
                    Slots = prevSlot;
                    return false;
                }
            }
        }
        for (; count < value; count++) {
            Slots[count] = new ItemSlot();
        }
        return true;
    }
    public Inventory(int size = 0) {
        this.size = size;
        Slots = new ItemSlot[size];
        for(int i=0;i<size;i++) {
            Slots[i] = new ItemSlot();
        }
        refreshed = true;
    }

    public void Save(BinaryWriter writer) {
        writer.Write(size);
        for(int i = 0; i < size; i++) {
            Slots[i].Save(writer);
        }
    }

    public static Inventory Load(BinaryReader reader) {
        int tSize = reader.ReadInt32();
        Inventory ret = new Inventory(tSize);
        for(int i = 0; i < tSize; i++) {
            ret.Slots[i].Load(reader);
        }
        return ret;
    }
}
