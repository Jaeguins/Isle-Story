using UnityEngine;
using System.Collections;

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
    public void BuildTent() {
        Selector.Instance.RequestLocation((Unit)selected, TriIsleland.Instance.innPrefabs[0].sizeType, new BuildCommand(TriIsleland.Instance.innPrefabs[0]));
        Hide();
    }
    public void Hide() {
        gameObject.SetActive(false);
        Debug.Log("buildingMenu unbind");
    }
}
