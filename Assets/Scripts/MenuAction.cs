using UnityEngine;
using UnityEngine.SceneManagement;
public class MenuAction : MonoBehaviour
{
    public string mainMenu = "MainMenu";
    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(mainMenu);

    }
     public void ReturnMainMenu()
    {
        
        if (!string.IsNullOrEmpty(mainMenu))
            SceneManager.LoadScene(mainMenu);
    }
    
}
