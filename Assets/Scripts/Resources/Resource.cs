using System;
using System.Collections;
using System.Linq;
using System.Text;
using UnityEngine;

public class Resource:MonoBehaviour{
    public Sprite sprite;
    public string Name=Strings.NaN, Desc = Strings.NaN;
    public int Amount;
    public int MaxAmount;
    public void Clear() {
        Amount = 0;
        MaxAmount = 0;
    }
}

