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
        private set => m_nbShadowsDeads = value;
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

    private int m_playerTurn = -1;
    public int PlayerTurn
    {
        get => m_playerTurn;
        private set => m_playerTurn = value;
    }

    private List<Player> m_players;

    private GameBoard gameBoard;
    public GameBoard GameBoard { get; }

    // Cartes possibles des différents decks
    public List<VisionCard> m_visionCards;
    public List<Card> m_darknessCards;
    public List<Card> m_lightCards;
    public List<Card> m_locationCards;
    public List<Character> m_hunterCharacters;
    public List<Character> m_shadowCharacters;
    public List<Character> m_neutralCharacters;

    void Start()
    {
        const int NB_PLAYERS = 5;
        PrepareGame(NB_PLAYERS);
        ChooseNextPlayer();
        //gameBoard.PrintLog();

    }

    void AddPlayer(Player p)
    {
        this.m_players.Add(p);
        m_nbPlayers++;
    }

    void DeletePlayer(Player p)
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

    void PrepareGame(int nbPlayers)
    {
        m_players = new List<Player>();
        Player p;
        gameBoard = new GameBoard(PrepareDecks(m_locationCards), PrepareDecks(m_visionCards), PrepareDecks(m_darknessCards), PrepareDecks(m_lightCards), nbPlayers);
        List<Character> characters;
        for (int i = 0; i < nbPlayers; i++)
        {
            p = new Player(i);
            AddPlayer(p);
        }
        characters = PrepareCharacterCards(m_hunterCharacters, m_shadowCharacters, m_neutralCharacters);
        for (int i = 0; i < nbPlayers; i++)
        {
            m_players[i].SetCharacter(characters[0]);
            characters.RemoveAt(0);
        }

    }

    List<Card> PrepareDecks(List<Card> cards)
    {
        List<Card> deck = new List<Card>();
        for (int i = 0; i < cards.Count; i++)
            deck.Add(cards[i]);
        deck.Shuffle<Card>();
        return deck;
    }

    List<VisionCard> PrepareDecks(List<VisionCard> cards)
    {
        List<VisionCard> deck = new List<VisionCard>();
        for (int i = 0; i < cards.Count; i++)
            deck.Add(cards[i]);
        deck.Shuffle<VisionCard>();
        return deck;
    }

    List<Character> PrepareCharacters(List<Character> cards)
    {
        List<Character> deck = new List<Character>();
        for (int i = 0; i < cards.Count; i++)
            deck.Add(cards[i]);
        deck.Shuffle<Character>();
        return deck;
    }

    void ResetDecks(List<Card> oldDeck, List<Card> newDeck)
    {
        Debug.Log("Le deck est vide, redistribution des cartes.");
        for (int i = 0; i < oldDeck.Count; i++)
        {
            newDeck.Add(oldDeck[i]);
            oldDeck.RemoveAt(i);
        }
        newDeck.Shuffle<Card>();
    }

    void AddCharacterCards(List<Character> deck, List<Character> deckCharacter, int nb, bool addBob)
    {
        for (int i = 0; i < nb; i++)
        {
            if (nb >= 7 && !addBob)
                i--;
            else
                deck.Add(deckCharacter[i]);
        }
    }

    List<Character> PrepareCharacterCards(List<Character> cardHunters, List<Character> cardShadows, List<Character> cardNeutrals)
    {
        List<Character> HuntersCards = new List<Character>();
        List<Character> ShadowsCards = new List<Character>();
        List<Character> NeutralsCards = new List<Character>();
        List<Character> characterCards = new List<Character>();
        bool addBob = true;
        switch (m_nbPlayers)
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
                addBob = false;
                break;
            case 8:
                m_nbHunters = m_nbShadows = 3;
                m_nbNeutrals = 2;
                addBob = false;
                break;
        }
        HuntersCards = PrepareCharacters(cardHunters);
        ShadowsCards = PrepareCharacters(cardShadows);
        NeutralsCards = PrepareCharacters(cardNeutrals);

        AddCharacterCards(characterCards, HuntersCards, NbHunters, addBob);
        AddCharacterCards(characterCards, ShadowsCards, NbShadows, addBob);
        AddCharacterCards(characterCards, NeutralsCards, NbNeutrals, addBob);
        return characterCards;
    }

    void MoveCharacter()
    {
        int lancer1 = Random.Range(1, 6);
        int lancer2 = Random.Range(1, 4);
        int lancerTotal = lancer1 + lancer2;
        string carte = "";
        Debug.Log("Le lancer de dés donne " + lancer1 + " et " + lancer2 + " (" + lancerTotal + ").");
        switch (lancerTotal)
        {
            case 2:
            case 3:
                m_players[m_playerTurn].Position = Position.Antre;
                gameBoard.setPositionOfAt(m_playerTurn, Position.Antre);
                carte = "Antre de l'Ermite";
                break;
            case 4:
            case 5:
                m_players[m_playerTurn].Position = Position.Porte;
                gameBoard.setPositionOfAt(m_playerTurn, Position.Porte);
                carte = "Porte de l'outremonde";
                break;
            case 6:
                m_players[m_playerTurn].Position = Position.Monastere;
                gameBoard.setPositionOfAt(m_playerTurn, Position.Monastere);
                carte = "Monastère";
                break;
            case 7:
                Debug.Log("Où souhaitez-vous aller ?");
                m_players[m_playerTurn].Position = Position.Antre;
                gameBoard.setPositionOfAt(m_playerTurn, Position.Antre);
                // TODO choix du lieu
                carte = "Antre de l'Ermite";
                break;
            case 8:
                m_players[m_playerTurn].Position = Position.Cimetiere;
                gameBoard.setPositionOfAt(m_playerTurn, Position.Cimetiere);
                carte = "Cimetière";
                break;
            case 9:
                m_players[m_playerTurn].Position = Position.Foret;
                gameBoard.setPositionOfAt(m_playerTurn, Position.Foret);
                carte = "Forêt hantée";
                break;
            case 10:
                m_players[m_playerTurn].Position = Position.Sanctuaire;
                gameBoard.setPositionOfAt(m_playerTurn, Position.Sanctuaire);
                carte = "Sanctuaire ancien";
                break;
        }
        Debug.Log("Le joueur " + m_playerTurn + " se rend sur la carte " + carte + ".");
    }

    void ActivateLocationPower()
    {
        int choosenPlayerId = m_playerTurn;
        switch (m_players[m_playerTurn].Position)
        {
            case Position.Antre:
                Debug.Log("Le joueur " + m_playerTurn + " pioche une carte Vision.");
                // TODO Choix du joueur à qui donner la carte vision
                while (choosenPlayerId == m_playerTurn)
                    choosenPlayerId = Random.Range(0, m_nbPlayers - 1);
                Debug.Log("La carte Vision a été donnée au joueur " + choosenPlayerId + ".");
                VisionCardPower(gameBoard.DrawCard(CardType.Vision) as VisionCard, choosenPlayerId);
                break;
            case Position.Porte:
                // TODO choix de la carte piochée
                Debug.Log("Le joueur " + m_playerTurn + " choisit de piocher une carte Ténèbres.");
                m_players[m_playerTurn].AddCard(gameBoard.DrawCard(CardType.Darkness));
                break;
            case Position.Monastere:
                Debug.Log("Le joueur " + m_playerTurn + " pioche une carte Lumière.");
                m_players[m_playerTurn].AddCard(gameBoard.DrawCard(CardType.Light));
                break;
            case Position.Cimetiere:
                Debug.Log("Le joueur " + m_playerTurn + " pioche une carte Ténèbres.");
                m_players[m_playerTurn].AddCard(gameBoard.DrawCard(CardType.Darkness));
                break;
            case Position.Foret:
                // TODO choix du joueur + infliger les Blessures ou en soigner
                Debug.Log("Le joueur " + m_playerTurn + " peut infliger des Blessures ou en soigner.");
                break;
            case Position.Sanctuaire:
                // TODO vol d'une carte équipement
                Debug.Log("Le joueur " + m_playerTurn + " peut voler une carte équipement.");
                break;
        }
        // TODO : activer les pouvoirs des lieux
        return;
    }

    void VisionCardPower(VisionCard pickedCard, int playerId)
    {
        CharacterTeam team = m_players[playerId].Team;
        // Cartes applicables en fonction des équipes ?
        if ((team == CharacterTeam.Shadow && !pickedCard.visionEffect.effectOnShadow)
            || (team == CharacterTeam.Hunter && !pickedCard.visionEffect.effectOnHunter)
            || (team == CharacterTeam.Neutral && !pickedCard.visionEffect.effectOnNeutral))
        {
            if (pickedCard.visionEffect.effectSupremeVision)
                Debug.Log("C'est une carte Vision Suprême !");
            else
                Debug.Log("Rien ne se passe.");
        }
        else
        {
            // Cas des cartes applicables en fonction des points de vie
            if (pickedCard.visionEffect.effectOnLowHP && checkLowHPCharacters(m_players[playerId].Name))
                m_players[playerId].Wounded(1);
            else if (pickedCard.visionEffect.effectOnHighHP && checkHighHPCharacters(m_players[playerId].Name))
                m_players[playerId].Wounded(2);

            // Cas des cartes infligeant des Blessures
            else if (pickedCard.visionEffect.effectOneWound)
                m_players[playerId].Wounded(1);
            else if (pickedCard.visionEffect.effectTwoWounds)
                m_players[playerId].Wounded(2);
            // Cas des cartes soignant des Blessures
            else if (pickedCard.visionEffect.effectHealingOneWound)
            {
                if (m_players[playerId].Wound == 0)
                    m_players[playerId].Wounded(1);
                else
                    m_players[playerId].Healed(1);
            }
            // Cas des cartes volant une carte équipement ou infligeant des Blessures
            else if (pickedCard.visionEffect.effectGivingEquipementCard)
            {
                // TODO vol d'une carte équipement
                Debug.Log("Possibilité de voler une carte équipement");
                m_players[playerId].Wounded(1);
            }
            else
                Debug.Log("Rien ne se passe.");
        }
    }

    bool checkLowHPCharacters(string characterName)
    {
        return characterName.StartsWith("A") || characterName.StartsWith("B")
            || characterName.StartsWith("C") || characterName.StartsWith("E")
            || characterName.StartsWith("M");
    }

    bool checkHighHPCharacters(string characterName)
    {
        return characterName.StartsWith("D") || characterName.StartsWith("F")
            || characterName.StartsWith("G") || characterName.StartsWith("L")
            || characterName.StartsWith("V");
    }

    void Attack()
    {
        int playerAttackedId = Random.Range(0, m_nbPlayers);
        // TODO : méthode pour choisir le joueur à attaquer
        if (playerAttackedId == m_playerTurn)
            Debug.Log("Vous choisissez de ne pas attaquer.");
        else
        {
            Debug.Log("Vous choisissez d'attaquer le joueur " + playerAttackedId + ".");
            int lancer1 = Random.Range(1, 6);
            int lancer2 = Random.Range(1, 4);
            int lancerTotal = Mathf.Abs(lancer1 - lancer2);
            m_players[playerAttackedId].Wounded(lancerTotal);
            if (m_players[playerAttackedId].IsDead())
                Debug.Log("Le joueur " + playerAttackedId + " est mort !");
        }
    }

    void ChooseNextPlayer()
    {
        if (m_playerTurn == -1)
            m_playerTurn = Random.Range(0, m_nbPlayers - 1);
        else
            m_playerTurn = (m_playerTurn + 1) % m_nbPlayers;
        Debug.Log("C'est au joueur " + m_playerTurn + " de jouer.");
    }

    public void PlayTurn()
    {
        MoveCharacter();
        ActivateLocationPower();
        Attack();
        ChooseNextPlayer();
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