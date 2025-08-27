using System.Collections.Generic;
using UnityEngine;

public class DeckManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private CardView cardPrefab;   // tek kart prefab (Front/Back bağlı)
    [SerializeField] private PileUI[] tableauPiles; // 7 eleman: Pile 1..7 sırayla
    [SerializeField] private Transform cardsParent; // genelde Tableau (RectTransform)

    // (İstersen stok için kalan 24 kartı tutarız)
    private readonly List<(Suit suit, Rank rank)> deck = new();

    void Start()
    {
        BuildDeck52();
        Shuffle(deck);

        DealTableau28(); // 1-2-3-4-5-6-7 şekliyle toplam 28 kart
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

        // Sütun 0..6 (1..7 kart)
        for (int col = 0; col < 7; col++)
        {
            var pileUI = tableauPiles[col];

            // Bu sütuna col+1 adet kart
            for (int row = 0; row <= col; row++)
            {
                var def = deck[deckIndex++];
                // Kartı yarat
                var cv = Instantiate(cardPrefab, cardsParent ? cardsParent : pileUI.transform);
                cv.name = $"Card_{def.rank}_{def.suit}";

                // Kartın görsel verisini ata (ön/arka gruplar Inspector’da bağlı olmalı)
                cv.SetCard(def.suit, def.rank, keepFaceState: true);

                // En üst (son dağıtılan) açık, alttakiler kapalı
                bool faceUp = (row == col);

                // Kartı bu pile’a ekle ve hizalamayı pileUI yapsın
                pileUI.AddCard(cv, faceUp, worldPositionStays: false);
            }
        }

        // deckIndex şu an 28 — kalan 24 kartı istersen stock için saklayabilirsin.
        // Örn: remaining = deck.GetRange(deckIndex, deck.Count - deckIndex);
    }
}
