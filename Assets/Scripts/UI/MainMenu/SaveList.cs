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
    public MainMenu mainMenu;
    public SaveIndicator IndicatorPrefab;
    public List<SaveIndicator> indicators = new List<SaveIndicator>();
    public Queue<SaveIndicator> indicatorPool = new Queue<SaveIndicator>();
    public void OnEnable() {
        mainMenu.RefreshSaveList();
        foreach (Save t in mainMenu.NowList) {
            SaveIndicator tmpIndicator =
                    indicatorPool.Count > 0 ?
                    indicatorPool.Dequeue() :
                    Instantiate(IndicatorPrefab, panel.transform);
            tmpIndicator.Bind(t, indicators.Count);
            tmpIndicator.list = this;
            indicators.Add(tmpIndicator);
        }
    }
    public void OnDisable() {
        foreach (SaveIndicator ind in indicators) {
            ind.Clear();
            indicatorPool.Enqueue(ind);
        }
        indicators.Clear();
    }
    public void Select(int i) {
        foreach(SaveIndicator ind in indicators) {
            ind.Deselect();
        }
        selected = i;
        mainMenu.LoadSaveName = indicators[selected].SaveName.text;
    }
    public void DeleteSave() {
        if (selected == -1) return;
        new DirectoryInfo(mainMenu.NowList[selected].Path).Delete(true);
        mainMenu.RefreshSaveList();
        OnDisable();
        OnEnable();
        selected = -1;
    }

    public void Load() {
        mainMenu.LoadGame();
    }
}
