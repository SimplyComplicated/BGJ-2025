using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;


// Scene Management System
public class SceneLoader : MonoBehaviour {
    public static SceneLoader Instance { get; private set; }
    
    [SerializeField] private string mainMenuScene = "MainMenu";
    [SerializeField] private string gameScene = "Game";
    
    private void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else {
            Destroy(gameObject);
        }
    }

    public void LoadScene(string sceneName, Action onComplete = null) {
        StartCoroutine(LoadSceneAsync(sceneName, onComplete));
    }

    private IEnumerator LoadSceneAsync(string sceneName, Action onComplete) {
        GameStateManager.Instance.SetState(GameState.Loading);
        
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        
        while (!asyncLoad.isDone) {
            yield return null;
        }
        
        onComplete?.Invoke();
    }
}