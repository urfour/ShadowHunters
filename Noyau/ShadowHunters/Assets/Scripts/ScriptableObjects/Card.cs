using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Card", menuName = "Card/Card", order = 2)]
public class Card : ScriptableObject
{
    public enum CardType
    {
        Location,
        Vision,
        Light,
        Darkness
    }

    public enum CardEffect
    {

    }

    public string cardName;
    public CardType cardType;
    public bool isEquipement;
    public string description;
    public CardEffect cardEffect;
    public Sprite sprite;
    public bool isHidden;

}

public enum Position
{
    Antre,
    Cimetiere,
    Foret,
    Monastere,
    Porte,
    Sanctuaire
}


