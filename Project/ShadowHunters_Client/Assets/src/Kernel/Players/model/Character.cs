using Assets.Noyau.Players.model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Définition d'un personnage
/// </summary>

//[CreateAssetMenu(fileName = "Character", menuName = "Card/Character", order = 1)]
public class Character
{
    public readonly string characterName;
    public readonly CharacterTeam team;
    public readonly int characterHP;
    public readonly Power power;
    public Goal goal;

    /// <summary>
    /// Constructeur d'un personnage.
    /// </summary>
    /// <param name="characterName">Nom du personnage</param>
    /// <param name="team">Equipe du personnage</param>
    /// <param name="characterHP">Ses points de vie</param>
    /// <param name="goal">Ses conditions de victoires</param>
    /// <param name="power">Son pouvoir</param>
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
    Neutral,
    None,
}