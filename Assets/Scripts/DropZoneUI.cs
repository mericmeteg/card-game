using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Pile))]
public class DropZone : MonoBehaviour, IDropHandler
{
    [Header("Snap ayarı")]
    [SerializeField] float yNudge = 0f; 

    Pile pile;
    PileUI pileUI;
    DraggableCardUI draggableCard;

    void Awake()
    {
        pile = GetComponent<Pile>();
        pileUI = GetComponent<PileUI>();
    } 

    public void OnDrop(PointerEventData eventData)
    {
        var draggable = eventData.pointerDrag?.GetComponent<DraggableCardUI>();
        if (!draggable) return;

        var r = (RectTransform)draggable.transform;
        draggable.droppedOnTarget = true;

      
        r.SetParent(pile.transform, worldPositionStays: false);

       
        r.anchorMin = r.anchorMax = new Vector2(0.5f, 1f); 
        r.pivot     = new Vector2(0.5f, 1f);              
        r.localRotation = Quaternion.identity;
        r.localScale    = Vector3.one;
        r.SetAsLastSibling();
        r.anchoredPosition = new Vector2(0f, yNudge);
        pileUI?.RelayoutFromHierarchy();
        pile.RevealTopIfNeeded();
    }
}
