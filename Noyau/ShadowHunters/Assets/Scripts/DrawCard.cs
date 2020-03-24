/*using System.Collections;
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
        PrepareDecks();
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
        if (deck.Count == 0)
            ResetDecks();
    }

    public void PrepareDecks()
    {
        for (int i = 0; i < cards.Count; i++)
            deck.Add(cards[i]);
        deck.Shuffle<Card>();
        // Il faudra ajouter les différents types de cartes
    }

    public void ResetDecks()
    {
        Debug.Log("Le deck est vide, redistribution des cartes.");
        for (int i = 0; i < usedCards.Count; i++)
            usedCards.RemoveAt(i);
        PrepareDecks();
        foreach (Transform child in discardPile.transform)
            Destroy(child.gameObject);
    }
    public void OnClick()
    {
        StartCoroutine(Draw());
    }
}
*/