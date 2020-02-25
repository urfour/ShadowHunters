using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player
{
    private int id;                  // ordre du jeu d'un joueur
    private string playerName;       // nom du pesonnage
    private CharacterTeam team;      // shadow/hunter/neutre
    private int life;                // nombre de points de vie
    private int wound;               // nombre de blessure
    private bool revealed;           // carte révélée à tous ou cachée
    private bool dead;               // vivant ou mort
    private bool usedPower;          // pouvoir déjà utilisé ou non
    private int bonusAttack;         // bonus d'attaque (par défaut = 0)
    private int malusAttack;         // malus d'attaque (par défaut = 0)
    private int reductionWounds;     // réduction du nombre de Blessures subites (par défaut = 0)
    private bool hasGatling;
    private bool hasRevolver;
    private bool hasSaber;
    private bool isTurn;
    private Position position;       // position du joueur
    private Character character;     // personnage du joueur
    private List<Card> listCard;     // liste des cartes possédées par le joueur

    public Player(int id)
    {
        this.id = id;
        this.playerName = "undefined";
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
        this.isTurn = false;
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
        set { wound = value; }
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

    public void Wounded(int damage)
    {
        if (damage > 0)
        {
            string blessure = " Blessure";
            this.Wound += damage - reductionWounds;
            if (damage > 1)
                blessure += "s";
            Debug.Log("Le joueur " + id + " subit " + damage + blessure + " !");
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
            this.Wound -= heal;
            if (heal > 1)
                blessure += "s";
            Debug.Log("Le joueur " + id + " est soigné de " + heal + blessure + " !");
        }

        if (this.Wound < 0)
            this.Wound = 0;
    }

    public bool IsDead()
    {
        if (this.Wound >= this.Life)
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
            Debug.Log("Carte : " + c.cardName);
    }

    public void AddCard(Card card)
    { 
        listCard.Add(card);
    }

    public void RemoveLastDrawnCard()
    {
        listCard.RemoveAt(0);
    }

    public void SetCharacter(Character character)
    {
        this.playerName = character.characterName;
        this.team = character.team;
        this.life = character.characterHP;
        this.character = character;
    }
}   
