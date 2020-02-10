using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DrawCard : MonoBehaviour
{
    public GameObject cardPrefab; // Prefab d'une carte
    public List<Card> cards = new List<Card>(); // Différentes cartes possibles du deck
    public List<Card> deck; // Deck qui sera initialisé
    public GameObject playerCards; // Zone du joueur

    private SpriteRenderer spriteRenderer;

    void Start()
    {
        for (int i = 0; i < 50; i++)
        {
            deck.Add(cards[i % 2]);
        }
        // Il faudra ajouter les différents types de cartes lumière
    }
    public void OnClick()
    {
        Card pickedCard = cards[0];
        cards.Remove(cards[0]);
        GameObject playerCard = Instantiate(cardPrefab, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
        playerCard.GetComponent<SpriteRenderer>().sprite = pickedCard.sprite;
        playerCard.transform.SetParent(playerCards.transform, false);
        //StartCoroutine(playerCard.GetComponent<CardEvent>().UsePower());
    }
}
