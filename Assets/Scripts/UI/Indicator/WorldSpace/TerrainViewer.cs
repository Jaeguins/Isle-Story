using UnityEngine;
using System.Collections;

public class TerrainViewer : TriMesh{
    public static TerrainViewer Instance;

    new void Awake() {
        base.Awake();
        Instance = this;
    }
    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {

    }
}
