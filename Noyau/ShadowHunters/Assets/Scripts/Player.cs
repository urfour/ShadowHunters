using System.Collections;
using System.Collections.Generic;
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
    private int id;                  // ordre du jeu d'un joueur
    private string playerName;       // nom du joueur
    private CharacterTeam team;      // shadow/hunter/neutre
    private int life;                // nombre de points de vie
    private int wound;               // nombre de blessure
    private bool revealed;           // carte révélée à tous ou cachée
    private bool dead;               // vivant ou mort
    private bool usedPower;          // pouvoir déjà utilisé ou non
    private int bonusAttack;         // bonus d'attaque (par défaut = 0)
    private int malusAttack;         // malus d'attaque (par défaut = 0)
    private int reductionWounds;     // réduction du nombre de Blessures subites (par défaut = 0)
    private bool hasGatling;         // le joueur possède-t-il la mitrailleuse ?
    private bool hasRevolver;        // le joueur possède-t-il le revolver ?
    private bool hasSaber;           // le joueur possède-t-il le sabre ?
    private bool hasAmulet;          // le joueur possède-t-il l'amulette ?
    private bool hasCompass;         // le joueur possède-t-il la boussole ?
    private bool hasBroche;          // le joueur possède-t-il la broche ?
    private bool hasCrucifix;        // le joueur possède-t-il le crucifix ?
    private bool hasSpear;           // le joueur possède-t-il la lance ?
    private bool hasToge;            // le joueur possède-t-il la toge ?
    private bool hasGuardian;         // le joueur est-il sous l'effet de l'ange gardien ?
    private bool isTurn;             // est-ce le tour du joueur ?
    private bool hasWon;             // le joueur a-t-il gagné ?
    private Position position;       // position du joueur
    private Character character;     // personnage du joueur
    private List<Card> listCard;     // liste des cartes possédées par le joueur

    public Player(int id)
    {
        this.id = id;
        this.playerName = ((PlayerNames)id).ToString();
        this.life = 0;
        this.wound = 0;
        this.revealed = false;
        this.dead = false;
        this.usedPower = false;
        this.bonusAttack = 0;
        this.malusAttack = 0;
        this.reductionWounds = 0;
        this.hasGatling = false;
        this.hasRevolver = false;
        this.hasSaber = false;
        this.hasAmulet = false;
        this.hasBroche = false;
        this.hasCompass = false;
        this.hasCrucifix = false;
        this.hasSpear = false;
        this.hasToge = false;
        this.hasGuardian = false;
        this.isTurn = false;
        this.hasWon = false;
        this.position = Position.None;
        this.listCard = new List<Card>();
    }

    public int Id
    {
        get { return id; }
        set { id = value; }
    }

    public string Name
    {
        get { return playerName; }
        set { playerName = value; }
    }

    public CharacterTeam Team
    {
        get { return team; }
        set { team = value; }
    }

    public int Life
    {
        get { return life; }
        set { life = value; }
    }

    public int Wound
    {
        get { return wound; }
    }
    public bool Revealed
    {
        get { return revealed; }
        set { revealed = value; }
    }

    public bool Dead
    {
        get { return dead; }
        set { dead = value; }
    }

    public bool UsedPower
    {
        get { return usedPower; }
        set { usedPower = value; }
    }

    public int BonusAttack
    {
        get { return bonusAttack; }
        set { bonusAttack = value; }
    }

    public int MalusAttack
    {
        get { return malusAttack; }
        set { malusAttack = value; }
    }
    
    public int ReductionWounds
    {
        get { return reductionWounds; }
        set { reductionWounds = value; }
    }

    public Position Position
    {
        get { return position; }
        set { position = value; }
    }

    public bool IsTurn
    {
        get { return isTurn; }
        set { isTurn = value; }
    }

    public bool HasGatling
    {
        get { return hasGatling; }
        set { hasGatling = value; }
    }

    public bool HasRevolver
    {
        get { return hasRevolver; }
        set { hasRevolver = value; }
    }

    public bool HasSaber
    {
        get { return hasSaber; }
        set { hasSaber = value; }
    }

    public bool HasAmulet
    {
        get{return hasAmulet;}
        set{hasAmulet=value;}
    }
    public bool HasBroche
    {
        get{return hasBroche;}
        set{hasBroche=value;}
    }
    public bool HasCompass
    {
        get{return hasCompass;}
        set{hasCompass=value;}
    }
    public bool HasCrucifix
    {
        get{return hasCrucifix;}
        set{hasCrucifix=value;}
    }
    public bool HasSpear
    {
        get{return hasSpear;}
        set{hasSpear=value;}
    }
    public bool HasToge
    {
        get{return hasToge;}
        set{hasToge=value;}
    }
    public bool HasGuardian
    {
        get { return hasGuardian; }
        set { hasGuardian = value; }
    }

    public bool HasWon
    {
        get { return hasWon; }
        set { hasWon = value; }
    }

    public void Wounded(int damage)
    {
        if (damage > 0 && !HasGuardian)
        {
            string blessure = " Blessure";
            
            if (reductionWounds > 0)
                damage = (damage - reductionWounds < 0) ? 0 : damage - reductionWounds;

            this.wound += damage;
            if (damage > 1)
                blessure += "s";
            Debug.Log("Le joueur " + id + " subit " + damage + blessure + " !");
        }

        if (HasGuardian)
        {
            Debug.Log("Le joueur " + id + " est protégé par l'Ange Gardien !");
        }

        if (this.IsDead())
            this.Dead = true;
    }

    public void Healed(int heal)
    {
        if (this.IsDead())
            return;

        if (heal > 0)
        {
            string blessure = " Blessure";
            this.wound -= heal;
            if (heal > 1)
                blessure += "s";
            Debug.Log("Le joueur " + id + " est soigné de " + heal + blessure + " !");
        }

        if (this.wound < 0)
            this.wound = 0;
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
				
			this.wound = wound;
			Debug.Log("Le joueur " + id + " a maintenant " + wound + blessure + " !");
		}

	}

    public bool IsDead()
    {
        if (this.wound >= this.Life)
            return true;

        return false;
    }

    public List<Card> ListCard
    {
        get { return listCard; }
    }

    public void PrintCards()
    {
        Debug.Log("Joueur " + playerName + " : ");
        foreach (Card c in listCard)
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
        listCard.Add(card);
    }

    public void RemoveCard(int index)
    {
        listCard.RemoveAt(index);
    }

    public void SetCharacter(Character character)
    {
        this.team = character.team;
        this.life = character.characterHP;
        this.character = character;
    }

    public Character Character
    {
        get { return character; }
    }

    public int HasCard(string cardName)
    {
        for (int i = 0 ; i < listCard.Count ; i++)
        {
            if (listCard[i].cardName.Equals(cardName))
                return i;
        }
        return -1;
    }
}   
