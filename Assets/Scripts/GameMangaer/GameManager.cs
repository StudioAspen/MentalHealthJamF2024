using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    ResourceManager resourceManager;
    EndScreenUI endScreen;

    private void Start() {
        // Finding objects
        resourceManager = FindObjectOfType<ResourceManager>();
        endScreen = FindObjectOfType<EndScreenUI>();
        
        resourceManager.OnReachGoal.AddListener(WinGame); // Setting listener for winning game
    }

    private void Update() {
        CheckResources();
    }

    public void StartGame() {
        resourceManager.SetDraining(true); // Starting draining
    }

    public void WinGame() {
        endScreen.SetEndScreen(true);
        resourceManager.SetDraining(false);
    }
    public void LoseGame() {
        endScreen.SetEndScreen(false);
        resourceManager.SetDraining(false);
    }

    private void CheckResources() {
        if(resourceManager.currentPhysicalHealth <= 0 ||
            resourceManager.currentMentalHealth <= 0 ||
            resourceManager.currentFinancialHealth <= 0) {
            LoseGame();
        }
    }
}
