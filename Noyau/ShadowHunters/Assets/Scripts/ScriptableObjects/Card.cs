using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct VisionEffect
{
    // Types de cartes vision
    public bool effectOnShadow; // "Je pense que tu es Shadow"
    public bool effectOnHunter; // "Je pense que tu es Hunter"
    public bool effectOnNeutral; // "Je pense que tu es Neutre"
    public bool effectOnLowHP; // "Je pense que tu es un personnage de 11 Points de vie ou moins"
    public bool effectOnHighHP; // "Je pense que tu es un personnage de 12 Points de vie ou plus"

    // Effet des cartes
    public bool effectSupremeVision; // Carte Vision Suprême
    public bool effectGivingEquipementCard; // Donner une carte équipement ou subir 1 Blessure
    public bool effectHealingOneWound; // Soigner 1 Blessure
    public bool effectOneWound; // Subir 1 Blessure
    public bool effectTwoWounds; // Subir 2 Blessures
}

public enum CardType
{
    Location,
    Vision,
    Light,
    Darkness
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


