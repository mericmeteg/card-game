using UnityEngine;
using System.Collections.Generic;

[ExecuteAlways]
[RequireComponent(typeof(RectTransform))]
public class PileUI : MonoBehaviour
{
    [SerializeField] private Pile pile;
    [SerializeField] private float yOffsetFaceDown = 18f;
    [SerializeField] private float yOffsetFaceUp = 28f;
    [SerializeField] private bool alignFromTop = true;
    [SerializeField] private bool relayoutOnChildChanged = true;
    [Header("X Layout (columns)")]
    [SerializeField] bool autoPlaceX = true;   // Açık ise Awake/OnValidate'te otomatik uygular
    [SerializeField] float cardWidth = 158f;
    [SerializeField] float gapX = 10f;
    List<RectTransform> childRectT = new List<RectTransform>();
    List<CardView> childCardV = new List<CardView>();


    RectTransform rect;
    void Awake()
    {
        rect = GetComponent<RectTransform>();
        if (!pile) pile = GetComponent<Pile>();
        AnchorTopCenter(rect);
        if (autoPlaceX) ApplyX();
    }

    void Start()
    {
        RebuildChildCache();
        RelayoutFromHierarchy();
    }

    void OnValidate()
    {
        if (autoPlaceX && !Application.isPlaying) ApplyX();
        if (!rect) rect = GetComponent<RectTransform>();
        if (!Application.isPlaying) RelayoutFromHierarchy();
    }

    void OnTransformChildrenChanged()
    {
        if (relayoutOnChildChanged)
        {
            RebuildChildCache();
            RelayoutFromHierarchy();
        }
    }

    void OnRectTransformDimensionsChange()
    {
            if (autoPlaceX) ApplyX();
            Relayout();
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
        RebuildChildCache();
        Relayout();
    }


    private void RebuildChildCache()
    {
        childRectT.Clear();
        childCardV.Clear();

        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i) is RectTransform rt)
            {
                childRectT.Add(rt);
                if (rt.TryGetComponent(out CardView cv))
                    childCardV.Add(cv);
                else
                    childCardV.Add(null);
            }
        }
    }

    public void Relayout()
    {
        float y = 0f;

        for (int i = 0; i < childRectT.Count; i++)
        {
            var t = childRectT[i];
            var cv = childCardV[i];

            AnchorTopCenter(t);

            bool isFaceUp = cv ? cv.IsFaceUp : false;
            float step = isFaceUp ? yOffsetFaceUp : yOffsetFaceDown;

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
        if (rt) rt.SetParent(transform, false);
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

        int columns = 0, idx = 0;
        int run = 0;

        foreach (Transform ch in transform.parent)
        {
            if (!ch.TryGetComponent<PileUI>(out _)) continue; // TryGetComponent gereksiz allocation yapmamak için
            if (ch == transform) idx = run;
            run++;
        }
        columns = run;

        float spacing = cardWidth + gapX;
        float startX = -((columns - 1) * spacing) * 0.5f;

        var rt = (RectTransform)transform;
        AnchorTopCenter(rt);
        var p = rt.anchoredPosition;
        p.x = startX + idx * spacing;
        rt.anchoredPosition = p;
    }


}