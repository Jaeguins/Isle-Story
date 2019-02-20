using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BuildingButton : MonoBehaviour {
    public Buildable Target;
    public Button b;
    public Entity Prefab;
    public BuildCondition condition = new BuildCondition();
    public void ClickCallback() {
        if (Prefab)
            Selector.Instance.RequestLocation(Target.GetCommandReceiver(), new BuildCommand(Prefab),Prefab);
        else
            Selector.Instance.RequestTarget(Target.GetCommandReceiver(), new DestroyCommand(null));
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
