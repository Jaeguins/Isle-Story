using UnityEngine;
using System.Collections;

public class Construction : Building{
    public Building target;
    public void SetTarget(Building target) {
        this.target = target;

    }
}
