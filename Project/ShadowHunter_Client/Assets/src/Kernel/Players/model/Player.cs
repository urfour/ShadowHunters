﻿using System.Collections;
using System.Collections.Generic;
using Assets.Noyau.Cards.controller;
using Assets.Noyau.Cards.model;
using Assets.Noyau.Players.model;
using EventSystem;
using UnityEngine;
using Assets.Noyau.Players.view;
using Assets.Noyau.Manager.view;
using Kernel.Settings;

public enum PlayerNames
{
    Alpha,
    Bravo,
    Charlie,
    Delta,
    Echo,
    Foxtrot,
    Golf,
    Hotel
}

/// <summary>
/// Définition d'un joueur dans une partie
/// </summary>
public class Player
{
    /// <summary>
    /// ordre du jeu d'un joueur
    /// </summary>
    public int Id { get; private set; }
    /// <summary>
    /// nom du joueur
    /// </summary>
    public string Name { get; set; }
    /// <summary>
    /// nombre de blessure
    /// </summary>

    public Setting<int> Wound { get; private set; } = new Setting<int>(0);
    /// <summary>
    /// carte révélée à tous ou cachée
    /// </summary>
    public Setting<bool> Revealed { get; private set; } = new Setting<bool>(false);
    /// <summary>
    /// vivant ou mort
    /// </summary>
    public Setting<bool> Dead { get; private set; } = new Setting<bool>(false);
    /// <summary>
    /// pouvoir déjà utilisé ou non
    /// </summary>
    public Setting<bool> UsedPower { get; private set; } = new Setting<bool>(false);
    /// <summary>
    /// bonus d'attaque (par défaut = 0)
    /// </summary>
    public Setting<int> BonusAttack { get; private set; } = new Setting<int>(0);
    /// <summary>
    /// malus d'attaque (par défaut = 0)
    /// </summary>
    public Setting<int> MalusAttack { get; private set; } = new Setting<int>(0);
    /// <summary>
    /// réduction du nombre de Blessures subites (par défaut = 0)
    /// </summary>
    public Setting<int> ReductionWounds { get; private set; } = new Setting<int>(0);
    /// <summary>
    /// le joueur possède-t-il la mitrailleuse ?
    /// </summary>

    public Setting<bool> HasGatling { get; private set; } = new Setting<bool>(false);
    /// <summary>
    /// le joueur possède-t-il le revolver ?
    /// </summary>
    public Setting<bool> HasRevolver { get; private set; } = new Setting<bool>(false);
    /// <summary>
    /// le joueur possède-t-il le sabre ?
    /// </summary>
    public Setting<bool> HasSaber { get; private set; } = new Setting<bool>(false);
    /// <summary>
    /// le joueur possède-t-il l'amulette ?
    /// </summary>
    public Setting<bool> HasAmulet { get; private set; } = new Setting<bool>(false);
    /// <summary>
    /// le joueur possède-t-il la broche ?
    /// </summary>
    public Setting<bool> HasBroche { get; private set; } = new Setting<bool>(false);
    /// <summary>
    /// le joueur possède-t-il la boussole ?
    /// </summary>
    public Setting<bool> HasCompass { get; private set; } = new Setting<bool>(false);
    /// <summary>
    /// le joueur possède-t-il le crucifix ?
    /// </summary>
    public Setting<bool> HasCrucifix { get; private set; } = new Setting<bool>(false);
    /// <summary>
    /// le joueur possède-t-il la lance ?
    /// </summary>
    public Setting<bool> HasSpear { get; private set; } = new Setting<bool>(false);
    /// <summary>
    /// le joueur possède-t-il la toge ?
    /// </summary>
    public Setting<bool> HasToge { get; private set; } = new Setting<bool>(false);
    /// <summary>
    /// le joueur est-il sous l'effet de l'ange gardien ?
    /// </summary>
    public Setting<bool> HasGuardian { get; private set; } = new Setting<bool>(false);
    /// <summary>
    /// le joueur est-il sous l'effet du savoir ancestral ?
    /// </summary>
    public Setting<bool> HasAncestral { get; private set; } = new Setting<bool>(false);
    /// <summary>
    /// nb d'équipements
    /// </summary>
    public Setting<int> NbEquipment { get; private set; } = new Setting<int>(0);
    /// <summary>
    /// le joueur a-t-il gagné ?
    /// </summary>
    public Setting<bool> HasWon { get; private set; } = new Setting<bool>(false);
    /// <summary>
    /// position du joueur
    /// </summary>
    public Setting<int> Position { get; private set; } = new Setting<int>(-1);

