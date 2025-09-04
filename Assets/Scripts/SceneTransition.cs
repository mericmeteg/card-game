using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class SceneFader : MonoBehaviour
{
    [SerializeField] Image fade;     
    [SerializeField] float dur = 0.8f;
    [SerializeField] bool fadeInOnStart = true; 

    void Start()
    {
        if (fadeInOnStart) fade.DOFade(0f, dur);  
    }

    public void Load(string sceneName)
    {
        fade.DOFade(1f, dur).OnComplete(() => SceneManager.LoadScene(sceneName));
    }
}
