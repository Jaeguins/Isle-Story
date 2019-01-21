using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EntityList<T> : MonoBehaviour  where T : Entity{
    public GameObject panel;
    public Entity NowEntity;
    public List<T> NowList;
    public EntityIndicator IndicatorPrefab;
    public List<EntityIndicator> indicators=new List<EntityIndicator>();
    public Queue<EntityIndicator> indicatorPool=new Queue<EntityIndicator>();
    public void Bind(Entity target,List<T> list) {
        NowEntity = target;
        NowList = list;
        int i = 0;
        foreach(T t in list) {
            AddIndicator(t);
        }
        gameObject.SetActive(list.Count>0);
    }
    public void Close() {
        Clear();
        gameObject.SetActive(false);
    }
    public void Clear() {
        //TODO clearing NowBuilding if needed
        NowEntity = null;
        NowList = null;
        foreach(EntityIndicator t in indicators) {
            t.Clear();
            indicatorPool.Enqueue(t);
        }
        indicators.Clear();
        
    }
    public void Refresh() {
        Entity t = NowEntity;
        List<T> tList = NowList;
        Clear();
        Bind(t, tList);
    }
    public void AddIndicator(T target) {
        EntityIndicator tRet = indicatorPool.Count > 0 ?
            indicatorPool.Dequeue() :
            Instantiate(IndicatorPrefab, panel.transform);
        tRet.Bind(target);
        tRet.gameObject.SetActive(true);
        indicators.Add(tRet);
    }
    private void LateUpdate() {
        if (NowEntity&& NowList.Count!=indicators.Count) {
            Refresh();
        }
    }
}
