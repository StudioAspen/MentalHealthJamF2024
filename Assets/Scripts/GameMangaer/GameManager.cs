using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] bool useTimer = false;
    [SerializeField] GameObject startScreen;
    ResourceManager resourceManager;
    EndScreenUI endScreen;
    WinConditionTimer winConditionTimer;
    Board board;

    private void Start() {
        // Finding objects
        resourceManager = FindObjectOfType<ResourceManager>();
        endScreen = FindObjectOfType<EndScreenUI>(true);
        winConditionTimer = FindAnyObjectByType<WinConditionTimer>();
        board = FindObjectOfType<Board>();

        startScreen.SetActive(true);
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
        if (board != null) {
            board.SetGameActive(true);
        }
    }

    public void LoseGame(string msg)
    {
        endScreen.ShowEndScreen(msg);
        resourceManager.SetDraining(false);
        winConditionTimer.SetTimerActive(false);
        if(board != null)
        {
            board.SetGameActive(false);
        }
    }

    public void WinGame() {
        endScreen.ShowResultsEndScreen(resourceManager.currentPhysicalHealth/resourceManager.maxGoal, resourceManager.currentMentalHealth/resourceManager.maxGoal, resourceManager.currentFinancialHealth/resourceManager.maxGoal);
        resourceManager.SetDraining(false);
        winConditionTimer.SetTimerActive(false);
        if (board != null) {
            board.SetGameActive(false);
        }

    }

    private void CheckResources() {
        if(resourceManager.currentGoal >= resourceManager.maxGoal)
        {
            WinGame();
        }
    }
    private void CheckTimerWin() {
        if(winConditionTimer.timer <= 0) {
            LoseGame("Ran out of time!");
        }
    }
}
