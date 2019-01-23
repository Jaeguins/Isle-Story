using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fader : MonoBehaviour {
    public RawImage fader;
    static Fader Instance;

    public Color targetColor = Color.black,nowColor=Color.black;
    public float speed;
    public float status=0;
    private void Start() {
        Instance = this;
    }
    private void LateUpdate() {
        if (status<1) {
            status += speed;
            if (status > 1) status = 1;
            fader.color = Color.Lerp(nowColor, targetColor, status);
            if (status == 1) nowColor = fader.color;
        }
        fader.raycastTarget = fader.color.a != 0;
    }
    public void ChangeTargetLocal(float i) {
        ChangeTarget(new Color(0, 0, 0, i));
    }
    public static void ChangeTarget(float i) {
        Instance.ChangeTarget(new Color(0, 0, 0, i));
    }
    public void ChangeTarget(Color color) {
        targetColor = color;
        status = 0;
    }
    public void ChangeImmediately(float value) {
        ChangeImmediately(new Color(0, 0, 0, value));
    }
    public void ChangeImmediately(Color color) {
        fader.color = color;
        nowColor = fader.color;
        status = 1;
    }
}
