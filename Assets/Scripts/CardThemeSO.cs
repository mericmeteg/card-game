using UnityEngine;

public enum Suit { Clubs, Diamonds, Hearts, Spades }
public enum Rank { Ace = 1, Two, Three, Four, Five, Six, Seven, Eight, Nine, Ten, Jack, Queen, King }

[CreateAssetMenu(fileName = "CardThemeSO", menuName = "ScriptableObjects/CardThemeSO", order = 1)]
public class CardThemeSO : ScriptableObject
{
    [Header("Suit Sprites (Index: 0 Clubs, 1 Diamonds, 2 Hearts, 3 Spades)")]
    public Sprite[] suitSmall = new Sprite[4];
    public Sprite[] suitBig   = new Sprite[4];

    [Header("Artwork Sprites for J/Q/K (each 4 sprites: Clubs, Diamonds, Hearts, Spades)")]
    public Sprite[] jackArt  = new Sprite[4];
    public Sprite[] queenArt = new Sprite[4];
    public Sprite[] kingArt  = new Sprite[4];

    public Sprite GetArtwork(Rank r, Suit s)
    {
        int i = (int)s;
        switch (r)
        {
            case Rank.Jack:  return Safe(jackArt,  i);
            case Rank.Queen: return Safe(queenArt, i);
            case Rank.King:  return Safe(kingArt,  i);
            default:         return null; // A ve 2..10 -> artwork yok (resimsiz)
        }
    }

    private Sprite Safe(Sprite[] arr, int idx)
    {
        if (arr == null || arr.Length < 4) return null;
        return arr[idx];
    }

    public static bool IsFancy(Rank r)
        => (r == Rank.Jack || r == Rank.Queen || r == Rank.King); // SADECE J/Q/K
}
