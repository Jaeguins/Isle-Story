using System.Collections;
using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IslandLoader : MonoBehaviour {
    public IslandSelector IslandPrefab;
    public IslandSelector Focus;
    public List<IslandSelector> Islands = new List<IslandSelector>();
    public Queue<IslandSelector> IslandPool = new Queue<IslandSelector>();
    public Transform panel;
    public static IslandLoader Instance;
    public DirectoryInfo di;
    public GameObject NewIslandPanel;
    public Button CancelNewIsleButton;
    public InputField NewIslandName;
    public int nextLocationX=0, nextLocationZ=0;
    public void Awake() {
        Instance = this;
    }
    public void OnEnable() {
        di = new DirectoryInfo(Application.persistentDataPath + "/save/" + TriIsland.Instance.SaveName);
        if (TriIsland.Instance.SaveName != "NaN") {
            if (di.Exists) {
                RefreshList();
            }
            else {
                di.Create();
                NewGameStart();
            }
        }
    }
    public void OnDisable() {
        Clear();
    }
    public void NewGameStart() {
        NewIslandPanel.SetActive(true);
        CancelNewIsleButton.interactable = false;
    }
    public void NewIsleGenerate() {
        TriIsland.Instance.IsleName = NewIslandName.text;
        TriIsland.Instance.NewMap(0,0);
        CancelNewIsleButton.interactable = true;
        NewIslandPanel.SetActive(false);
    }
    public void RefreshList() {
        
        foreach (DirectoryInfo tdi in di.GetDirectories()) {
            string p = Path.Combine(tdi.FullName, "thumbnail.dat");
            if (!File.Exists(p)) continue;
            IslandSelector tisle = IslandPool.Count > 0 ? IslandPool.Dequeue() : Instantiate(IslandPrefab, panel);
            using (BinaryReader rd = new BinaryReader(File.OpenRead(p))) {
                Vector3 pos = new Vector3(rd.ReadInt32()*120, rd.ReadInt32()*120, 0);
                tisle.transform.localPosition = pos;
                tisle.IsleName = tdi.Name;
                tisle.IslePath = tdi.FullName;
                tisle.gameObject.SetActive(true);
                Islands.Add(tisle);
            }
            Texture2D img = new Texture2D(120, 120);
            img.LoadImage(File.ReadAllBytes(Path.Combine(tdi.FullName, "thumbnailImg.png")));
            img.filterMode = FilterMode.Point;
            tisle.Image.texture = img;
            tisle.btn.onClick.AddListener(() => ClickIsle(tisle));
        }
    }
    public void ClickIsle(IslandSelector selected) {
        Focus = selected;
        LoadIsland();
    }
    public void Clear() {
        foreach (IslandSelector sel in Islands) {
            sel.gameObject.SetActive(false);
            IslandPool.Enqueue(sel);
        }
        Islands.Clear();
    }
    public void LoadIsland() {
        TriIsland isle = TriIsland.Instance;
        isle.Load(Focus.IslePath);
    }
}
