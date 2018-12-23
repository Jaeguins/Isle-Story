using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using System.Collections;

public class Property : MonoBehaviour {
    public static Property Instance;
    public static string NaN = "NaN";
    public Entity Target;
    private void Awake() {
        Instance = this;
        gameObject.SetActive(false);
    }
    public Text nameText, typeText;
    public CommandPanel commandPanel;
    public void Bind(Entity target) {
        Clear();
        Target = target;
        nameText.text = target.UIName;
        typeText.text = target.UIType;
        gameObject.SetActive(true);
        commandPanel.Bind(target);
        //TODO bind others
    }
    public void Clear() {
        //TODO Target Clearing
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
