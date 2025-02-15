using UnityEngine;


// Example of a State-Based Game Controller
public class GameController : MonoBehaviour {
    private void Start() {
        GameStateManager.Instance.OnStateChanged += HandleGameStateChanged;
    }

    private void OnDestroy() {
        if (GameStateManager.Instance != null) {
            GameStateManager.Instance.OnStateChanged -= HandleGameStateChanged;
        }
    }

    private void HandleGameStateChanged(GameState newState) {
        // Handle game-specific logic here
        switch (newState) {
            case GameState.Playing:
                // Initialize game elements
                break;
            case GameState.GameOver:
                // Clean up game elements
                break;
        }
    }

    // Example usage
    public void StartNewGame() {
        SceneLoader.Instance.LoadScene("Game", () => {
            GameStateManager.Instance.SetState(GameState.Playing);
        });
    }
}