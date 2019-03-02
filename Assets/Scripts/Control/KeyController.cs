using UnityEngine;
using System.Collections;

public class KeyController : MonoBehaviour {
    public static KeyController Instance;
    public TriIsland island;
    public void Awake() {
        Instance = this;
    }
    public void Start() {
        island = TriIsland.Instance;
    }
    public void Update() {
        if (Input.GetKeyDown(KeyMap.ShowSummary)) island.ShowSummary = !island.ShowSummary;
    }
}
public class KeyMap {
    public static KeyCode ShowSummary = KeyCode.Backslash
        ;
}
