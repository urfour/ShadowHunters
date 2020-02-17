using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    public string name;             // nom du pesonnage
    public CharacterTeam team;      // shadow/hunter/neutre
    public int life;                // nombre de points de vie
    public int wound;               // nombre de blessure
    public bool revealed;           // carte révélée à tous ou cachée
    public bool dead;               // vivant ou mort
    public bool usedPower;          // pouvoir déjà utilisé ou non
    //public Card[] listCard;       // liste des cartes possédées par le joueur


    public Player(string name, CharacterTeam team, int life)
    {
        this.name = name;
        this.team = team;
        this.life = life;
        this.wound = 0;
        this.revealed = false;
        this.dead = false;
        this.usedPower = false;
        //listCard = [];
    }

    public string Name
    {
        get { return name; }
        set { name = value; }
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

    public void wounded(int damage)
    {
        if (damage > 0)
            this.Wound += damage;

        if (this.isDead())
            this.Dead = true;
    }

    public void healed(int heal)
    {
        if (this.isDead())
            return;

        if (heal > 0)
            this.Wound -= heal;

        if (this.Wound < 0)
            this.Wound = 0;
    }

    public bool isDead()
    {
        if (this.Wound >= this.Life)
            return true;

        return false;
    }
}
