using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GivePlayerCard : MonoBehaviour
{
    public List<Character> characterCards;
    public GameObject cardPrefab;
    public GameObject playerCards;
    // Start is called before the first frame update
    void Start()
    {
        GameObject playerCard = Instantiate(cardPrefab, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
        playerCard.GetComponent<SpriteRenderer>().sprite = characterCards[0].sprite;
        playerCard.transform.localScale = new Vector2(1, 1);
        playerCard.transform.SetParent(playerCards.transform, false);
        int i = Random.Range(0, characterCards.Count - 1);
        Debug.Log("Vous êtes " + characterCards[i].characterName + ", vous êtes donc dans l'équipe des " + characterCards[i].team + "s.");
    }
}