    /// <summary>
    /// personnage du joueur
    /// </summary>
    public Character Character { get; private set; }
    /// <summary>
    /// liste des cartes possédées par le joueur
    /// </summary>
    public List<Card> ListCard { get; private set; }
    /// <summary>
    /// le joueur peut-il utiliser son pouvoir ?
    /// </summary>
    public Setting<bool> CanUsePower { get; private set; } = new Setting<bool>(false);
    /// <summary>
    /// le joueur a-t-il déjà utilisé son pouvoir une fois (utilisé pour les usages uniques)
    /// </summary>
    public Setting<bool> PowerUsed { get; private set; } = new Setting<bool>(false);
    /// <summary>
    /// Id du joueur qui m'a attaqué en dernier (Loup-garou)
    /// </summary>
    public Setting<int> OnAttacked { get; private set; } = new Setting<int>(-1);
    /// <summary>
    /// Id du joueur que j'ai attaqué en dernier (Charles)
    /// </summary>
    public Setting<int> OnAttacking { get; private set; } = new Setting<int>(-1);
    /// <summary>
    /// Nombre de dommage reçu
    /// </summary>
    public Setting<int> OnDealDamage { get; private set; } = new Setting<int>(0);
    /// <summary>
    /// Nombre de dommage infligé pour la dernière fois en attaquant
    /// </summary>
    public Setting<int> DamageDealed { get; private set; } = new Setting<int>(-1);
 
    ///private static List<Player> players = new List<Player>();

    /// <summary>
    /// Constructeur d'un joueur
    /// </summary>
    /// <param name="id">Id du joueur</param>
    /// <param name="c">Personnage qu'il va jouer</param>
    public Player(int id, Character c)
    {
        this.Id = id;
        this.Name = ((PlayerNames)id).ToString();
        this.ListCard = new List<Card>();
        this.Character = c;

        // add death logic
        Wound.AddListener((sender) =>
        {
            if (Wound.Value >= this.Character.characterHP)
            {
                this.Dead.Value = true;
            }
        });
        
        
        //players.Add(this);
    }

    /// <summary>
    /// Fonction qui inflige des dégâts
    /// </summary>
    /// <param name="damage">Le nombre de dommage</param>
    /// <param name="attacker">Joueur qui l'a attaqué</param>
    /// <param name="isAttack">Booléen si c'est une attaque ou des dégats infligés par une carte à effet</param>
    public virtual int Wounded(int damage, Player attacker, bool isAttack)
    {
        if (attacker.Character.characterName == "character.name.charles" && isAttack)
                GameManager.HasKilled.Value=true;

        if (damage > 0 && (!HasGuardian.Value || !isAttack))
        {

            if (ReductionWounds.Value > 0)
                damage = (damage - ReductionWounds.Value < 0) ? 0 : damage - ReductionWounds.Value;
            
            //si c'est une attaque pour les pouvoirs du Vampire et Bob
            if(isAttack)
                attacker.DamageDealed.Value = damage;
            
            this.Wound.Value += damage;
            return damage;
        }
        return 0;
    }

    /// <summary>
    /// Fonction qui soigne
    /// </summary>
    /// <param name="heal">Le nombre de point de vie soignés</param>
    public virtual void Healed(int heal)
    {
        if (Dead.Value)
            return;

        this.Wound.Value -= Mathf.Min(heal, this.Wound.Value);
    }
    

    public void PrintCards()
    {
        Debug.Log("Joueur " + Name + " : ");
        foreach (Card c in ListCard)
        {
            Debug.Log("Carte : " + c.cardLabel);
            if (c is EquipmentCard)
                Debug.Log("C'est une carte équipement !");
            else
                Debug.Log("C'est une carte à utilisation unique.");
        }
    }

    /// <summary>
    /// Fonction qui ajoute une carte au joueur
    /// </summary>
    /// <param name="card">La carte ajoutée</param>
    public void AddCard(Card card)
    {
        ListCard.Add(card);
        NbEquipment.Value++;
    }

    /// <summary>
    /// Fonction qui enlève une carte au joueur
    /// </summary>
    /// <param name="index">L'index de la carte enlevée</param>
    public void RemoveCard(int index)
    {
        ListCard.RemoveAt(index);
        NbEquipment.Value--;
    }
    
    /// <summary>
    /// Fonction qui renvoie l'indice de la carte chez un joueur
    /// </summary>
    /// <param name="cardName">Le nom de la carte recherchée</param>
    public int HasCard(string cardName)
    {
        for (int i = 0 ; i < ListCard.Count ; i++)
        {
            if (ListCard[i].cardLabel.Equals(cardName))
                return i;
        }
        return -1;
    }

    /// <summary>
    /// Fonction qui renvoie la liste de joueurs pouvant être ciblés lors d'une attaque
    /// </summary>
    public List<Player> getTargetablePlayers()
    {
        List<Player> tps = new List<Player>();

        int posP1 = this.Position.Value;
        int posP2;

        foreach (Player player in PlayerView.GetPlayers())
        {
            if (!player.Dead.Value && player.Id != this.Id && player.Position.Value != -1)
            {
                posP2 = player.Position.Value;
                if (((posP1 % 2 == 0 && (posP2 == posP1 || posP2 == posP1 + 1))
                    || (posP1 % 2 == 1 && (posP2 == posP1 || posP2 == posP1 - 1)))
                    ^ this.HasRevolver.Value)
                    tps.Add(player);
            }
        }

        return tps;
    }
}   
