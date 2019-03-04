using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public enum WorkType {
    FARMLAND, FELLINGLAND, TERRAINMODDER,MINE
}
public class Worksite : Building, ProductionSelectable,ISummary {
    public WorkType subType;
    public int Capacity;
    public override void Save(BinaryWriter writer) {
        base.Save(writer);
        writer.Write((int)subType);
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

    public Sprite GetProductSprite() {
        return CurrentProd==-1?null:ResourceManager.Instance.itemResources[(int)(productions[CurrentProd].type)].sprite;
    }

    public int GetTotalPeople() {
        return Capacity;
    }

    public int GetNowPeople() {
        return Workers.Count;
    }

    public int GetSparePeople() {
        return -1;
    }

    public float GetProdPercentage() {
        return 1f-NowConstructTime / ConstructTime;
    }

    public bool IsProducing() {
        return Working;
    }
}
