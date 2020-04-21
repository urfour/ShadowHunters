using Assets.Noyau.Players.model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(fileName = "Character", menuName = "Card/Character", order = 1)]
public class Character
{
    public readonly string characterName;
    public readonly CharacterTeam team;
    public readonly int characterHP;
    public readonly Power power;
    public readonly Goal goal;

    public Character(string characterName, CharacterTeam team, int characterHP, Goal goal, Power power)
    {
        this.characterName = characterName;
        this.team = team;
        this.characterHP = characterHP;
        this.goal = goal;
        this.power = power;
    }
}

public enum CharacterTeam
{
    Shadow,
    Hunter,
    Neutral
}