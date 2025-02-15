using UnityEngine;

// SceneInitializer - Place this in each scene
public class SceneInitializer : MonoBehaviour {
    [SerializeField] private GameState sceneState;
    
    private void Start() {
        if (GameStateManager.Instance != null) {
            GameStateManager.Instance.SetState(sceneState);
        }
    }
}