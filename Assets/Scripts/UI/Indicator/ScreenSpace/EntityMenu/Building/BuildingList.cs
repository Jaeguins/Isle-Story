using UnityEngine;
using System.Collections;

public class BuildingList : EntityList<Statics> {
    public BuildingType flag;
    public void Change() {
        switch (flag) {
            case BuildingType.COMPANY:
                (NowEntity as Human).ChangeJob();
                break;
            case BuildingType.WORKSITE:
                (NowEntity as Human).ChangeWork();
                break;
            case BuildingType.INN:
                (NowEntity as Human).ChangeHome();
                break;
            default:
                Debug.LogError("Unknown building type for change order");
                break;
        }
    }
}
