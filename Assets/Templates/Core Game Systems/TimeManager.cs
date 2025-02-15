using UnityEngine;


// Time Management System
public class TimeManager : MonoBehaviour {
    public static TimeManager Instance { get; private set; }
    
    [SerializeField] private float defaultTimeScale = 1f;
    [SerializeField] private float pausedTimeScale = 0f;
    
    private void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else {
            Destroy(gameObject);
        }
    }

    public void SetTimeScale(float scale) {
        Time.timeScale = scale;
    }

    public void PauseTime() {
        SetTimeScale(pausedTimeScale);
    }

    public void ResumeTime() {
        SetTimeScale(defaultTimeScale);
    }
}
