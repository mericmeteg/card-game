using System;
using System.Collections.Generic;
using UnityEngine;

public class Pile : MonoBehaviour
{
    private readonly List<CardView> cards = new List<CardView>();

    public int Count => cards.Count;
    public Transform TopCardTransform => transform.childCount > 0 ? transform.GetChild(transform.childCount - 1) : null;
    public bool IsTop(Transform card) => card != null && card == TopCardTransform;

    public CardView Top
    {
        get
        {
            
            for (int i = cards.Count - 1; i >= 0; i--)
            {
                if (cards[i] == null) cards.RemoveAt(i);
                else return cards[i];
            }
            return null;
        }
    }

    public IReadOnlyList<CardView> Cards => cards;

    
    public event Action OnChanged;

    public bool Contains(CardView card) => cards.Contains(card);

    public void Clear()
    {
        if (cards.Count == 0) return;
        cards.Clear();
        OnChanged?.Invoke();
    }

    public bool AddTop(CardView card)
    {
        if (!card) return false;
        if (cards.Contains(card)) return false; 
        cards.Add(card);
        OnChanged?.Invoke();
        return true;
    }

    public CardView PopTop()
    {
        for (int i = cards.Count - 1; i >= 0; i--)
        {
            var c = cards[i];
            cards.RemoveAt(i);
            if (c != null)
            {
                OnChanged?.Invoke();
                return c;
            }
        }
        return null;
    }

    public bool Remove(CardView card)
    {
        if (!card) return false;
        bool removed = cards.Remove(card);
        if (removed) OnChanged?.Invoke();
        return removed;
    }

    public void SetCards(IEnumerable<CardView> newCards)
    {
        cards.Clear();
        if (newCards != null)
        {
            var seen = new HashSet<CardView>();
            foreach (var c in newCards)
            {
                if (c != null && seen.Add(c)) cards.Add(c);
            }
        }
        OnChanged?.Invoke();
    }
    public void RevealTopIfNeeded()
{
    var top = TopCardTransform;
    if (!top) return;
    var cv = top.GetComponent<CardView>();
    if (cv && !cv.IsFaceUp) cv.FlipOpen();
}
}
