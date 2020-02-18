using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLogic : MonoBehaviour
{
    private int m_nbPlayers = 0;
    public int NbPlayers { get; set; }

    private int m_nbHunters = 0;
    public int NbHunters { get; set; }

    private int m_nbHuntersDead = 0;
    public int NbHuntersDead { get; set; }

    private int m_nbShadows = 0;
    public int NbShadows { get; set; }

    private int m_nbShadowsDeads = 0;
    public int NbShadowDeads { get; set; }

    private int m_nbNeutrals = 0;
    public int NbNeutrals { get; set; }

    private int m_nbNeutralsDeads = 0;
    public int NbNeutralsDeads { get; set; }

    private int m_playerTurn = 0;
    public int PlayerTurn { get; set; }

    private List<Player> players;
    public List<Player> Players { get; }

    private GameBoard gameBoard;
    public GameBoard GameBoard { get; }

    // Cartes possibles des différents decks
    public List<Card> m_visionCards;
    public List<Card> m_darknessCards;
    public List<Card> m_lightCards;
    public List<Card> m_locationCards;
    public List<Card> m_characterCards;

    public GameLogic(int players)
    {
        m_nbPlayers = players;
    }

    void Start()
    {
 
    }
    public void PrepareGame(int nbPlayers)
    {
        gameBoard = new GameBoard(PrepareDecks(m_locationCards), PrepareDecks(m_visionCards),
            PrepareDecks(m_darknessCards), PrepareDecks(m_lightCards), nbPlayers);




    }

    public List<Card> PrepareDecks(List<Card> cards)
    {
        List<Card> deck = new List<Card>();
        for (int i = 0; i < cards.Count; i++)
            deck.Add(cards[i]);
        deck.Shuffle<Card>();
        return deck;
    }

    public List<Card> PrepareCharacterCards(List<Card> cards)
    {
        List<Card> characterCards = new List<Card>();
        switch (m_nbPlayers)
        {
            case 4:
                m_nbHunters = 2;
                m_nbShadows = 2;
                break;
            case 5:
                m_nbHunters = 2;
                m_nbShadows = 2;
                m_nbNeutrals = 1;
                break;
            case 6:
                m_nbHunters = 2;
                m_nbShadows = 2;
                m_nbNeutrals = 2;
                break;
            case 7:
                m_nbHunters = 2;
                m_nbShadows = 2;
                m_nbNeutrals = 3;
                // enlever Bob
                break;
            case 8:
                m_nbHunters = 3;
                m_nbShadows = 3;
                m_nbNeutrals = 2;
                break;
        }

        return characterCards;
    }

    void SetLocation()
    {

    }

    void PrepareDecks()
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
