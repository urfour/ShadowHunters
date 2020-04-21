using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CardType { Location, Vision, Light, Darkness }

/// <summary>
/// Définition d'une carte
/// (les cartes Lieu utilisent cette implémentation)
/// </summary>
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