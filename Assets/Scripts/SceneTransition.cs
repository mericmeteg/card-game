using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class SceneFader : MonoBehaviour
{
    [SerializeField] Image fade;     // tam ekran siyah Image
    [SerializeField] float dur = 0.8f;
    [SerializeField] bool fadeInOnStart = true; // sadece oyun sahnesinde true

    void Start()
    {
        if (fadeInOnStart) fade.DOFade(0f, dur);   // açılışta şeffaflaş
    }

    public void Load(string sceneName)
    {
        fade.DOFade(1f, dur).OnComplete(() => SceneManager.LoadScene(sceneName));
    }
}
