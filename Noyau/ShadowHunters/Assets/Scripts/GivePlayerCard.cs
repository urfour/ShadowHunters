using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GivePlayerCard : MonoBehaviour
{
    public List<Character> characterCards; // liste des cartes possibles
    public GameObject cardPrefab; // prefab d'une carte
    public GameObject playerCards; // zone du joueur

    public void GiveCard(int i)
    {
        GameObject playerCard = Instantiate(cardPrefab, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
        playerCard.GetComponent<SpriteRenderer>().sprite = characterCards[i].sprite;
        playerCard.transform.localScale = new Vector2(1, 1);
        playerCard.transform.SetParent(playerCards.transform, false);
        Debug.Log("Vous êtes " + characterCards[i].characterName + ", vous êtes donc dans l'équipe des " + characterCards[i].team + "s.");
    }
}
