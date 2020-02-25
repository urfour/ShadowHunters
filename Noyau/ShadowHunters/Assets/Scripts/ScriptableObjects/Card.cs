using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CardType { Location, Vision, Light, Darkness }

public enum Position
{
    Antre,
    Cimetiere,
    Foret,
    Monastere,
    Porte,
    Sanctuaire
}

[CreateAssetMenu(fileName = "Card", menuName = "Card/Card", order = 2)]
[System.Serializable]
public class Card : ScriptableObject
{
    public string cardName;
    public CardType cardType;
    public bool isEquipement;
    public string description;
    public Sprite sprite;
    public bool isHidden;
}


