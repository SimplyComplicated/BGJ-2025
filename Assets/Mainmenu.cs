using UnityEngine;
using UnityEngine.SceneManagement;
public class Mainmenu : MonoBehaviour
{
    public string gameScene;
    public GameObject settingsPanel;
    public void LoadGameScene()
    {
        SceneManager.LoadScene(gameScene);
    }

    public void EnableSettingsPanel()
    {
        settingsPanel.SetActive(true);
    }

    public void DisableSettingsPanel()
    {
        settingsPanel.SetActive(false);
    }

   
}
