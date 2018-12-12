using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public enum CamType{
    TOPVIEW,BUILDINGVIEW
}

public class CameraManager : MonoBehaviour {
    public List<CameraController> cameras;
    public CamType camStatus = 0;
    public GameUI manager;

    public void SwitchCamera(CamType target) {
        camStatus = target;
        StopAllCoroutines();
        StartCoroutine(SwitchCameraInternal(target));
    }

    IEnumerator SwitchCameraInternal(CamType target) {
        yield return StartCoroutine(Fader.FadeOut());
        for(int i = 0; i < cameras.Count; i++) {
            cameras[i].cam.enabled = false;
        }
        cameras[(int)target].enabled = true;
        manager.buildingMenu.enabled = true;
        yield return StartCoroutine(Fader.FadeIn());
    }
    public CameraController GetNowActive() {
        return cameras[(int)camStatus];
    }
}
