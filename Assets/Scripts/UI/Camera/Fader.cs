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
            color.a += speed;
            fader.color = color;
            yield return new WaitForEndOfFrame();
        }
    }
    public IEnumerator FadeInInternal() {
        color.a = 1;
        while (color.a >0f) {
            color.a -= speed;
            fader.color = color;
            yield return new WaitForEndOfFrame();
        }
    }
    public static IEnumerator FadeOut() {
        yield return Instance.StartCoroutine(Instance.FadeOutInternal());
    }
    public static IEnumerator FadeIn() {
        yield return Instance.StartCoroutine(Instance.FadeInInternal());
    }
}
