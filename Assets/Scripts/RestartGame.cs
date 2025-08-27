using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartGame : MonoBehaviour
{
    [SerializeField] private string OyunEkrani; // Inspector’dan yaz

    public void LoadScene()
    {
        if (!string.IsNullOrEmpty(OyunEkrani))
            SceneManager.LoadScene(OyunEkrani);
    }

    public void LoadSceneByName(string OyunEkrani)
    {
        if (!string.IsNullOrEmpty(OyunEkrani))
            SceneManager.LoadScene(OyunEkrani);
    }
}
