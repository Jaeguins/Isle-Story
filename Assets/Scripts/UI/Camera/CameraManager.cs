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
        yield return StartCoroutine(Fader.FadeOut());
        cameras[(int)camStatus].enabled = false;
        camStatus = target;
        cameras[(int)target].enabled = true;
        manager.buildingMenu.enabled = true;

        //TODO change worldspace event cam


        yield return StartCoroutine(Fader.FadeIn());
    }
    public CameraController GetNowActive() {
        return cameras[(int)camStatus];
    }
}
