﻿using UnityEngine;
using System.Collections;

public class Inventory: MonoBehaviour {
    public ItemSlot[] Slots;
    public bool refreshed;
    public int size;
    public Inventory(int size) {
        this.size = size;
        Slots = new ItemSlot[size];
        for(int i=0;i<size;i++) {
            Slots[i] = new ItemSlot();
        }
        
    }
}
