using UnityEngine;

public class PauseMenu : MonoBehaviour
{
   public void PauseGame()
   {
        Time.timeScale=0f;
   }
   public void ResumeGame()
   {
        Time.timeScale=1f;
   }
}
