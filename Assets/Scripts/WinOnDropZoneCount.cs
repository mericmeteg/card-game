using UnityEngine;
using DG.Tweening;

public class WinOnDropZoneCount : MonoBehaviour
{
    [SerializeField] GameObject gameOverPanel;  
    [SerializeField] int targetCount = 28;      

    bool shown;

    void Update()
    {
        if (shown) return;

        int count = transform.childCount; 
        if (count >= targetCount)
            ShowWin();
    }

    void ShowWin()
    {
        shown = true;
        gameOverPanel.SetActive(true);
        gameOverPanel.transform.localScale = Vector3.zero;
        gameOverPanel.transform.DOScale(Vector3.one, 0.4f).SetEase(Ease.OutBack);
        
    }
}
