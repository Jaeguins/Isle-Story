using UnityEngine;
using System.Collections;
using System;

public class BuildingMenu : MonoBehaviour {
    public Entity selected;
    public static BuildingMenu Instance;
    public void Bind(Entity target) {
        selected = target;
        gameObject.SetActive(true);
        //PersonList.Instance.Hide();
        Debug.Log("buildingMenu bind");
    }
    private void Awake() {
        Instance = this;
        gameObject.SetActive(false);
    }
    public void BuildBuilding(int type,int subtype,int index) {
        Entity t = TriIsland.GetBuildingPrefabs(type, subtype, index);
        Selector.Instance.RequestLocation((Unit)selected, t.sizeType, new BuildCommand(t));
        Close();
    }
    public void BuildTent() {
        BuildBuilding((int)BuildingType.INN, (int)InnType.TENT, 0);
    }
    public void BuildFarmland() {
        BuildBuilding((int)BuildingType.WORKSITE, (int)WorkType.FARMLAND, 0);
    }
    public void Close() {
        if (gameObject.activeInHierarchy) {
            gameObject.SetActive(false);
            Debug.Log("buildingMenu unbind");
        }
    }
}
