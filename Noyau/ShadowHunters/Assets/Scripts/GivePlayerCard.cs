using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GivePlayerCard : MonoBehaviour
{
    public List<GameObject> characterCards = new List<GameObject>();
    public GameObject playerCards;
    // Start is called before the first frame update
    void Start()
    {
        GameObject playerCard = Instantiate(characterCards[Random.Range(0, characterCards.Count)], new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
        playerCard.transform.SetParent(playerCards.transform, false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
