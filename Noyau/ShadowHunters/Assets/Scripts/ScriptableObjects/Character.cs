using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Character", menuName = "Card/Character", order = 1)]
public class Character : ScriptableObject
{
    public string characterName;
    public int characterHP;
    public string characterPower;
    public string characterVictoryCondition;
}
