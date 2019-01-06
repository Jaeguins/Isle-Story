using System;
using System.Collections;
using System.Linq;
using System.Text;
using UnityEngine;

public class Resource:MonoBehaviour{
    public bool Refreshed = true;
    public int Amount {
        get {
            return amount;
        }
        set {
            amount = value;
        }
    }
    int amount;
    private void Start() {
        StartCoroutine(coroutine());
    }
    public virtual void Refresh() {

    }
    public IEnumerator coroutine() {
        while (true) {
            if (Refreshed) {
                Refresh();
                Refreshed = false;
            }
            yield return new WaitForSeconds(0.1f);
        }
    }
}

