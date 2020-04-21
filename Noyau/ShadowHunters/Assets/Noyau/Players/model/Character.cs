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
    public readonly CheckWinningCondition characterWinningCondition;
    public readonly SetWinningListeners setWinningListeners;
    public readonly Power power;

    public Character(string characterName, CharacterTeam team, int characterHP, CheckWinningCondition characterWinningCondition, SetWinningListeners setWinningListeners, Power power)
    {
        this.characterName = characterName;
        this.team = team;
        this.characterHP = characterHP;
        this.characterWinningCondition = characterWinningCondition;
        this.setWinningListeners = setWinningListeners;
        this.power = power;
    }
}

public enum CharacterTeam
{
    Shadow,
    Hunter,
    Neutral
}