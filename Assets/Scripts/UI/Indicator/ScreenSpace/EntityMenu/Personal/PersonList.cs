using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PersonList : MonoBehaviour {
    public GameObject panel;
    public Building NowBuilding;
    public List<Unit> NowList;
    public PersonIndicator IndicatorPrefab;
    public List<PersonIndicator> indicators=new List<PersonIndicator>();
    public Queue<PersonIndicator> indicatorPool=new Queue<PersonIndicator>();
    public void Bind(Building building,List<Unit> list) {
        NowBuilding = building;
        NowList = list;
        int i = 0;
        foreach(Person t in list) {
            AddIndicator(i++, t);
        }
        gameObject.SetActive(list.Count>0);
    }
    public void Close() {
        Clear();
        gameObject.SetActive(false);
    }
    public void Clear() {
        //TODO clearing NowBuilding if needed
        NowBuilding = null;
        NowList = null;
        foreach(PersonIndicator t in indicators) {
            t.Clear();
            indicatorPool.Enqueue(t);
        }
        indicators.Clear();
        
    }
    public void Refresh() {
        Building t = NowBuilding;
        List<Unit> tList = NowList;
        Clear();
        Bind(t, tList);
    }
    public void AddIndicator(int count,Person target) {
        PersonIndicator tRet = indicatorPool.Count > 0 ?
            indicatorPool.Dequeue() :
            Instantiate(IndicatorPrefab, panel.transform);
        tRet.Bind(target,count);
        tRet.gameObject.SetActive(true);
        indicators.Add(tRet);
    }
    private void LateUpdate() {
        if (NowBuilding&& NowList.Count!=indicators.Count) {
            Refresh();
        }
    }
}
