using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class BuildingPanel : EntityPanel {
    public List<BuildingButton> buttons;
    public override void Bind(Entity entity) {
        gameObject.SetActive(entity is Buildable);
        if (!(entity is Buildable)) return;
        base.Bind(entity);
        foreach(BuildingButton t in buttons) {
            t.Bind(entity as Buildable);
        }
    }
}
