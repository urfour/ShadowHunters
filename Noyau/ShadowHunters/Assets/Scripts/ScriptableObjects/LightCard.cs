using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LightEffect
{
    Amulette,
    Ange,
    Benediction,
    Boussole,
    Broche,
    Barre,
    Crucifix,
    EauBenite,
    Eclair,
    Lance,
    Miroir,
    PremiersSecours,
    Savoir,
    Avenement,
    Toge
}

[CreateAssetMenu(fileName = "LightCard", menuName = "Card/LightCard", order = 5)]
public class LightCard : Card
{
    public LightEffect lightEffect;
}
