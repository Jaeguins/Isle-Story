using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public enum WorkType {
    FARMLAND, FELLINGLAND, TERRAINMODDER,MINE
}
public class Worksite : Building, ProductionSelectable {
    public int Capacity;
    public WorkType subType;
    public override void Save(BinaryWriter writer) {
        base.Save(writer);
        writer.Write((int)subType);
        writer.Write(Capacity);
        writer.Write(CurrentProd);
    }
    public new static Worksite Load(BinaryReader reader) {
        WorkType subType = (WorkType)reader.ReadInt32();
        int capacity = reader.ReadInt32(),currentProd=reader.ReadInt32();
        Worksite ret = null;
        switch (subType) {
            case WorkType.FARMLAND:
                ret = Farmland.Load(reader);
                break;
        }
        ret.subType = subType;
        ret.Capacity = capacity;
        ret.CurrentProd=currentProd;
        return ret;
    }
    public void ShowWorkers() {
        personList.Bind(this, Workers);
    }



    public List<Production> productions;
    public List<GameObject> productModels;
    public int currentProd = -1;
    public int CurrentProd {
        get {
            return currentProd;
        }
        set {
            currentProd = value;
            SetProduction(value);
        }
    }
    public List<Production> GetItems() {
        return productions;
    }

    public void SetProduction(int target) {
        currentProd = target;
        for (int i = 0; i < productModels.Count; i++) {
            if(productModels[i]!=null)productModels[i].SetActive(i == target);
        }
        resourceController.productions.Clear();
        for (int i = 0; i < productions.Count; i++) {
            if (i == target) resourceController.productions.Add(productions[i]);
        }
    }

    public int GetCurrent() {
        return CurrentProd;
    }
}
