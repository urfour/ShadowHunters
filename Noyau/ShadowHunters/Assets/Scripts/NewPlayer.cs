using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewPlayer : MonoBehaviour
{
    public GameObject playerCards;
    public List<Character> characterCards; 
    private Player player;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Un nouveau joueur s'est connecté.");
        int i = Random.Range(0, characterCards.Count - 1);
        player = new Player(characterCards[i].name, characterCards[i].team, characterCards[i].characterHP);
        characterCards.RemoveAt(i);
        transform.GetComponent<GivePlayerCard>().GiveCard(i);
    }

}
