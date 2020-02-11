enum CardType
{
    Dark,
    Light,
    Vision,
    Location,
    Faction
}

enum Facing 
{
    UP, 
    DOWN
}

public class Card 
{
    public int id;
    public string Name;
    public string Description;
    public Facing facing = Facing.DOWN;
    public CardType Type;
}