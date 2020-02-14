using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;

public class DrawCard : MonoBehaviour
{
    public GameObject cardPrefab; // Prefab d'une carte
    public List<Card> cards = new List<Card>(); // Différentes cartes possibles du deck
    public List<Card> deck; // Deck qui sera initialisé
    public GameObject playerCards; // Zone du joueur
    public List<Card> usedCards; // Cartes défaussées
    public GameObject discardPile; // Pile de défausse

    private SpriteRenderer spriteRenderer;

    void Start()
    {
        for (int i = 0; i < 50; i++)
        {
            deck.Add(cards[i%cards.Count]);
        }
        deck.Shuffle<Card>();
        // Il faudra ajouter les différents types de cartes
    }

    public IEnumerator Draw()
    {
        Card pickedCard = deck[0];
        deck.RemoveAt(0);
        GameObject playerCard = Instantiate(cardPrefab, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
        playerCard.GetComponent<SpriteRenderer>().sprite = pickedCard.sprite;
        playerCard.transform.localScale = new Vector2(1, 1);
        playerCard.transform.SetParent(playerCards.transform, false);
        playerCard.GetComponent<CardEvent>().UsePower(pickedCard);
        yield return new WaitForSeconds(3f);
        usedCards.Add(pickedCard);
        playerCard.transform.SetParent(discardPile.transform, false);
        playerCard.transform.rotation = Quaternion.Euler(0f, 0f, 90f);
        Debug.Log("L'effet a été utilisé, retour à la défausse.");
    }
    public void OnClick()
    {
        StartCoroutine(Draw());
    }
}

public static class IListExtensions
{
    /// <summary>
    /// Shuffles the element order of the specified list.
    /// </summary>
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
}
