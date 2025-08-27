using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
public class DraggableCardUI : MonoBehaviour,
    IBeginDragHandler, IDragHandler, IEndDragHandler
{
    RectTransform rect;
    Canvas       canvas;
    CanvasGroup  cg;
    LayoutElement layoutEl;

    Transform  originalParent;
    int        originalSiblingIndex;
    Vector2    originalAnchoredPos;
    Vector2    originalAnchorMin, originalAnchorMax, originalPivot;
    Quaternion originalLocalRot;
    Vector3    originalLocalScale;

    [Header("Drag")]
    [SerializeField] Transform dragLayer;     
    [SerializeField] bool onlyTopAndFaceUp = true;

    Pile sourcePile;
    [HideInInspector] public bool droppedOnTarget = false;

    void Awake()
    {
        rect     = GetComponent<RectTransform>();
        canvas   = GetComponentInParent<Canvas>();
        cg       = GetComponent<CanvasGroup>() ?? gameObject.AddComponent<CanvasGroup>();
        layoutEl = GetComponent<LayoutElement>();
        if (!dragLayer && canvas) dragLayer = canvas.transform;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left) return;

        

        sourcePile = GetComponentInParent<Pile>();
        if (onlyTopAndFaceUp)
        {
            if (!sourcePile || !sourcePile.IsTop(transform)) { eventData.Use(); return; }
            var cv = GetComponent<CardView>();
            if (cv && !cv.IsFaceUp) { eventData.Use(); return; }
        }

        droppedOnTarget = false;

        // Orijinali kaydet
        originalParent       = rect.parent;
        originalSiblingIndex = rect.GetSiblingIndex();
        originalAnchoredPos  = rect.anchoredPosition;
        originalAnchorMin    = rect.anchorMin;
        originalAnchorMax    = rect.anchorMax;
        originalPivot        = rect.pivot;
        originalLocalRot     = rect.localRotation;
        originalLocalScale   = rect.localScale;

        if (layoutEl) layoutEl.ignoreLayout = true;
        cg.blocksRaycasts = false;

       
        rect.SetParent(dragLayer, true);
        rect.SetAsLastSibling();

    
        rect.anchorMin = rect.anchorMax = new Vector2(0.5f, 0.5f);
        rect.pivot     = new Vector2(0.5f, 0.5f);

        rect.localScale    = originalLocalScale;
        rect.localRotation = Quaternion.identity;

        
        Vector3 worldPoint;
        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(
            (RectTransform)dragLayer, eventData.position, eventData.pressEventCamera, out worldPoint))
        {
            rect.position = worldPoint; 
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left) return;
        if (!canvas) return;

        
        Vector3 worldPoint;
        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(
            (RectTransform)dragLayer, eventData.position, eventData.pressEventCamera, out worldPoint))
        {
            rect.position = worldPoint;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        cg.blocksRaycasts = true;
        if (layoutEl) layoutEl.ignoreLayout = false;

        if (!droppedOnTarget)
        {
           
            rect.SetParent(originalParent, false);

            rect.anchorMin       = originalAnchorMin;
            rect.anchorMax       = originalAnchorMax;
            rect.pivot           = originalPivot;
            rect.localRotation   = originalLocalRot;
            rect.localScale      = originalLocalScale;
            rect.SetSiblingIndex(originalSiblingIndex);
            rect.anchoredPosition = originalAnchoredPos;
        }
        else
        {
           
            rect.localScale = originalLocalScale;

        
            if (sourcePile != null) sourcePile.RevealTopIfNeeded();
        }

        droppedOnTarget = false;
        sourcePile = null;
    }
}
