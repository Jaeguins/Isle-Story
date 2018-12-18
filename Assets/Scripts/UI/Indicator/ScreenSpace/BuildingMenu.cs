using UnityEngine;
using System.Collections;

public class BuildingMenu : MonoBehaviour {
    public Entity selected;
    public void Bind(Entity target) {
        selected = target;
        gameObject.SetActive(true);
    }
    public void buildTent() {
        Selector selector = Selector.Instance;
        selector.commandType = CommandType.BUILD;
        selector.sizeType = SizeType.SINGLE;
        selector.ordering = true;
        selector.target = TriIsleland.Instance.innPrefabs[0];
        Hide();
    }
    public void Hide() {
        gameObject.SetActive(false);
    }
}
