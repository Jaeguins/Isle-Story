using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;

public class IslandLoader : MonoBehaviour {
    public string saveName;
    public IslandSelector IslandPrefab;
    public IslandSelector Focus;
    public List<IslandSelector> Islands=new List<IslandSelector>();
    public Queue<IslandSelector> IslandPool=new Queue<IslandSelector>();
    public Transform panel;
    public static IslandLoader Instance;
    public void Awake() {
        Instance = this;
    }
    public void Load() {
        DirectoryInfo di = new DirectoryInfo(Application.persistentDataPath + "/save/" + saveName);
        foreach (DirectoryInfo tdi in di.GetDirectories()) {
            using (BinaryReader rd = new BinaryReader(File.OpenRead(Path.Combine(tdi.FullName, "thumbnail.dat")))) {
                IslandSelector tisle=IslandPool.Count>0?IslandPool.Dequeue():Instantiate(IslandPrefab, panel);
                Vector3 pos = new Vector3(rd.ReadInt32(), rd.ReadInt32(), rd.ReadInt32());
                tisle.transform.localPosition = pos;
                tisle.IsleName = tdi.Name;
                tisle.IslePath = tdi.FullName;
                tisle.gameObject.SetActive(true);
                Islands.Add(tisle);
            }
        }
    }
    public void Clear() {
        foreach(IslandSelector sel in Islands) {
            sel.gameObject.SetActive(false);
            IslandPool.Enqueue(sel);
        }
        Islands.Clear();
    }
    public void LoadIsland() {
        TriIsland isle = TriIsland.Instance;
        isle.IslePath = Focus.IslePath;
        isle.Load();
    }
}
