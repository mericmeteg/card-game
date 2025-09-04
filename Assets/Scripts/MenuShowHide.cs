using UnityEngine;
using DG.Tweening;

public class MenuShowHide : MonoBehaviour
{
    [Header("Assign in Inspector")]
    public GameObject MenuScreen;      
    public RectTransform MenuPanel;    
    public CanvasGroup canvasGroup;   

    [Header("Animation Settings")]
    public float duration = 0.35f;
    public Ease easeIn = Ease.OutBack;  
    public Ease easeOut = Ease.InBack;   
    public Vector2 shownPos = Vector2.zero;        
    public Vector2 hiddenPos = new Vector2(0, -900f); 

    Tween t;
    bool isVisible;

    void Awake()
    {
        if (canvasGroup == null && MenuScreen != null)
            canvasGroup = MenuScreen.GetComponent<CanvasGroup>();

        ApplyHidden();
    }

    void ApplyHidden()
    {
        if (MenuPanel) MenuPanel.anchoredPosition = hiddenPos;
        if (canvasGroup)
        {
            canvasGroup.alpha = 0f;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
        }
        MenuScreen.SetActive(true); 
        isVisible = false;
    }

    public void Open()
    {
        if (isVisible) return;
        t?.Kill();

        MenuScreen.SetActive(true);
        MenuScreen.transform.SetAsLastSibling();

        if (canvasGroup) canvasGroup.blocksRaycasts = true;

        t = DOTween.Sequence()
            .Join(MenuPanel.DOAnchorPos(shownPos, duration).SetEase(easeIn))
            .Join(canvasGroup.DOFade(1f, duration * 0.9f))
            .OnComplete(() =>
            {
                if (canvasGroup) canvasGroup.interactable = true;
                isVisible = true;
            });
    }

    public void Close()
    {
        if (!isVisible) return;
        t?.Kill();

        if (canvasGroup) canvasGroup.interactable = false;

        t = DOTween.Sequence()
            .Join(MenuPanel.DOAnchorPos(hiddenPos, duration).SetEase(easeOut))
            .Join(canvasGroup.DOFade(0f, duration * 0.9f))
            .OnComplete(() =>
            {
                if (canvasGroup) canvasGroup.blocksRaycasts = false;
                isVisible = false;
            });
    }

    public void Toggle()
    {
        if (isVisible) Close();
        else Open();
    }
}
