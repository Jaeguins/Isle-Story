using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
public struct Save {
    public string Name;
    public string Path;
}
public class SaveList : MonoBehaviour {
    public Transform panel;
    int selected = -1;
    public List<Save> NowList = new List<Save>();
    public SaveIndicator IndicatorPrefab;
    public List<SaveIndicator> indicators = new List<SaveIndicator>();
    public Queue<SaveIndicator> indicatorPool = new Queue<SaveIndicator>();
    public void OnEnable() {
        NowList.Clear();
        DirectoryInfo di = new DirectoryInfo(Application.persistentDataPath + "/save");
        if (di.Exists) {
            NowList.Clear();
            foreach (DirectoryInfo dir in di.GetDirectories()) {
                Save tmp = new Save {
                    Name = dir.Name,
                    Path = dir.FullName
                };
                NowList.Add(tmp);
                SaveIndicator tmpIndicator =
                    indicatorPool.Count > 0 ?
                    indicatorPool.Dequeue() :
                    Instantiate(IndicatorPrefab, panel.transform);
                tmpIndicator.Bind(tmp, indicators.Count);
                tmpIndicator.list = this;
                indicators.Add(tmpIndicator);
            }
        }
    }
    public void OnDisable() {
        foreach (SaveIndicator ind in indicators) {
            ind.Clear();
            indicatorPool.Enqueue(ind);
        }
        indicators.Clear();
        NowList.Clear();
    }
    public void Select(int i) {
        foreach(SaveIndicator ind in indicators) {
            ind.Deselect();
        }
        
        selected = i;
    }
    public void DeleteSave() {
        if (selected == -1) return;
        new DirectoryInfo(NowList[selected].Path).Delete(true);
        OnDisable();
        OnEnable();
        selected = -1;
    }

    public void Load() {

    }
}
