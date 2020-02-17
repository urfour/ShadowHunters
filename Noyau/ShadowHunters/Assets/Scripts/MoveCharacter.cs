using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCharacter : MonoBehaviour
{
    public List<Card> locationCards; // Cartes lieu
    public void Move()
    {
        int lancer1 = Random.Range(1, 6);
        int lancer2 = Random.Range(1, 4);
        int lancerTotal = lancer1 + lancer2;
        Debug.Log("Le lancer de dés donne " + lancer1 + " et " + lancer2 + " (" + lancerTotal + ").");
        switch (lancerTotal)
        {
            case 2:
            case 3:
                break;
            case 4:
            case 5:
                break;
        }

    }
}
