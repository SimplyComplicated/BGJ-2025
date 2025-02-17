using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance => instance;

    public AudioManager audioManager;
    
     void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        audioManager.PlayMusic("BgMusic");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
