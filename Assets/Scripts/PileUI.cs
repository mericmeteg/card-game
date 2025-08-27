using UnityEngine;

[ExecuteAlways]
[RequireComponent(typeof(RectTransform))]
public class PileUI : MonoBehaviour
{
    [SerializeField] private Pile pile;
    [SerializeField] private float yOffsetFaceDown = 18f;
    [SerializeField] private float yOffsetFaceUp = 28f;
    [SerializeField] private bool alignFromTop = true;   // şimdilik hep üstten dizeceğiz
    [SerializeField] private bool relayoutOnChildChanged = true;
    [Header("X Layout (columns)")]
    [SerializeField] bool  autoPlaceX = true;   // Açık ise Awake/OnValidate'te otomatik uygular
    [SerializeField] float cardWidth  = 158f;   // Kartının RectTransform genişliği
    [SerializeField] float gapX       = 10f;

    RectTransform rect;

    void Awake()
    {
        rect = GetComponent<RectTransform>();
        if (!pile) pile = GetComponent<Pile>();
        AnchorTopCenter(rect);
        if (autoPlaceX) ApplyX();
    }

    void Start() { RelayoutFromHierarchy(); }

    void OnValidate()
    {
        if (autoPlaceX && !Application.isPlaying) ApplyX();
        if (!rect) rect = GetComponent<RectTransform>();
        if (!Application.isPlaying) RelayoutFromHierarchy();
    }

    void OnTransformChildrenChanged()
    {
        if (relayoutOnChildChanged) RelayoutFromHierarchy();
    }

    void OnRectTransformDimensionsChange()
    {
        if (autoPlaceX) ApplyX();
        Relayout(); // çözünürlük değişiminde yeniden hizala
    }

    public void RelayoutFromHierarchy()
    {
        if (!pile) { Relayout(); return; }

        var list = new System.Collections.Generic.List<CardView>();
        for (int i = 0; i < transform.childCount; i++)
        {
            var cv = transform.GetChild(i).GetComponent<CardView>();
            if (cv) list.Add(cv);
        }
        pile.SetCards(list);
        Relayout();
    }

    public void Relayout()
{
    float y = 0f;

    for (int i = 0; i < transform.childCount; i++)
    {
        var t = transform.GetChild(i) as RectTransform;
        if (!t) continue;

        // üst-ortadan hizala
        AnchorTopCenter(t);

        var cv = t.GetComponent<CardView>();
        bool isFaceUp = cv ? cv.IsFaceUp : false;

        float step = isFaceUp ? yOffsetFaceUp : yOffsetFaceDown;

        // ÜSTTEN diz
        float ay = alignFromTop ? -y : y;
        t.anchoredPosition = new Vector2(0f, ay);

        y += step;
    }
}


    public void AddCard(CardView card, bool faceUp, bool worldPositionStays = false)
    {
        if (!card) return;
        if (pile != null) pile.AddTop(card);

        var rt = card.transform as RectTransform;
        if (rt) rt.SetParent(transform, false);         // worldPositionStays = false ÖNEMLİ
        else card.transform.SetParent(transform, false);

        card.SetFaceUp(faceUp, true);
        Relayout();
    }

    public CardView PopTop(bool destroy = false)
    {
        if (transform.childCount == 0) return null;

        CardView top = (pile != null)
            ? pile.PopTop()
            : transform.GetChild(transform.childCount - 1).GetComponent<CardView>();

        if (!top) return null;

        if (destroy) Destroy(top.gameObject);
        else top.transform.SetParent(null, false);

        Relayout();
        return top;
    }

    public void RevealTopIfHidden()
    {
        if (transform.childCount == 0) return;
        var top = transform.GetChild(transform.childCount - 1).GetComponent<CardView>();
        if (top && !top.IsFaceUp)
        {
            top.SetFaceUp(true, true);
            Relayout();
        }
    }

    static void AnchorTopCenter(RectTransform rt)
    {
        rt.anchorMin = new Vector2(0.5f, 1f);
        rt.anchorMax = new Vector2(0.5f, 1f);
        rt.pivot = new Vector2(0.5f, 1f);
    }
    [ContextMenu("Apply X Now")]
public void ApplyX()
{
    if (!transform.parent) return;

    // Kardeşler içinde PileUI olanların sayısı ve bu Pile’ın index’i
    int columns = 0, idx = 0, run = 0;
    foreach (Transform ch in transform.parent)
        if (ch.GetComponent<PileUI>()) columns++;

    foreach (Transform ch in transform.parent)
    {
        if (!ch.GetComponent<PileUI>()) continue;
        if (ch == transform) { idx = run; break; }
        run++;
    }

    float spacing = cardWidth + gapX;                   // aralığı buradan kontrol edersin
    float startX  = -((columns - 1) * spacing) * 0.5f;  // ortala

    var rt = (RectTransform)transform;
    AnchorTopCenter(rt); // zaten sınıfında var
    var p = rt.anchoredPosition;
    p.x   = startX + idx * spacing;                     // sadece X’i güncelle
    rt.anchoredPosition = p;
}
}
