using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Character", menuName = "Card/Character", order = 1)]
public class Character : ScriptableObject
{
    public string characterName;
    public CharacterTeam team;
    public int characterHP;
    public CharacterType characterType;
    public WinningCondition characterWinningCondition;
    public Sprite sprite;
}

public enum CharacterTeam
{
    Shadow,
    Hunter,
    Neutral
}

public enum CharacterType
{
    Allie,
    Bob,
    Bryan,
    David,
    Emi,
    Franklin,
    Georges,
    LoupGarou,
    Metamorphe,
    Vampire
}

public enum WinningCondition
{
    BeingAlive,
    HavingEquipement,
    Bryan,
    David,
    HunterCondition,
    ShadowCondition
}
