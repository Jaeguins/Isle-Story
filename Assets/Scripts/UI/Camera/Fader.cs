using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fader : MonoBehaviour {
    public RawImage fader;
    static Fader Instance;
    public Color color = Color.black;
    public float speed;
    private void Start() {
        Instance = this;
    }
    public IEnumerator FadeOutInternal() {
        color.a = 0;
        while (color.a < 1f) {
            Debug.Log("FO call");
            color.a += speed;
            fader.color = color;
            yield return new WaitForEndOfFrame();
        }
    }
    public IEnumerator FadeInInternal() {
        color.a = 1;
        while (color.a >0f) {
            Debug.Log("FI call");
            color.a -= speed;
            fader.color = color;
            yield return new WaitForEndOfFrame();
        }
    }
    public static IEnumerator FadeOut() {
        Debug.Log("FO started");
        yield return Instance.StartCoroutine(Instance.FadeOutInternal());
        Debug.Log("FO end");
    }
    public static IEnumerator FadeIn() {
        Debug.Log("FI started");
        yield return Instance.StartCoroutine(Instance.FadeInInternal());
        Debug.Log("FI end");
    }
}
