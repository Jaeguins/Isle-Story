using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PrefabManager{
    public Prefabs this[int index] {
        get {
            return lists[index];
        }
        set {
            lists[index] = value;
        }
    }
    public List<Prefabs> lists=new List<Prefabs>();
}
[Serializable]
public class Prefabs {
    public Entity this[int index] {
        get {
            return lists[index];
        }
        set {
            lists[index] = value;
        }
    }
    public List<Entity> lists=new List<Entity>();
}
