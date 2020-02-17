using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Character", menuName = "Card/Character", order = 1)]
public class Character : ScriptableObject
{
    public string characterName;
    public CharacterTeam team;
    public int characterHP;
    public string characterPower;
    public string characterVictoryCondition;
    public Sprite sprite;
}

public enum CharacterTeam
{
    Shadow,
    Hunter,
    Neutral
}
