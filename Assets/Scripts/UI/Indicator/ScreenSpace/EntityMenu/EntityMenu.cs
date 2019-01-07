using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;

public class EntityMenu : MonoBehaviour {
    public static EntityMenu Instance;
    public static string NaN = "NaN";
    public Entity Target;
    private void Awake() {
        Instance = this;
        gameObject.SetActive(false);
    }
    public Text nameText, typeText;
    public CommandPanel commandPanel;
    public void BindUnit(Unit unit) {
        BindStart(unit);
    }
    public void BindBuilding(Building building) {
        BindStart(building);
    }
    void BindStart(Entity target) {
        Clear();
        Target = target;
        target.Select();
        nameText.text = target.UIName;
        typeText.text = target.UIType;
        gameObject.SetActive(true);
        commandPanel.Bind(target);
    }
    public void Clear() {
        if(Target)
        Target.Deselect();
        //TODO Target Clearing
        BuildingMenu.Instance.Close();
        Target = null;
        nameText.text = NaN;
        typeText.text = NaN;
        commandPanel.Clear();
    }
    public void Close() {
        Clear();
        gameObject.SetActive(false);
    }
}
