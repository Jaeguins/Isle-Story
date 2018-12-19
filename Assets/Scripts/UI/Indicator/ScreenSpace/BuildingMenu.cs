using UnityEngine;
using System.Collections;

public class BuildingMenu : MonoBehaviour {
    public Entity selected;
    public void Bind(Entity target) {
        selected = target;
        gameObject.SetActive(true);
    }
    public void buildTent() {
        Selector.Instance.RequestLocation((Unit)selected, TriIsleland.Instance.innPrefabs[0].sizeType, new BuildCommand(TriIsleland.Instance.innPrefabs[0]));
        Hide();
    }
    public void Hide() {
        gameObject.SetActive(false);
    }
}
