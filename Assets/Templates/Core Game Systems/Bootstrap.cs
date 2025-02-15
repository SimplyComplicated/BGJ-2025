using System;
using System.Threading.Tasks;
using UnityEngine;

public class Bootstrap : MonoBehaviour {
    [SerializeField] private GameConfig config;
    
    async void Start() {
        // Version checking
        await CheckForUpdates();
        
        // Analytics initialization
        SetupAnalytics();
        
        // Network setup
        await InitializeNetwork();
        
        // Load saved data
        LoadPlayerProgress();
        
        // Only when everything is ready
        SceneLoader.Instance.LoadScene("MainMenu");
    }

    private void LoadPlayerProgress()
    {
        throw new NotImplementedException();
    }

    private async Task InitializeNetwork()
    {
        throw new NotImplementedException();
    }

    private void SetupAnalytics()
    {
        throw new NotImplementedException();
    }

    private async Task CheckForUpdates()
    {
        throw new NotImplementedException();
    }
}