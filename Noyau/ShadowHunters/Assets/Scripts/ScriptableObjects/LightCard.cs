using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LightEffect
{
    Amulette,
    AngeGardien,
    Benediction,
    Boussole,
    Broche,
    Chocolat,
    Crucifix,
    EauBenite,
    Eclair,
    Lance,
    Miroir,
    PremiersSecours,
    Savoir,
    Supreme,
    Toge
}

[CreateAssetMenu(fileName = "LightCard", menuName = "Card/LightCard", order = 5)]
public class LightCard : Card
{
    public LightEffect lightEffect;
}
