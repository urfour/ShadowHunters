using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DarknessEffect
{
    Araignee,
    ChauveSouris,
    Dynamite,
    Hache,
    Mitrailleuse,
    Banane,
    Poupee,
    Revolver,
    Rituel,
    Sabre,
    Succube
}

[CreateAssetMenu(fileName = "DarknessCard", menuName = "Card/DarknessCard", order = 4)]
public class DarknessCard : Card
{
    public DarknessEffect darknessEffect;
}
