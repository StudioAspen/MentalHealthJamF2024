using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class WinConditionTimer : MonoBehaviour
{
    [SerializeField] TMP_Text timerText;
    [SerializeField] public float maxTimer;
    public float timer;
    bool timerActive = false;

    [SerializeField] AnimationCurve redCurve;
    public float redDuration = 1f;
    float redTimer;

    // Start is called before the first frame update
    void Start() {
        timer = maxTimer;
    }

    // Update is called once per frame
    void Update() {
        if (timerActive) {
            UpdateTimer();
        }
        if(redTimer >= 0) {
            redTimer -= Time.deltaTime;
            float value = redCurve.Evaluate(redTimer / redDuration);
            timerText.color = Color.Lerp(Color.white, Color.red, value);
        }
        else {
            timerText.color = Color.white;
        }
    }

    public void SetTimerActive(bool value) {
        timerActive = value;
    }
    private void UpdateTimer() {
        timer = Mathf.Max(0, timer - Time.deltaTime);

        TimeSpan timeSpan = TimeSpan.FromSeconds(timer);
        timerText.text = "Timer: " + timeSpan.ToString(@"mm\:ss");
    }
    public void LowerTimer(float value) {
        timer = Mathf.Max(0, timer - value);

        redTimer = redDuration; // setting tezt duration

        TimeSpan timeSpan = TimeSpan.FromSeconds(timer);
        timerText.text = "Timer: " + timeSpan.ToString(@"mm\:ss");
    } 
}
