using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Card", menuName = "Card/Deck Card", order = 2)]
public class Card : ScriptableObject
{
    public enum CardType
    {
        Vision,
        Light,
        Darkness
    }
    public string cardName;
    public CardType cardType;
    public bool isEquipement;
    public string effect;
    public Sprite sprite;
}
