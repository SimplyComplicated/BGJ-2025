using System;
using UnityEngine;

// Example of a Game Manager using Config
public class GameManager : MonoBehaviour {
    private float gameTimer;
    private bool isGameOver;

    private void Start() {
        if (GameConfigHelper.Config.skipMainMenu) {
            StartGame();
        }
    }

    private void StartGame()
    {
        throw new NotImplementedException();
    }

    private void Update() {
        if (!isGameOver) {
            gameTimer += Time.deltaTime;
            if (gameTimer >= GameConfigHelper.Config.timeLimit) {
                EndGame();
            }
        }
    }

    private void EndGame()
    {
        throw new NotImplementedException();
    }

    // Quick game setup based on difficulty
    public void SetupGame(string difficulty) {
        switch (difficulty.ToLower()) {
            case "easy":
                GameConfigHelper.Config.SetEasyMode();
                break;
            case "hard":
                GameConfigHelper.Config.SetHardMode();
                break;
        }
    }
}