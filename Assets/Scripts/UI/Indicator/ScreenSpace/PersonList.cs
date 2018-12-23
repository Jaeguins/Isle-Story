using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PersonList : MonoBehaviour {
    public static PersonList Instance;
    public Building NowBuilding;
    public PersonIndicator IndicatorPrefab;
    public List<PersonIndicator> indicators;
    public Queue<PersonIndicator> indicatorPool;
    private void Awake() {
        Instance = this;
        indicatorPool = new Queue<PersonIndicator>();
        indicators= new List<PersonIndicator>();
        gameObject.SetActive(false);
    }
    public void Bind(Building building,List<Person> list) {
        NowBuilding = building;
        int i = 0;
        foreach(Person t in list) {
            AddIndicator(i++, t);
        }
        gameObject.SetActive(true);
    }
    public void Close() {
        Clear();
        gameObject.SetActive(false);
    }
    public void Clear() {
        //TODO clearing NowBuilding if needed
        NowBuilding = null;
        foreach(PersonIndicator t in indicators) {
            t.Clear();
            indicatorPool.Enqueue(t);
        }
        indicators.Clear();
    }
    public void AddIndicator(int count,Person target) {
        PersonIndicator tRet = indicatorPool.Count > 0 ?
            indicatorPool.Dequeue() :
            Instantiate(IndicatorPrefab, transform);
        tRet.Bind(target,count);
        tRet.gameObject.SetActive(true);
        indicators.Add(tRet);
    }
}
