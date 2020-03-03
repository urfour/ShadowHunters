using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    private bool m_isGameOver = false;
    public bool IsGameOver
    {
        get => m_isGameOver;
        private set => m_isGameOver = value;
    }

    private List<Player> m_players;

    private GameBoard gameBoard;
    public GameBoard GameBoard { get; }

    // Cartes possibles des différents decks
    public List<VisionCard> m_visionCards;
    public List<DarknessCard> m_darknessCards;
    public List<LightCard> m_lightCards;
    public List<Card> m_locationCards;
    public List<Character> m_hunterCharacters;
    public List<Character> m_shadowCharacters;
    public List<Character> m_neutralCharacters;

    // Boutons d'interaction

    public GameObject rollDicesButton;
    public GameObject visionCardsButton;
    public GameObject darknessCardsButton;
    public GameObject lightCardsButton;
    public GameObject attackPlayer;
    public GameObject endTurn;

    void Start()
    {
        const int NB_PLAYERS = 5;
        PrepareGame(NB_PLAYERS);
        visionCardsButton.SetActive(false);
        darknessCardsButton.SetActive(false);
        lightCardsButton.SetActive(false);
        attackPlayer.SetActive(false);
        endTurn.SetActive(false);
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
        gameBoard = new GameBoard(m_locationCards.PrepareDecks<Card>(), 
            m_visionCards.PrepareDecks<VisionCard>(), m_darknessCards.PrepareDecks<DarknessCard>(), 
            m_lightCards.PrepareDecks<LightCard>(), nbPlayers);
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
        HuntersCards = cardHunters.PrepareDecks<Character>();
        ShadowsCards = cardShadows.PrepareDecks<Character>();
        NeutralsCards = cardNeutrals.PrepareDecks<Character>();

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
                carte = "Antre de l'ermite";
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
                carte = "Antre de l'ermite";
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
        switch (m_players[m_playerTurn].Position)
        {
            case Position.Antre:
                visionCardsButton.SetActive(true);
                break;
            case Position.Porte:
                Debug.Log("Quelle carte voulez-vous piocher ?");
                visionCardsButton.SetActive(true);
                darknessCardsButton.SetActive(true);
                lightCardsButton.SetActive(true);
                break;
            case Position.Monastere:
                lightCardsButton.SetActive(true);
                break;
            case Position.Cimetiere:
                darknessCardsButton.SetActive(true);
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
        attackPlayer.SetActive(true);
        endTurn.SetActive(true);
    }

    public void VisionCardLocation()
    {
        visionCardsButton.SetActive(false);
        darknessCardsButton.SetActive(false);
        lightCardsButton.SetActive(false);

        int choosenPlayerId = m_playerTurn;
        Debug.Log("Le joueur " + m_playerTurn + " choisit de piocher une carte Vision.");
        // TODO Choix du joueur à qui donner la carte vision
        while (choosenPlayerId == m_playerTurn)
            choosenPlayerId = Random.Range(0, m_nbPlayers - 1);
        VisionCard visionCard = gameBoard.DrawCard(CardType.Vision) as VisionCard;
        Debug.Log("La carte Vision a été donnée au joueur " + choosenPlayerId + ".");
        VisionCardPower(visionCard, choosenPlayerId);
        attackPlayer.SetActive(true);
        endTurn.SetActive(true);
    }

    public void DarknessCardLocation()
    {
        visionCardsButton.SetActive(false);
        darknessCardsButton.SetActive(false);
        lightCardsButton.SetActive(false);

        Debug.Log("Le joueur " + m_playerTurn + " choisit de piocher une carte Ténèbres.");
        DarknessCard darknessCard = gameBoard.DrawCard(CardType.Darkness) as DarknessCard;
        if (darknessCard.isEquipement)
        {
            m_players[m_playerTurn].AddCard(darknessCard);
            Debug.Log("La carte " + darknessCard.cardName + " a été ajoutée à la main du joueur " + m_playerTurn + ".");
        }
        DarknessCardPower(darknessCard);
        attackPlayer.SetActive(true);
        endTurn.SetActive(true);
    }

    public void LightCardLocation()
    {
        visionCardsButton.SetActive(false);
        darknessCardsButton.SetActive(false);
        lightCardsButton.SetActive(false);

        Debug.Log("Le joueur " + m_playerTurn + " pioche une carte Lumière.");
        LightCard lightCard = gameBoard.DrawCard(CardType.Light) as LightCard;
        if (lightCard.isEquipement)
        {
            m_players[m_playerTurn].AddCard(lightCard);
            Debug.Log("La carte " + lightCard.cardName + " a été ajoutée à la main du joueur " + m_playerTurn + ".");
        }
        LightCardPower(lightCard);
        attackPlayer.SetActive(true);
        endTurn.SetActive(true);
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
            if (pickedCard.visionEffect.effectOnLowHP && checkLowHPCharacters(m_players[playerId].Character.characterName))
                m_players[playerId].Wounded(1);
            else if (pickedCard.visionEffect.effectOnHighHP && checkHighHPCharacters(m_players[playerId].Character.characterName))
                m_players[playerId].Wounded(2);

            // Cas des cartes infligeant des Blessures
            else if (pickedCard.visionEffect.effectTakeWounds)
                m_players[playerId].Wounded(pickedCard.visionEffect.nbWounds);
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
        gameBoard.AddDiscard(pickedCard, CardType.Vision);
    }

    void DarknessCardPower(DarknessCard pickedCard)
    {
        int playerChoosenId = m_playerTurn;
        switch (pickedCard.darknessEffect)
        {
            case DarknessEffect.Araignee:
                // TODO choix de la personne à qui infliger des Blessures
                while (playerChoosenId == m_playerTurn)
                    playerChoosenId = Random.Range(0, m_nbPlayers - 1);
                Debug.Log("Le joueur " + playerChoosenId + " subit l'effet de l'Araignée Sanguinaire !");
                m_players[playerChoosenId].Wounded(2);
                m_players[m_playerTurn].Wounded(2);
                break;
            case DarknessEffect.Banane:
                // TODO effet de la carte
                Debug.Log("Implémentation en cours");
                break;
            case DarknessEffect.ChauveSouris:
                // TODO choix de la personne à qui infliger des Blessures
                while (playerChoosenId == m_playerTurn)
                    playerChoosenId = Random.Range(0, m_nbPlayers - 1);
                Debug.Log("Le joueur " + playerChoosenId + " subit l'effet de la Chauve-Souris Vampire !");
                m_players[playerChoosenId].Wounded(2);
                m_players[m_playerTurn].Healed(1);
                break;
            case DarknessEffect.Dynamite:
                // TODO effet de la carte
                Debug.Log("Implémentation en cours");
                break;
            case DarknessEffect.Hache:
                m_players[m_playerTurn].BonusAttack++;
                break;
            case DarknessEffect.Mitrailleuse:
                m_players[m_playerTurn].HasGatling = true;
                break;
            case DarknessEffect.Poupee:
                // TODO choix de la personne à qui infliger des Blessures
                while (playerChoosenId == m_playerTurn)
                    playerChoosenId = Random.Range(0, m_nbPlayers - 1);
                int lancer = Random.Range(1, 6);
                Debug.Log("Le lancer du dé à 6 faces donne " + lancer + ".");
                if (lancer <= 4)
                    m_players[playerChoosenId].Wounded(3);
                else
                    m_players[m_playerTurn].Wounded(3);
                break;
            case DarknessEffect.Revolver:
                m_players[m_playerTurn].HasRevolver = true;
                break;
            case DarknessEffect.Rituel:
                // TODO effet de la carte
                Debug.Log("Implémentation en cours");
                break;
            case DarknessEffect.Sabre:
                m_players[m_playerTurn].HasSaber = true;
                break;
            case DarknessEffect.Succube:
                // TODO choix de la personne à qui voler une carte équipement
                while (playerChoosenId == m_playerTurn)
                    playerChoosenId = Random.Range(0, m_nbPlayers - 1);
                Debug.Log("Le joueur " + m_playerTurn + " vole une carte équipement au joueur " + playerChoosenId + " !");
                break;
        }
        if (!pickedCard.isEquipement)
            gameBoard.AddDiscard(pickedCard, CardType.Darkness);
    }

    void LightCardPower(LightCard lightCard)
    {
        CharacterTeam team = m_players[playerId].Team;
        Character character = m_players[playerId].Character.characterName;
        bool reaveled = m_players[playerId].Reaveled;
        int playerChoosenId = m_playerTurn;
        
        switch (pickedCard.lightEffect)
        {
			case LightEffect.Amulette:
			
				Debug.Log("Implémentation en cours");
				break;
				
			case LightEffect.Ange: // A implémenter comme un équipement qui se discard au début du tour
			
				Debug.Log("Implémentation en cours");
				break;
				
			case LightEffect.Supreme:
			
				if (team == CharacterTeam.Hunter)
				{
					if (reaveled)
						m_players[m_playerTurn].SetWound(0);
					else // A le choix de se révéler ou non pour se heal
						Debug.Log("Implémentation en cours");
				}
				break;
			
			case LightEffect.Chocolat:
			
				if (character == "Allie" || character == "Emi" || character == "Métamorphe")
				{
					if (reaveled)
						m_players[m_playerTurn].SetWound(0);
					else // A le choix de se révéler ou non pour se heal
						Debug.Log("Implémentation en cours");
				}
				break;
			
			case LightEffect.Benediction:
				// TODO choix de la personne à qui infliger des Blessures
                while (playerChoosenId == m_playerTurn)
                    playerChoosenId = Random.Range(0, m_nbPlayers - 1);
                    
				break;
			
			case LightEffect.Boussole:
				break;
			
			case LightEffect.Broche:
				break;
				
			case LightEffect.Crucifix:
				break;
				
			case LightEffect.EauBenite:
				break;
				
			case LightEffect.Eclair:
				break;
			
			case LightEffect.Lance:
				break;
				
			case LightEffect.Miroir:
				break;
				
			case LightEffect.PremiersSecours:
				break;
				
			case LightEffect.Savoir:
				break;
			
			case LightEffect.Toge:
				break;
		}
        
        
        return;
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

    public void Attack()
    {
        visionCardsButton.SetActive(false);
        darknessCardsButton.SetActive(false);
        lightCardsButton.SetActive(false);
        attackPlayer.SetActive(false);
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
            m_players[playerAttackedId].Wounded(lancerTotal + m_players[m_playerTurn].BonusAttack - m_players[m_playerTurn].MalusAttack);
            if (m_players[playerAttackedId].IsDead())
            {
                Debug.Log("Le joueur " + playerAttackedId + " est mort !");
                if (m_players[playerAttackedId].Team == CharacterTeam.Hunter)
                    m_nbHuntersDead++;
                else if (m_players[playerAttackedId].Team == CharacterTeam.Shadow)
                    m_nbShadowsDeads++;
                else
                    m_nbNeutralsDeads++;
                // TODO vol d'une carte équipement
            }
        }

    }

    public void ChooseNextPlayer()
    {
        visionCardsButton.SetActive(false);
        darknessCardsButton.SetActive(false);
        lightCardsButton.SetActive(false);
        attackPlayer.SetActive(false);
        endTurn.SetActive(false);
        if (m_playerTurn == -1)
            m_playerTurn = Random.Range(0, m_nbPlayers - 1);
        else
            m_playerTurn = (m_playerTurn + 1) % m_nbPlayers;
        Debug.Log("C'est au joueur " + m_playerTurn + " de jouer.");
        rollDicesButton.SetActive(true);
    }

    void PlayTurn()
    {
        rollDicesButton.SetActive(false);
        MoveCharacter();
        ActivateLocationPower();
    }

    void HasWon(int playerId)
    {
        switch (m_players[playerId].Character.characterWinningCondition)
        {
            case WinningCondition.BeingAlive:
                if (!m_players[playerId].Dead && m_isGameOver)
                    m_players[playerId].HasWon = true;
                break;
            case WinningCondition.HavingEquipement:
                if (m_players[playerId].ListCard.Count >= 5)
                {
                    m_players[playerId].HasWon = true;
                    m_isGameOver = true;
                }
                break;
            case WinningCondition.Bryan:
                // TODO vérifier si tue un perso de 13 HP ou plus
                if (m_players[playerId].Position == Position.Sanctuaire && m_isGameOver)
                    m_players[playerId].HasWon = true;
                break;
            case WinningCondition.David:
                int nbCardsOwned = 0;
                if (m_players[playerId].hasCard("Crucifix en argent"))
                    nbCardsOwned++;
                if (m_players[playerId].hasCard("Amulette"))
                    nbCardsOwned++;
                if (m_players[playerId].hasCard("Lance de Longinus"))
                    nbCardsOwned++;
                if (m_players[playerId].hasCard("Toge sainte"))
                    nbCardsOwned++;
                
                if (nbCardsOwned >= 3)
                {
                    m_players[playerId].HasWon = true;
                    m_isGameOver = true;
                }
                break;
            case WinningCondition.HunterCondition:
                if (m_nbShadowsDeads == m_nbShadows)
                {
                    m_players[playerId].HasWon = true;
                    m_isGameOver = true;
                } 
                break;
            case WinningCondition.ShadowCondition:
                if (m_nbHuntersDead == m_nbHunters || m_nbNeutralsDeads == 3)
                {
                    m_players[playerId].HasWon = true;
                    m_isGameOver = true;
                }
                break;
        }

    }

    void playerCardPower(Character character)
    {
        switch(character.characterName)
        {
            case "Allie":
                break;
            case "Bryan":
                break;
            case "David":
                break;
            case "Emi":
                break;
            case "Metamorphe":
                break;
            case "Bob":
                break;
            case "Franklin":
                break;
            case "Georges":
                break;
            case "Loup-Garou":
                break;
            case "Vampire":
                break;
        }
    }
}
