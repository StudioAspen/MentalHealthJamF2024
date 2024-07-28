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

    // Start is called before the first frame update
    void Start() {
        timer = maxTimer;
    }

    // Update is called once per frame
    void Update() {
        if (timerActive) {
            UpdateTimer();
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
}
