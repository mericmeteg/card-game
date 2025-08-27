using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnMainMenu : MonoBehaviour
{

    [SerializeField] private string main = "MainMenu";

    public void Go()
    {
        Time.timeScale = 1f; 
        SceneManager.LoadScene(main);
    }

    
    
}
