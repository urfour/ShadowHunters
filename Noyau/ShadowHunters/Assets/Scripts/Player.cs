using System.Collections;
using System.Collections.Generic;
using Kernel.Settings;
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
    //private int id;                  // ordre du jeu d'un joueur
    //private string playerName;       // nom du joueur
    //private CharacterTeam team;      // shadow/hunter/neutre
    //private int life;                // nombre de points de vie
    //private int wound;               // nombre de blessure
    //private bool revealed;           // carte révélée à tous ou cachée
    //private bool dead;               // vivant ou mort
    //private bool usedPower;          // pouvoir déjà utilisé ou non
    //private int bonusAttack;         // bonus d'attaque (par défaut = 0)
    //private int malusAttack;         // malus d'attaque (par défaut = 0)
    //private int reductionWounds;     // réduction du nombre de Blessures subites (par défaut = 0)
    //private bool hasGatling;         // le joueur possède-t-il la mitrailleuse ?
    //private bool hasRevolver;        // le joueur possède-t-il le revolver ?
    //private bool hasSaber;           // le joueur possède-t-il le sabre ?
    //private bool hasAmulet;          // le joueur possède-t-il l'amulette ?
    //private bool hasCompass;         // le joueur possède-t-il la boussole ?
    //private bool hasBroche;          // le joueur possède-t-il la broche ?
    //private bool hasCrucifix;        // le joueur possède-t-il le crucifix ?
    //private bool hasSpear;           // le joueur possède-t-il la lance ?
    //private bool hasToge;            // le joueur possède-t-il la toge ?
    //private bool hasGuardian;        // le joueur est-il sous l'effet de l'ange gardien ?
    //private bool hasAncestral;       // le joueur est-il sous l'effet du savoir ancestral ?
    //private bool isTurn;             // est-ce le tour du joueur ?
    //private bool hasWon;             // le joueur a-t-il gagné ?
    //private Position position;       // position du joueur
    //private Character character;     // personnage du joueur
    //private List<Card> listCard;     // liste des cartes possédées par le joueur

    // ordre du jeu d'un joueur
    public int Id { get; private set; }
    // nom du joueur
    public string Name { get; private set; }
    // shadow/hunter/neutre
    public CharacterTeam Team { get; private set; }
    // nombre de points de vie
    public int Life { get; private set; }
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
    // est-ce le tour du joueur ?
    public Setting<bool> IsTurn { get; private set; } = new Setting<bool>(false);
    // personnage du joueur
    public Character Character { get; private set; }
    // liste des cartes possédées par le joueur
    public List<Card> ListCard { get; private set; }
    // le joueur peut-il utiliser son pouvoir ?
    public Setting<bool> CanUsePower { get; private set; } = new Setting<bool>(false);
    // le joueur peut-il ne pas utiliser son pouvoir ?
    public Setting<bool> CanNotUsePower { get; private set; } = new Setting<bool>(false);


    public Player(int id)
    {
        this.Id = id;
        this.Name = ((PlayerNames)id).ToString();
        this.Life = 0;
        this.Position = Position.None;
        this.ListCard = new List<Card>();
    }

    public void Wounded(int damage)
    {
        if (damage > 0 && !HasGuardian.Value)
        {
            string blessure = " Blessure";
            
            if (ReductionWounds.Value > 0)
                damage = (damage - ReductionWounds.Value < 0) ? 0 : damage - ReductionWounds.Value;

            this.Wound.Value += damage;
            if (damage > 1)
                blessure += "s";
            Debug.Log("Le joueur " + Id + " subit " + damage + blessure + " !");
        }

        if (HasGuardian.Value)
        {
            Debug.Log("Le joueur " + Id + " est protégé par l'Ange Gardien !");
        }

        if (this.IsDead())
            this.Dead.Value = true;
    }

    public void Healed(int heal)
    {
        if (this.IsDead())
            return;

        else if (Wound.Value == 0)
            Debug.Log("Le joueur n'a pas de Blessures, il n'est donc pas soigné.");

        else if (heal > 0)
        {
            string blessure = " Blessure";
            this.Wound.Value -= heal;
            if (heal > 1)
                blessure += "s";
            Debug.Log("Le joueur " + Id + " est soigné de " + heal + blessure + " !");
        }

        if (this.Wound.Value < 0)
            this.Wound.Value = 0;
    }
    
    public void SetWound (int wound)
    {
		if (this.IsDead())
            return;
            
        if (wound > 0)
        {
			string blessure = " Blessure";
            if (wound > 1)
				blessure += "s";
				
			this.Wound.Value = wound;
			Debug.Log("Le joueur " + Id + " a maintenant " + wound + blessure + " !");
		}

	}

    public bool IsDead()
    {
        if (this.Wound.Value >= this.Life)
            return true;

        return false;
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

    public void SetCharacter(Character character)
    {
        this.Team = character.team;
        this.Life = character.characterHP;
        this.Character = character;
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
