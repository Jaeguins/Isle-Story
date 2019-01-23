using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public enum CamType{
    TOPVIEW,BUILDINGVIEW
}

public class CameraManager : MonoBehaviour {
    public static CameraManager Instance;
    public List<CameraController> cameras;
    public CamType camStatus = 0;
    public GameUI manager;
    private void Awake() {
        Instance = this;
    }
    public IEnumerator SwitchCamera(CamType target) {
        StopAllCoroutines();
        yield return StartCoroutine(SwitchCameraInternal(target));
    }

    IEnumerator SwitchCameraInternal(CamType target) {
        //yield return StartCoroutine(Fader.FadeOut());
        Fader.ChangeTarget(0);
        cameras[(int)camStatus].enabled = false;
        camStatus = target;
        cameras[(int)target].enabled = true;

        //TODO change worldspace event cam

        Fader.ChangeTarget(1);
        //yield return StartCoroutine(Fader.FadeIn());
        yield return null;
    }
    public CameraController GetNowActive() {
        return cameras[(int)camStatus];
    }
}
