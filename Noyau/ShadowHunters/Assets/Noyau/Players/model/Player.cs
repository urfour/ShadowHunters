using System.Collections;
using System.Collections.Generic;
using Assets.Noyau.Players.model;
using EventSystem;
using Scripts.Settings;
using UnityEngine;

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
    // le joueur a-t-il gagné ?
    public Setting<bool> HasWon { get; private set; } = new Setting<bool>(false);
    // position du joueur
    public Position Position { get; set; }


    // personnage du joueur
    public Character Character { get; private set; }

    // liste des cartes possédées par le joueur
    public List<Card> ListCard { get; private set; }
    // le joueur peut-il utiliser son pouvoir ?
    public Setting<bool> CanUsePower { get; private set; } = new Setting<bool>(false);

    public Setting<bool> OnAttacked { get; private set; } = new Setting<bool>(false);
    public Setting<int> OnDealDamage { get; private set; } = new Setting<int>(0);
 
    //private static List<Player> players = new List<Player>();

    public Player(int id, Character c)
    {
        this.Id = id;
        this.Name = ((PlayerNames)id).ToString();
        this.Position = Position.None;
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

    public virtual int Wounded(int damage)
    {
        if (damage > 0 && !HasGuardian.Value)
        {

            if (ReductionWounds.Value > 0)
                damage = (damage - ReductionWounds.Value < 0) ? 0 : damage - ReductionWounds.Value;

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
            Debug.Log("Carte : " + c.cardName);
            if (c.isEquipement)
                Debug.Log("C'est une carte équipement !");
            else
                Debug.Log("C'est une carte à utilisation unique.");
        }
    }

    public void AddCard(Card card)
    {
        ListCard.Add(card);
    }

    public void RemoveCard(int index)
    {
        ListCard.RemoveAt(index);
    }
    

    public int HasCard(string cardName)
    {
        for (int i = 0 ; i < ListCard.Count ; i++)
        {
            if (ListCard[i].cardName.Equals(cardName))
                return i;
        }
        return -1;
    }
}   
