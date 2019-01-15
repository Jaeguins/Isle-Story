using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ProductionPanel : EntityPanel {
    public new ProductionSelectable target;
    public int currentIndex = 0, tempIndex = 0;
    public List<ItemResource> ProdList;
    public Image Icon;
    public Text Name, Description;
    public override void Bind(Entity entity) {
        base.Bind(entity);
        if (!(entity is ProductionSelectable)) return;
        target = entity as ProductionSelectable;
        ProdList = target.GetItems();
        if (ProdList.Count == 0) Debug.LogError("Production list is zero.");
        currentIndex = target.GetCurrent();
        tempIndex = currentIndex;
        ChangeView();
        gameObject.SetActive(true);
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
        Icon.sprite = ProdList[tempIndex].sprite;
        Name.text = ProdList[tempIndex].Name + (currentIndex == tempIndex ? " - equiped" : "");
        Description.text = ProdList[tempIndex].Desc;
        Debug.Log("list index changed");
    }
    public void ApplySelection() {
        currentIndex = tempIndex;
        target.SetProduction(currentIndex);
        ChangeView();
        Debug.Log("Selection applied");
    }
    public void CancelSelection() {
        tempIndex = currentIndex;
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
