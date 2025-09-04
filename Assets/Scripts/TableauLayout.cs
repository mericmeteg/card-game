using System.Collections.Generic;
using UnityEngine;

public class TableauLayout : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] CardView cardPrefab;     
    [SerializeField] PileUI[] piles;          
    [SerializeField] Transform cardsParent;   

    
    readonly List<(Suit suit, Rank rank)> deck = new();
    bool dealt;

    void Awake()
    {
        
        if (piles == null || piles.Length == 0)
        {
            var tmp = new List<PileUI>();
            foreach (Transform t in transform)
            {
                var p = t.GetComponent<PileUI>();  
                if (p) tmp.Add(p);
            }
            piles = tmp.ToArray(); 
        }
    }

    void Start()
    {
        if (!dealt)
        {
            BuildDeck52();   
            Shuffle(deck);   
            DealTableau28(); 
            dealt = true;
        }
    }

    void BuildDeck52()
    {
        deck.Clear();
        foreach (Suit s in System.Enum.GetValues(typeof(Suit)))
            foreach (Rank r in System.Enum.GetValues(typeof(Rank)))
                deck.Add((s, r));
    }

    void Shuffle(List<(Suit, Rank)> list)
    {
        var rng = new System.Random();
        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = rng.Next(i + 1);
            (list[i], list[j]) = (list[j], list[i]);
        }
    }

    void DealTableau28()
    {
        if (piles == null || piles.Length < 7) { Debug.LogError("Tableau: 7 pile gerekli."); return; }
        if (!cardPrefab)                        { Debug.LogError("Tableau: cardPrefab atanmadı."); return; }

        int k = 0; // deste indeksi
        for (int col = 0; col < 7; col++)
        {
            var pileUI = piles[col];

            for (int row = 0; row <= col; row++)
            {
                var (suit, rank) = deck[k++];
                var cv = Instantiate(cardPrefab, cardsParent ? cardsParent : pileUI.transform);
                cv.name = $"Card_{rank}_{suit}";
                cv.SetCard(suit, rank, keepFaceState: true);

                bool faceUp = (row == col); // üst açık, alttakiler kapalı
                pileUI.AddCard(cv, faceUp, worldPositionStays: false); // SetParent(false) önemli
            }
        }
    }
}
