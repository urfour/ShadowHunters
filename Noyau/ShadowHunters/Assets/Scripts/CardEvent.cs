using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardEvent : MonoBehaviour
{

    public void UsePower(Card card)
    {
        Debug.Log("Carte : " + card.cardName);
        Debug.Log("Effet : " + card.description);
    }

}
