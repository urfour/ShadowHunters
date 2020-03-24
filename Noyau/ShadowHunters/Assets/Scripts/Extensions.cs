using System.Collections;
using System.Collections.Generic;

public static class IListExtensions
{
    public static void Shuffle<T>(this List<T> ts)
    {
        var count = ts.Count;
        var last = count - 1;
        for (var i = 0; i < last; ++i)
        {
            var r = UnityEngine.Random.Range(i, count);
            var tmp = ts[i];
            ts[i] = ts[r];
            ts[r] = tmp;
        }
    }

    public static List<T> PrepareDecks<T>(this List<T> ts)
    {
        List<T> deck = new List<T>();
        foreach (T card in ts)
            deck.Add(card);
        deck.Shuffle<T>();
        return deck;
    }

}