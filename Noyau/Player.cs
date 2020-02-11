using System;

class Player
{

    public string name;     // nom du pesonnage
    public string type;     // shadow/hunter/neutre
    public int life;        // nombre de points de vie
    public int wound;       // nombre de blessure
    public bool revealed;   // carte révélée à tous ou cachée
    public bool dead;       // vivant ou mort
    public bool usedPower;  // pouvoir déjà utilisé ou non
    //public Card[] listCard; // liste des cartes possédées par le joueur


    public Player(string name, string type, int life)
    {
        this.name = name;
        this.type = type;
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

    public string Type
    {
        get { return type; }
        set { type = value; }
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
        if(damage > 0)
            this.Wound += damage;

        if(this.isDead())
            this.Dead = true;
    }

    public void healed(int heal)
    {
        if(this.isDead())
            return;

        if(heal > 0)
            this.Wound -= heal;

        if(this.Wound < 0)
            this.Wound == 0; 
    }

    public bool isDead()
    {
        if(this.Wound >= this.Life)
            return true;
        
        return false;
    }

    
    static void Main(string[] args)
    {
        Player p = new Player("Mustang", "Hunter", 12);

        Console.WriteLine("Personnage 1 : ");
        Console.WriteLine("  Vie : " + p.Life);
        Console.WriteLine("  Blessure : " + p.Wound);
        Console.WriteLine("  Révélée ? " + p.Revealed);
        Console.WriteLine("  Mort ? " + p.Dead);
        Console.WriteLine("  Pouvoir utilisé ? " + p.UsedPower);
    }
}
