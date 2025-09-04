using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class SettingsShowHideScript : MonoBehaviour
{
    [Header("Assign in Inspector")]
    [SerializeField] RectTransform panel;     
    [SerializeField] CanvasGroup canvasGroup; 
    [SerializeField] Image backdrop;          

    [Header("Animation")]
    [SerializeField] float duration = 0.35f;
    [SerializeField] Ease easeIn  = Ease.OutCubic; 
    [SerializeField] Ease easeOut = Ease.InCubic;  
    [SerializeField] float backdropAlpha = 0.6f;

    [Header("Positions (Anchored)")]
    [SerializeField] Vector2 shownPos  = Vector2.zero;      
    [SerializeField] Vector2 hiddenPos = new Vector2(0, -900f); 

    bool isVisible;
    Tween t;

    void Awake()
    {
        if (!panel)      Debug.LogError("[Settings] Panel atanmamış!");
        if (!canvasGroup) Debug.LogError("[Settings] CanvasGroup atanmamış!");

        
        ApplyHiddenState();
    }

    void ApplyHiddenState()
    {
        t?.Kill();
        if (panel) panel.anchoredPosition = hiddenPos;
        if (canvasGroup)
        {
            canvasGroup.alpha = 0f;
            canvasGroup.blocksRaycasts = false;
            canvasGroup.interactable = false;
        }
        if (backdrop)
        {
            var c = backdrop.color; c.a = 0f; backdrop.color = c;
            backdrop.gameObject.SetActive(true); 
        }
        isVisible = false;
    }

    public void Show()
    {
        if (isVisible || panel == null || canvasGroup == null) return;

        t?.Kill();
        panel.SetAsLastSibling();
        canvasGroup.blocksRaycasts = true;

        Sequence s = DOTween.Sequence().SetUpdate(true); 
        s.Join(panel.DOAnchorPos(shownPos, duration).SetEase(easeIn));
        s.Join(canvasGroup.DOFade(1f, duration * 0.9f));
        if (backdrop) s.Join(backdrop.DOFade(backdropAlpha, duration));
        s.OnComplete(() => { canvasGroup.interactable = true; isVisible = true; });
        t = s;
    }

    public void Hide()
    {
        if (!isVisible || panel == null || canvasGroup == null) return;

        t?.Kill();
        canvasGroup.interactable = false;

        Sequence s = DOTween.Sequence().SetUpdate(true);
        s.Join(panel.DOAnchorPos(hiddenPos, duration).SetEase(easeOut));
        s.Join(canvasGroup.DOFade(0f, duration * 0.9f));
        if (backdrop) s.Join(backdrop.DOFade(0f, duration));
        s.OnComplete(() => { canvasGroup.blocksRaycasts = false; isVisible = false; });
        t = s;
    }

    public void Toggle()
    {
        if (isVisible) Hide();
        else Show();
    }
}
