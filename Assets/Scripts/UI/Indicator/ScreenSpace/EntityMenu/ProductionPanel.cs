using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ProductionPanel : EntityPanel {
    public new ProductionSelectable target;
    public int currentIndex = 0, tempIndex = 0;
    public List<Production> ProdList;
    public Image Icon;
    public Text Name, Description;
    public override void Bind(Entity entity) {
        base.Bind(entity);
        gameObject.SetActive(entity is ProductionSelectable);
        if (!(entity is ProductionSelectable)) return;
        target = entity as ProductionSelectable;
        ProdList = target.GetItems();
        if (ProdList.Count == 0) Debug.LogError("Production list is zero.");
        currentIndex = target.GetCurrent();
        tempIndex = currentIndex;
        ChangeView();
        
    }
    public override void Clear() {
        ProdList = null;
        Icon.sprite = null;
        Name.text = Strings.NaN;
        Description.text = Strings.NaN;
        base.Clear();
        gameObject.SetActive(false);
    }
    public void ChangeView() {
        tempIndex=Mathf.Clamp(tempIndex, 0, ProdList.Count-1);
        Icon.sprite = ResourceManager.Instance.itemResources[(int)ProdList[tempIndex].type].sprite;
        Name.text = ResourceManager.Instance.itemResources[(int)ProdList[tempIndex].type].Name + (currentIndex == tempIndex ? " - equiped" : "");
        Description.text = ResourceManager.Instance.itemResources[(int)ProdList[tempIndex].type].Desc;
        Debug.Log("list index changed");
    }
    public void ApplySelection() {
        currentIndex = tempIndex;
        target.SetProduction(currentIndex);
        ChangeView();
        Debug.Log("Selection applied");
    }
    public void CancelSelection() {
        target.SetProduction(-1);
        ChangeView();
        Debug.Log("Selection Canceled");
    }
    public void NextButton() {
        tempIndex += 1;
        ChangeView();
    }
    public void PrevButton() {
        tempIndex -= 1;
        ChangeView();
    }
}
