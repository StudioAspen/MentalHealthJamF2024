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
    }

    private void Update() {
        if(useTimer) {
            CheckTimerWin();
        }

        CheckResources();
    }

    public void StartGame() {
        resourceManager.SetDraining(true); // Starting draining
        winConditionTimer.SetTimerActive(true); // Starting timer
    }

    public void WinGame() {
        endScreen.SetEndScreen(true);
        resourceManager.SetDraining(false);
        winConditionTimer.SetTimerActive(false);
    }
    public void LoseGame() {
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
