using UnityEngine;
using UnityEngine.UI;

public class SoundEffectToggle : MonoBehaviour
{
    [Header("UI")]
    public Image soundEffectImage;
    public Sprite soundOnSprite;
    public Sprite soundOffSprite;

    [Header("Audio Sources to control")]
    public AudioSource[] soundEffects;

    private bool isSoundOn;

    private void Start()
    {
        isSoundOn = PlayerPrefs.GetInt("SoundOn", 1) == 1;
        ApplySoundSettings();
    }

    public void ToggleSound()
    {
        isSoundOn = !isSoundOn;
        PlayerPrefs.SetInt("SoundOn", isSoundOn ? 1 : 0);
        PlayerPrefs.Save();
        ApplySoundSettings();
        Debug.Log("ToggleSound çağrildi, isSoundOn=" + isSoundOn);
    }

    private void ApplySoundSettings()
    {
        if (soundEffectImage != null)
            soundEffectImage.sprite = isSoundOn ? soundOnSprite : soundOffSprite;

        if (soundEffects != null)
            foreach (var a in soundEffects)
                if (a) a.mute = !isSoundOn;
    }
}
