using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] bool useTimer = false;
    ResourceManager resourceManager;
    EndScreenUI endScreen;
    WinConditionTimer winConditionTimer;

    private void Start() {
        // Finding objects
        resourceManager = FindObjectOfType<ResourceManager>();
        endScreen = FindObjectOfType<EndScreenUI>();
        winConditionTimer = FindAnyObjectByType<WinConditionTimer>();
        
        resourceManager.OnReachGoal.AddListener(WinGame); // Setting listener for winning game
        PlayAreaManager.Instance.OnBoardFilled.AddListener(LoseGame);

        Time.timeScale = 0f;
    }

    private void Update() {
        if(useTimer) {
            CheckTimerWin();
        }

        CheckResources();
    }

    public void StartGame() {
        Time.timeScale = 1f;
        resourceManager.SetDraining(true); // Starting draining
        winConditionTimer.SetTimerActive(true); // Starting timer
    }

    public void WinGame() {
        Time.timeScale = 0f;
        endScreen.SetEndScreen(true);
        resourceManager.SetDraining(false);
        winConditionTimer.SetTimerActive(false);
    }
    public void LoseGame() {
        Time.timeScale = 0f;
        endScreen.SetEndScreen(false);
        resourceManager.SetDraining(false);
        winConditionTimer.SetTimerActive(false);
    }

    private void CheckResources() {
        if(resourceManager.currentPhysicalHealth <= 0 ||
            resourceManager.currentMentalHealth <= 0 ||
            resourceManager.currentFinancialHealth <= 0) {
            LoseGame();
        }
    }
    private void CheckTimerWin() {
        if(winConditionTimer.timer <= 0) {
            WinGame();
        }
    }
}
