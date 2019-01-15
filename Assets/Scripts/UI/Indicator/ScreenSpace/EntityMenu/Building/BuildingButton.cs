using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BuildingButton : MonoBehaviour {
    public Buildable Target;
    public Button b;
    public Entity Prefab;
    public BuildCondition condition=new BuildCondition();
    public void ClickCallback() {
        Selector.Instance.RequestLocation(Target.GetCommandReceiver(),Prefab.sizeType, new BuildCommand(Prefab));
    }
    public void Bind(Buildable target) {
        Target = target;
    }
    public virtual bool Condition() {
        if (!Target.HasCommandReceiver()) return false;
        if (condition.TechLevel > Target.GetTech()) return false;
        return true;
    }
    public void LateUpdate() {
        if (Target != null) {
            b.interactable = Condition();
        }
    }
}
[System.Serializable]
public struct BuildCondition {
    public int TechLevel;
}
