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

    public IEnumerator SwitchCamera(CamType target) {
        
        StopAllCoroutines();
        yield return StartCoroutine(SwitchCameraInternal(target));
    }

    IEnumerator SwitchCameraInternal(CamType target) {
        yield return StartCoroutine(Fader.FadeOut());
        cameras[(int)camStatus].CameraView.enabled = false;
        camStatus = target;
        cameras[(int)target].CameraView.enabled = true;
        manager.buildingMenu.enabled = true;
        yield return StartCoroutine(Fader.FadeIn());
    }
    public CameraController GetNowActive() {
        return cameras[(int)camStatus];
    }
}
