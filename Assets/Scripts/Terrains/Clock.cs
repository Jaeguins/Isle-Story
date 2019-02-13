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
        UpdateSun();

        currentTimeOfDay += (Time.deltaTime / (secondsInDay + secondsInNight)) * timeMultiplier;
        isDay = currentTimeOfDay <= 0.625 ? true : false;
        if (currentTimeOfDay >= 1) {
            currentTimeOfDay = 0;
        }
    }

    void UpdateSun() {
        if (currentTimeOfDay <= 0.125) {
            sun.intensity = 8*currentTimeOfDay;
        }
        else if(currentTimeOfDay <= 0.5) {
            sun.intensity = 1;
        }else if(currentTimeOfDay <= 0.625) {
            sun.intensity = 8 * (0.625f-currentTimeOfDay);
        }
        else {
            sun.intensity = 0;
        }
    }
}