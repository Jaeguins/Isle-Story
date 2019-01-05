using UnityEngine;
using System.Collections;

public class Clock : MonoBehaviour {
    public static Clock Instance;
    public Light sun;
    public float secondsInDay = 120f;
    public float secondsInNight = 60f;
    [Range(0, 1)]
    public float currentTimeOfDay = 0;
    public float timeMultiplier = 1f;
    float intensityMultiplier = 1;
    public bool isDay = false;
    public bool Updator = true;
    float sunInitialIntensity;
    float nightLength = 0.1f;
    private void Awake() {
        Instance = this;
    }
    public static float GetTime() {
        return Instance.currentTimeOfDay;
    }
    public static bool IsDay() {
        return Instance.isDay;
    }
    void Start() {
        sunInitialIntensity = sun.intensity;
    }

    void Update() {
        if (Updator) {
            nightLength = secondsInNight / 2 / (secondsInDay + secondsInNight);
            Updator = false;
        }
        UpdateSun();

        currentTimeOfDay += (Time.deltaTime / (secondsInDay + secondsInNight)) * timeMultiplier;

        if (currentTimeOfDay >= 1) {
            currentTimeOfDay = 0;
        }
    }

    void UpdateSun() {
        sun.transform.localRotation = Quaternion.Euler((currentTimeOfDay * 360f) - 90, 170, 0);
        
        if (currentTimeOfDay <= nightLength || currentTimeOfDay >= 1 - nightLength) {
            isDay = false;
            intensityMultiplier = 0;
        }
        else if (currentTimeOfDay <= nightLength + 0.02f) {
            isDay = true;
            intensityMultiplier = Mathf.Clamp01((currentTimeOfDay - nightLength) * (1 / 0.02f));
        }
        else if (currentTimeOfDay >= 0.98f - nightLength) {
            isDay = true;
            intensityMultiplier = Mathf.Clamp01(1 - ((currentTimeOfDay - 0.98f + nightLength) * (1 / 0.02f)));
        }

        sun.intensity = sunInitialIntensity * intensityMultiplier;
    }
}