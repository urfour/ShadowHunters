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
    public bool effectTakeWounds; // Subir x Blessures
    public int nbWounds; // Nombre de Blessures subies
}

/// <summary>
/// Définition des cartes Vision
/// </summary>
[CreateAssetMenu(fileName = "VisionCard", menuName = "Card/VisionCard", order = 3)]
[System.Serializable]
public class VisionCard : Card
{
    public VisionEffect visionEffect;
}
