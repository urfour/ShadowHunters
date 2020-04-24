using System.Collections;
using System.Collections.Generic;
using Assets.Noyau.Cards.controller;
using Assets.Noyau.Cards.model;
using Assets.Noyau.Players.model;
using EventSystem;
using Scripts.Settings;
using UnityEngine;
using Assets.Noyau.Players.view;
using Assets.Noyau.Manager.view;

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

    // ordre du jeu d'un joueur
    public int Id { get; private set; }
    // nom du joueur
    public string Name { get; private set; }
    // nombre de blessure
    public Setting<int> Wound { get; private set; } = new Setting<int>(0);
    // carte révélée à tous ou cachée
    public Setting<bool> Revealed { get; private set; } = new Setting<bool>(false);
    // vivant ou mort
    public Setting<bool> Dead { get; private set; } = new Setting<bool>(false);
    // pouvoir déjà utilisé ou non
    public Setting<bool> UsedPower { get; private set; } = new Setting<bool>(false);
    // bonus d'attaque (par défaut = 0)
    public Setting<int> BonusAttack { get; private set; } = new Setting<int>(0);
    // malus d'attaque (par défaut = 0)
    public Setting<int> MalusAttack { get; private set; } = new Setting<int>(0);
    // réduction du nombre de Blessures subites (par défaut = 0)
    public Setting<int> ReductionWounds { get; private set; } = new Setting<int>(0);
    // le joueur possède-t-il la mitrailleuse ?
    public Setting<bool> HasGatling { get; private set; } = new Setting<bool>(false);
    // le joueur possède-t-il le revolver ?
    public Setting<bool> HasRevolver { get; private set; } = new Setting<bool>(false);
    // le joueur possède-t-il le sabre ?
    public Setting<bool> HasSaber { get; private set; } = new Setting<bool>(false);
    // le joueur possède-t-il l'amulette ?
    public Setting<bool> HasAmulet { get; private set; } = new Setting<bool>(false);
    // le joueur possède-t-il la broche ?
    public Setting<bool> HasBroche { get; private set; } = new Setting<bool>(false);
    // le joueur possède-t-il la boussole ?
    public Setting<bool> HasCompass { get; private set; } = new Setting<bool>(false);
    // le joueur possède-t-il le crucifix ?
    public Setting<bool> HasCrucifix { get; private set; } = new Setting<bool>(false);
    // le joueur possède-t-il la lance ?
    public Setting<bool> HasSpear { get; private set; } = new Setting<bool>(false);
    // le joueur possède-t-il la toge ?
    public Setting<bool> HasToge { get; private set; } = new Setting<bool>(false);
    // le joueur est-il sous l'effet de l'ange gardien ?
    public Setting<bool> HasGuardian { get; private set; } = new Setting<bool>(false);
    // le joueur est-il sous l'effet du savoir ancestral ?
    public Setting<bool> HasAncestral { get; private set; } = new Setting<bool>(false);
    // nb d'équipements
    public Setting<int> NbEquipment { get; private set; } = new Setting<int>(0);
    // le joueur a-t-il gagné ?
    public Setting<bool> HasWon { get; private set; } = new Setting<bool>(false);
    // position du joueur
    public Setting<int> Position { get; private set; } = new Setting<int>(-1);


    // personnage du joueur
    public Character Character { get; private set; }

    // liste des cartes possédées par le joueur
    public List<Card> ListCard { get; private set; }
    // le joueur peut-il utiliser son pouvoir ?
    public Setting<bool> CanUsePower { get; private set; } = new Setting<bool>(false);
    // le joueur a-t-il déjà utilisé son pouvoir une fois (utilisé pour les usages uniques)
    public Setting<bool> PowerUsed { get; private set; } = new Setting<bool>(false);

    // Id du joueur qui m'a attaqué en dernier (Loup-garou)
    public Setting<int> OnAttacked { get; private set; } = new Setting<int>(-1);
    // Id du joueur que j'ai attaqué en dernier (Charles)
    public Setting<int> OnAttacking { get; private set; } = new Setting<int>(-1);

    public Setting<int> OnDealDamage { get; private set; } = new Setting<int>(0);
    // Nombre de dommage infligé pour la dernière fois en attaquant
    public Setting<int> DamageDealed { get; private set; } = new Setting<int>(-1);
 
    //private static List<Player> players = new List<Player>();

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

    public virtual int Wounded(int damage, Player attacker, bool isAttack)
    {
        if(attacker.Character.characterName=="Charles" && isAttack)
            GameManager.HasKilled.Value=true;

        if (damage > 0 && !HasGuardian.Value)
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

    public void AddCard(Card card)
    {
        ListCard.Add(card);
        NbEquipment.Value++;
    }

    public void RemoveCard(int index)
    {
        ListCard.RemoveAt(index);
        NbEquipment.Value--;
    }
    

    public int HasCard(string cardName)
    {
        for (int i = 0 ; i < ListCard.Count ; i++)
        {
            if (ListCard[i].cardLabel.Equals(cardName))
                return i;
        }
        return -1;
    }


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
                    || (posP2 % 2 == 1 && (posP2 == posP1 || posP2 == posP1 - 1)))
                    && !this.HasRevolver.Value)
                    tps.Add(player);

                else
                    tps.Add(player);
            }
        }

        return tps;
    }
}   
