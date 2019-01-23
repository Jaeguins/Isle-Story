using UnityEngine;
using System.Collections;

public class Pauser : MonoBehaviour {
    public bool paused=false;
    public void Pause() {
        paused= true;
        Time.timeScale = 0;
    }
    public void Resume() {
        paused = false;
        Time.timeScale = 1;
    }
}
