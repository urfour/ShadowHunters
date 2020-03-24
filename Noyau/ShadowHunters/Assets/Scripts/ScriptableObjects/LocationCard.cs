using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Position
{
    None,
    Antre,
    Cimetiere,
    Foret,
    Monastere,
    Porte,
    Sanctuaire
}

[CreateAssetMenu(fileName = "LocationCard", menuName = "Card/LocationCard", order = 6)]
public class LocationCard : Card
{
    public Position area;
}
