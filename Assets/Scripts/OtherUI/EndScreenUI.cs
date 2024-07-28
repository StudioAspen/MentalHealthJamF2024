using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class EndScreenUI : MonoBehaviour
{
    [SerializeField] GameObject endScreen;
    [SerializeField] TMP_Text endText;
    private void Start() {
        endScreen.SetActive(false);
    }

    public void SetEndScreen(bool win) {
        Time.timeScale = 0f;
        endScreen.SetActive(true);
        if(win) {
            endText.text = "You Survived";
        }
        else {
            endText.text = "You Got Overwhelmed";
        }
    }
}
