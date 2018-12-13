using UnityEngine;
using System.Collections;

public class BuildingMenu : MonoBehaviour {
    public void buildTent() {
        Selector selector = Selector.Instance;
        selector.commandType = CommandType.BUILD;
        selector.sizeType = SizeType.SINGLE;
        selector.ordering = true;
        selector.target = TriIsleland.Instance.innPrefabs[0];
    }
}
