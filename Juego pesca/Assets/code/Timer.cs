using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Grapple;

public class Timer : MonoBehaviour {
    TextMeshProUGUI timer;
    public float tiempo = 0f;
    bool flag;
    bool timeSetted;
    bool paused;
    public GrapplingHookLogic hookLogic;
    // Start is called before the first frame update
    void Start() {
        timer = GetComponent<TextMeshProUGUI>();
        flag = false;
        timeSetted = false;
    }

    // Update is called once per frame
    void Update() {
        timeSetted = hookLogic.GetStartMission();
        flag = timeSetted;

        if (!paused){
            if (flag) {
                if (timeSetted) {
                    tiempo = hookLogic.GetTime()*60;
                    hookLogic.SetStartMission(false);
                }
            }

            if (tiempo > 0f) {
                tiempo -= Time.deltaTime;
                SetTimer(tiempo);
            }

            timer.text = tiempo.ToString("F3");
            if (tiempo <= 0f) {
                timer.text = "";
                flag = false;
            }
        }
        else{
            timer.text = "";
        }
    }

    public void SetPause(bool flag){
        paused = flag;
    }

    public void SetTimer(float limite) {
        timer.text = limite.ToString("F3");
    }
    public float GetTimer(){
        return tiempo;
    }
}
