using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCharacter : MonoBehaviour
{
    public List<Card> locationCards; // Cartes lieu
    private Player player;
    public void Move()
    {
        int lancer1 = Random.Range(1, 6);
        int lancer2 = Random.Range(1, 4);
        int lancerTotal = lancer1 + lancer2;
        Card carteLieu = null;
        Debug.Log("Le lancer de dés donne " + lancer1 + " et " + lancer2 + " (" + lancerTotal + ").");
        switch (lancerTotal)
        {
            case 2:
            case 3:
                player.Position = Position.Antre;
                carteLieu = GetCardByName(locationCards, "Antre de l'Ermite");
                break;
            case 4:
            case 5:
                player.Position = Position.Porte;
                carteLieu = GetCardByName(locationCards, "Porte de l'Outremonde");
                break;
            case 6:
                player.Position = Position.Monastere;
                carteLieu = GetCardByName(locationCards, "Monastère");                                                                                                                              
                break;
            case 7:
                Debug.Log("Où souhaitez-vous aller ?");
                player.Position = Position.Antre;
                carteLieu = GetCardByName(locationCards, "Antre de l'Ermite");
                Debug.Log("Choix par défaut");
                break;
            case 8:
                player.Position = Position.Cimetiere;
                carteLieu = GetCardByName(locationCards, "Cimetière");
                break;
            case 9:
                player.Position = Position.Foret;
                carteLieu = GetCardByName(locationCards, "Forêt Hantée");
                break;
            case 10:
                player.Position = Position.Sanctuaire;
                carteLieu = GetCardByName(locationCards, "Sanctuaire Ancien");
                break;
        }
        Debug.Log("Vous vous rendez donc sur la carte " + carteLieu.cardName + ".");

    }

    public Card GetCardByName(List<Card> liste, string name)
    {
        foreach (Card c in liste)
            if (c.name == name)
            {
                Debug.Log("Carte trouvée : " + c.cardName);
                return c;
            }

        return null;
    }

}
