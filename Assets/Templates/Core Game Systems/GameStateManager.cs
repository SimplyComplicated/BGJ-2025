using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Collections;

// Core GameState
public enum GameState {
    MainMenu,
    Loading,
    Playing,
    Paused,
    GameOver,
    Victory
}

// GameStateManager
public class GameStateManager : MonoBehaviour {
    public static GameStateManager Instance { get; private set; }
    
    private GameState currentState;
    public event Action<GameState> OnStateChanged;
    public event Action<GameState, GameState> OnBeforeStateChanged;

    public GameState CurrentState {
        get => currentState;
        private set {
            if (currentState == value) return;
            OnBeforeStateChanged?.Invoke(currentState, value);
            currentState = value;
            OnStateChanged?.Invoke(currentState);
            HandleStateChange(currentState);
        }
    }

    private void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeState();
        }
        else {
            Destroy(gameObject);
        }
    }

    private void InitializeState() {
        CurrentState = GameState.MainMenu;
    }

    public void SetState(GameState newState) {
        if (CanTransitionToState(newState)) {
            CurrentState = newState;
        }
    }

    private void HandleStateChange(GameState state) {
        switch (state) {
            case GameState.MainMenu:
                TimeManager.Instance.ResumeTime();
                SceneLoader.Instance.LoadScene("MainMenu");
                break;
                
            case GameState.Playing:
                TimeManager.Instance.ResumeTime();
                break;
                
            case GameState.Paused:
                TimeManager.Instance.PauseTime();
                break;
                
            case GameState.GameOver:
                TimeManager.Instance.PauseTime();
                break;
        }
    }

    private bool CanTransitionToState(GameState newState) {
        // Same transition rules as before
        switch (currentState) {
            case GameState.MainMenu:
                return newState == GameState.Loading || newState == GameState.Playing;
            case GameState.Loading:
                return newState == GameState.Playing || newState == GameState.MainMenu;
            case GameState.Playing:
                return newState == GameState.Paused || 
                       newState == GameState.GameOver || 
                       newState == GameState.Victory;
            case GameState.Paused:
                return newState == GameState.Playing || newState == GameState.MainMenu;
            case GameState.GameOver:
            case GameState.Victory:
                return newState == GameState.MainMenu || newState == GameState.Loading;
            default:
                return false;
        }
    }
}


