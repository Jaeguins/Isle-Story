using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;

public class IslandLoader : MonoBehaviour {
    public string saveName;
    public GameObject islandPrefab;
    public Transform panel;
    public void OnEnable() {
        DirectoryInfo di = new DirectoryInfo(Application.persistentDataPath + "/save/" + saveName);
        foreach (DirectoryInfo tdi in di.GetDirectories()) {
            using (BinaryReader rd = new BinaryReader(File.OpenRead(Path.Combine(tdi.FullName, "thumbnail.dat")))) {
                Instantiate(islandPrefab, panel);

            }
        }
    }
    public void Clear() {

    }
}
