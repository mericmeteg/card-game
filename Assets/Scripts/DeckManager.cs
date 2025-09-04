using System.Collections.Generic;
using UnityEngine;

public class DeckManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private CardView cardPrefab;   
    [SerializeField] private PileUI[] tableauPiles; 
    [SerializeField] private Transform cardsParent; 

    
    private readonly List<(Suit suit, Rank rank)> deck = new();

    void Start()
    {
        BuildDeck52();
        Shuffle(deck);

        DealTableau28(); 
    }

    void BuildDeck52()
    {
        deck.Clear();
        // Sıfırdan 52’lik deste: 4 suit x 13 rank
        foreach (Suit s in System.Enum.GetValues(typeof(Suit)))
        {
            foreach (Rank r in System.Enum.GetValues(typeof(Rank)))
            {
                deck.Add((s, r));
            }
        }
    }

    // Fisher–Yates
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
        if (tableauPiles == null || tableauPiles.Length < 7)
        {
            Debug.LogError("DeckManager: tableauPiles 7 adet olmalı.");
            return;
        }
        if (!cardPrefab)
        {
            Debug.LogError("DeckManager: cardPrefab atanmamış.");
            return;
        }

        int deckIndex = 0;

        
        for (int col = 0; col < 7; col++)
        {
            var pileUI = tableauPiles[col];

            
            for (int row = 0; row <= col; row++)
            {
                var def = deck[deckIndex++];

                var cv = Instantiate(cardPrefab, cardsParent ? cardsParent : pileUI.transform);
                cv.name = $"Card_{def.rank}_{def.suit}";

                
                cv.SetCard(def.suit, def.rank, keepFaceState: true);

                
                bool faceUp = (row == col);

                
                pileUI.AddCard(cv, faceUp, worldPositionStays: false);
            }
        }

        
    }
}
