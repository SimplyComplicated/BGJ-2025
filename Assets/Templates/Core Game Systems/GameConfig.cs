using UnityEngine;

[CreateAssetMenu(fileName = "GameConfig", menuName = "GameJam/GameConfig")]
public class GameConfig : ScriptableObject {
    [Header("Game Information")]
    public string gameName = "Game Jam Game";
    public string version = "1.0.0";
    public bool isDebugMode = true;

    [Header("Player Settings")]
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    public int maxHealth = 100;
    public float invincibilityTime = 1f;

    [Header("Game Balance")]
    public float difficultyMultiplier = 1f;
    public int scoreMultiplier = 1;
    public float timeLimit = 180f; // 3 minutes
    public int targetScore = 1000;

    [Header("Spawning")]
    public float spawnRate = 2f;
    public int maxEnemies = 10;
    public float powerUpFrequency = 0.1f;

    [Header("Audio")]
    public float masterVolume = 1f;
    public float musicVolume = 0.8f;
    public float sfxVolume = 1f;

    [Header("Development")]
    public bool skipMainMenu = false;
    public bool infiniteHealth = false;
    public bool unlockAllLevels = false;

    // Runtime modifications
    private float currentDifficulty;

    private void OnEnable() {
        currentDifficulty = difficultyMultiplier;
    }

    // Example of a dynamic property that scales with game progression
    public float GetCurrentMoveSpeed(float progressPercentage) {
        return moveSpeed * (1 + (progressPercentage * 0.5f));
    }

    // Quick access for common game states
    public void SetEasyMode() {
        difficultyMultiplier = 0.5f;
        spawnRate *= 0.7f;
        timeLimit *= 1.5f;
    }

    public void SetHardMode() {
        difficultyMultiplier = 2f;
        spawnRate *= 1.5f;
        timeLimit *= 0.7f;
    }
}

// Helper class to access GameConfig anywhere
public static class GameConfigHelper {
    private static GameConfig instance;

    public static GameConfig Config {
        get {
            if (instance == null) {
                instance = Resources.Load<GameConfig>("GameConfig");
                if (instance == null) {
                    Debug.LogError("GameConfig not found in Resources folder!");
                }
            }
            return instance;
        }
    }
}

// Example Usage in a Player Controller
public class PlayerController : MonoBehaviour {
    private void Update() {
        // Easy access to config values
        float speed = GameConfigHelper.Config.moveSpeed;
        if (GameConfigHelper.Config.isDebugMode) {
            speed *= 2; // Double speed in debug mode
        }
        
        // Movement with configurable speed
        transform.Translate(Vector3.right * speed * Time.deltaTime);
    }
}

