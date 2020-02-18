using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLogic : MonoBehaviour
{
    private int m_nbPlayers = 0;
    public int NbPlayers
    {
        get => m_nbPlayers;
        private set => m_nbPlayers = value;
    }

    private int m_nbHunters = 0;
    public int NbHunters
    {
        get => m_nbHunters;
        private set => m_nbHunters = value;
    }

    private int m_nbHuntersDead = 0;
    public int NbHuntersDead
    {
        get => m_nbHuntersDead;
        private set => m_nbHuntersDead = value;
    }

    private int m_nbShadows = 0;
    public int NbShadows
    {
        get => m_nbShadows;
        private set => m_nbShadows = value;
    }

    private int m_nbShadowsDeads = 0;
    public int NbShadowsDeads
    {
        get => m_nbShadowsDeads;
        private set => m_nbShadowsDeads;
    }

    private int m_nbNeutrals = 0;
    public int NbNeutrals
    {
        get => m_nbNeutrals;
        private set => m_nbNeutrals = value;
    }

    private int m_nbNeutralsDeads = 0;
    public int NeutralsDeads
    {
        get => m_nbNeutralsDeads;
        private set => m_nbNeutralsDeads = value;
    }

    private int m_playerTurn = 0;
    public int PlayerTurn
    {
        get => m_playerTurn;
        private set => m_playerTurn = value;
    }

    private List<Player> m_players;

    private GameBoard gameBoard;
    public GameBoard GameBoard { get; }

    // Cartes possibles des différents decks
    public List<Card> m_visionCards;
    public List<Card> m_darknessCards;
    public List<Card> m_lightCards;
    public List<Card> m_locationCards;
    public List<Card> m_characterCards;

    void Start()
    {
 
    }

    public void AddPlayer(Player p)
    {
        this.m_players.Add(p);
        NbPlayers++;
    }

    public void DeletePlayer(Player p)
    {
        for (int i = 0; i < this.m_players.Count; ++i)
        {
            if (this.m_players[i] == p)
            {
                this.m_players.RemoveAt(i);
                NbPlayers--;
                break;
            }
        }
    }

    public void PrepareGame(int nbPlayers)
    {
        gameBoard = new GameBoard(PrepareDecks(m_locationCards), nbPlayers);
        PrepareDecks(m_visionCards);
        PrepareDecks(m_darknessCards);
        PrepareDecks(m_lightCards);


    }

    public List<Card> PrepareDecks(List<Card> cards)
    {
        List<Card> deck = new List<Card>();
        for (int i = 0; i < cards.Count; i++)
            deck.Add(cards[i]);
        deck.Shuffle<Card>();
        return deck;
    }

    public void AddCharacterCards(List<Card> deck, List<Card> deckCharacter,int nb)
    {
        for (int i = 0; i < nb; i++)
        {
            deck.Add(deckCharacter[i]);
        }
    }

    public List<Card> PrepareCharacterCards(List<Card> cardHunters, List<Card> cardShadows, List<Card> cardNeutrals)
    {
        List<Card> HuntersCards = new List<Card>();
        List<Card> ShadowsCards = new List<Card>();
        List<Card> NeutralsCards = new List<Card>();
        List<Card> characterCards = new List<Card>();
        switch (NbPlayers)
        {
            case 4:
                m_nbHunters = m_nbShadows = 2;
                break;
            case 5:
                m_nbHunters = m_nbShadows = 2;
                m_nbNeutrals = 1;
                break;
            case 6:
                m_nbHunters = m_nbShadows = m_nbNeutrals = 2;
                break;
            case 7:
                m_nbHunters = m_nbShadows = 2;
                m_nbNeutrals = 3;
                // enlever Bob
                break;
            case 8:
                m_nbHunters = m_nbShadows = 3;
                m_nbNeutrals = 2;
                //enlever Bob
                break;
        }
        cardHunters=PrepareDecks(HuntersCards);
        cardShadows=PrepareDecks(ShadowsCards);
        cardNeutrals=PrepareDecks(NeutralsCards);

        AddCharacterCards(characterCards,cardHunters,NbHunters);
        AddCharacterCards(characterCards,cardShadows,NbShadows);
        AddCharacterCards(characterCards,cardNeutrals,NbNeutrals);
        return characterCards;
    }

    void SetLocation()
    {
        
    }

}

public static class IListExtensions
{
    /// <summary>
    /// Shuffles the element order of the specified list.
    /// </summary>
    public static void Shuffle<T>(this List<T> ts)
    {
        var count = ts.Count;
        var last = count - 1;
        for (var i = 0; i < last; ++i)
        {
            var r = UnityEngine.Random.Range(i, count);
            var tmp = ts[i];
            ts[i] = ts[r];
            ts[r] = tmp;
        }
    }
}
