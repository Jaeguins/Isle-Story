using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitPersonalPanel : EntityPanel {
    public BuildingList Work, Company, Home;
    public void LateUpdate() {
        if (!target) return;
        Work.Refresh();
        Company.Refresh();
        Home.Refresh();
    }
    public override void Bind(Entity entity) {
        base.Bind(entity);
        gameObject.SetActive(entity is Human);
        if (entity is Human) {
            Human t = entity as Human;
            List<Statics> workList = new List<Statics>();
            List<Statics> compList= new List<Statics>();
            List<Statics> homeList = new List<Statics>();
            workList.Add(t.Work);
            compList.Add(t.Company);
            homeList.Add(t.Home);
            Work.Bind(t, workList);
            Company.Bind(t, compList);
            Home.Bind(t, homeList);
        }
    }
    public override void Clear() {
        base.Clear();
        Work.Clear();
        Home.Clear();
        Company.Clear();
    }
}
