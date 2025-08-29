using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class CardView : MonoBehaviour
{
    [Header("Theme (Single SO)")]
    [SerializeField] private CardThemeSO theme;

    [Header("Front/Back Roots")]
    [SerializeField] private GameObject frontGroup; 
    [SerializeField] private GameObject backGroup;  

    [Header("Front - Common")]
    [SerializeField] private TextMeshProUGUI rankText;
    [SerializeField] private Color redColor = new Color(0.85f, 0.1f, 0.1f);
    [SerializeField] private Color blackColor = Color.black;

    [Header("Front - Simple (resimsiz)")]
    [SerializeField] private GameObject simpleGroup;
    [SerializeField] private Image smallSuitSimple;
    [SerializeField] private Image bigSuitSimple;

    [Header("Front - Fancy (J/Q/K)")]
    [SerializeField] private GameObject fancyGroup;
    [SerializeField] private Image smallSuitFancy;
    [SerializeField] private Image bigSuitFancy;

    // ---- Flip Animation ----
    public enum FlipMode { ScaleX, RotateY }
    [Header("Flip Animation")]
    [SerializeField] private FlipMode flipMode = FlipMode.ScaleX;
    [SerializeField] private float flipDuration = 1.5f;
    [SerializeField] private Ease easeIn = Ease.InCubic;   
    [SerializeField] private Ease easeOut = Ease.OutCubic; 
    [SerializeField] private bool blockRaycastsDuringFlip = true;

    Tween flipTween;
    Vector3 initialLocalScale; 

    // ---- Runtime state ----
    public Suit Suit { get; private set; }
    public Rank Rank { get; private set; }
    public bool IsFaceUp { get; private set; } = true;

    CanvasGroup cg; 

    void Awake()
    {
        initialLocalScale = transform.localScale;
        cg = GetComponent<CanvasGroup>();
        ApplyFace(IsFaceUp, true);
    }

    void OnEnable()
    {
       
        if (frontGroup && backGroup)
        {
            frontGroup.SetActive(true);
            backGroup.SetActive(false);
        }

       
        if (theme && rankText && (smallSuitSimple || bigSuitSimple))
        {
            if (string.IsNullOrEmpty(rankText.text))
            {
                SetCard(Suit.Spades, Rank.Ace, keepFaceState: true);
            }
        }
    }

    public void SetCard(Suit suit, Rank rank, bool keepFaceState = false)
    {
        if (!theme)
        {
            Debug.LogWarning("CardView: theme (CardThemeSO) atanmamış.");
            return;
        }

        Suit = suit;
        Rank = rank;

        
        rankText.text = RankToString(rank);
        bool isRed = (suit == Suit.Hearts || suit == Suit.Diamonds);
        rankText.color = isRed ? redColor : blackColor;

        int sIdx = (int)suit;
        bool fancy = CardThemeSO.IsFancy(rank); 

        if (fancy)
        {
            fancyGroup.SetActive(true);
            simpleGroup.SetActive(false);

            if (theme.suitSmall != null && theme.suitSmall.Length >= 4)
                smallSuitFancy.sprite = theme.suitSmall[sIdx];
            if (theme.suitBig != null && theme.suitBig.Length >= 4)
                bigSuitFancy.sprite = theme.suitBig[sIdx];

            
            var art = theme.GetArtwork(rank, suit);
            if (art) bigSuitFancy.sprite = art;

            
            if (!art)
            {
                fancyGroup.SetActive(false);
                simpleGroup.SetActive(true);
                ApplySimpleSuit(sIdx);
            }

            smallSuitFancy.preserveAspect = true;
            bigSuitFancy.preserveAspect = true;
        }
        else
        {
            fancyGroup.SetActive(false);
            simpleGroup.SetActive(true);
            ApplySimpleSuit(sIdx);
        }

        if (!keepFaceState)
            SetFaceUp(IsFaceUp, instant: true); 
    }

    
    public void FlipOpen(float? duration = null)  => SetFaceUp(true,  instant:false, duration);
    public void FlipClose(float? duration = null) => SetFaceUp(false, instant:false, duration);
    public void FlipToggle(float? duration = null)=> SetFaceUp(!IsFaceUp, instant:false, duration);

    public void SetFaceUp(bool up, bool instant = true, float? durationOverride = null)
    {
        if (instant)
        {
            KillFlip();
            ApplyFace(up, true);
            return;
        }

        if (IsFaceUp == up)
        {
            
            ApplyFace(up, true);
            return;
        }

        KillFlip();

        float dur = durationOverride ?? flipDuration;

        
        if (blockRaycastsDuringFlip && cg)
        {
            cg.blocksRaycasts = false;
            cg.interactable = false;
        }

        if (flipMode == FlipMode.ScaleX)
        {
            
            transform.localScale = initialLocalScale;
            flipTween = DOTween.Sequence()
                .Append(transform.DOScaleX(0f, dur * 0.5f).SetEase(easeIn))
                .AppendCallback(() => ApplyFace(up, true))
                .Append(transform.DOScaleX(Mathf.Abs(initialLocalScale.x), dur * 2f).SetEase(easeOut))
                .OnComplete(UnblockRaycasts);
        }
        else 
        {
            var startRot = transform.localEulerAngles;
            flipTween = DOTween.Sequence()
                .Append(transform.DOLocalRotate(new Vector3(startRot.x, 90f, startRot.z), dur * 0.5f).SetEase(easeIn))
                .AppendCallback(() => ApplyFace(up, true))
                .Append(transform.DOLocalRotate(new Vector3(startRot.x, 0f, startRot.z), dur * 0.5f).SetEase(easeOut))
                .OnComplete(UnblockRaycasts);
        }
    }

    // --- Helpers ---
    void ApplyFace(bool faceUp, bool instant)
    {
        IsFaceUp = faceUp;
        if (frontGroup) frontGroup.SetActive(faceUp);
        if (backGroup)  backGroup.SetActive(!faceUp);

        if (instant)
        {
            
            if (flipMode == FlipMode.ScaleX)
                transform.localScale = new Vector3(Mathf.Abs(initialLocalScale.x), initialLocalScale.y, initialLocalScale.z);
            else
                transform.localEulerAngles = new Vector3(0,0,0);
        }
    }

    void UnblockRaycasts()
    {
        if (blockRaycastsDuringFlip && cg)
        {
            cg.blocksRaycasts = true;
            cg.interactable = true;
        }
        flipTween = null;
    }

    void KillFlip()
    {
        if (flipTween != null && flipTween.IsActive()) flipTween.Kill();
        flipTween = null;
    }

    private void ApplySimpleSuit(int sIdx)
    {
        if (theme.suitSmall != null && theme.suitSmall.Length >= 4)
            smallSuitSimple.sprite = theme.suitSmall[sIdx];
        if (theme.suitBig != null && theme.suitBig.Length >= 4)
            bigSuitSimple.sprite = theme.suitBig[sIdx];

        smallSuitSimple.preserveAspect = true;
        bigSuitSimple.preserveAspect = true;
    }

    private string RankToString(Rank r)
    {
        switch (r)
        {
            case Rank.Ace: return "A";
            case Rank.Jack: return "J";
            case Rank.Queen: return "Q";
            case Rank.King: return "K";
            default: return ((int)r).ToString();
        }
    }
}
