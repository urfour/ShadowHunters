using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

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
    public List<LocationCard> m_locationCards;
    public List<Character> m_hunterCharacters;
    public List<Character> m_shadowCharacters;
    public List<Character> m_neutralCharacters;

    // Boutons d'interaction
    public Dropdown choiceDropdown; 
    public Button rollDicesButton;
    public Button visionCardsButton;
    public Button darknessCardsButton;
    public Button lightCardsButton;
    public Button attackPlayer;
    public Button endTurn;
    public Button revealCardButton;
    public Button validateButton;
    public Toggle woundsForestToggle;
    public Toggle healForestToggle;

    void Start()
    {
        const int NB_PLAYERS = 5;
        PrepareGame(NB_PLAYERS);
        visionCardsButton.gameObject.SetActive(false);
        darknessCardsButton.gameObject.SetActive(false);
        lightCardsButton.gameObject.SetActive(false);
        attackPlayer.gameObject.SetActive(false);
        endTurn.gameObject.SetActive(false);
        choiceDropdown.gameObject.SetActive(false);
        validateButton.gameObject.SetActive(false);
        woundsForestToggle.gameObject.gameObject.SetActive(false);
        healForestToggle.gameObject.gameObject.SetActive(false);
        ChooseNextPlayer();

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
        gameBoard = new GameBoard(m_locationCards.PrepareDecks<LocationCard>(), 
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

    IEnumerator MoveCharacter()
    {
        int lancer1, lancer2, lancerTotal;
        Position position = Position.None;

        if(m_players[m_playerTurn].Character.characterType == CharacterType.Emi && m_players[m_playerTurn].Revealed)
        {
            // TODO Le joueur choisit s'il veut utiliser son pouvoir
            int usePowerEmi = UnityEngine.Random.Range(0, 1);

            if(usePowerEmi == 1)
                PlayerCardPower(m_players[m_playerTurn]);
        }

        while (position == Position.None)
        {
            lancer1 = UnityEngine.Random.Range(1, 6);
            lancer2 = UnityEngine.Random.Range(1, 4);
            lancerTotal = lancer1 + lancer2;
            Debug.Log("Le lancer de dés donne " + lancer1 + " et " + lancer2 + " (" + lancerTotal + ").");
            switch (lancerTotal)
            {
                case 2:
                case 3:
                    position = Position.Antre;
                    break;
                case 4:
                case 5:
                    position = Position.Porte;
                    break;
                case 6:
                    position = Position.Monastere;
                    break;
                case 7:
                    Debug.Log("Où souhaitez-vous aller ?");
                    yield return StartCoroutine(ChooseLocation());
                    yield break;
                case 8:
                    position = Position.Cimetiere;
                    break;
                case 9:
                    position = Position.Foret;
                    break;
                case 10:
                    position = Position.Foret;
                    break;
            }
            if (m_players[m_playerTurn].Position != position)
            {
                m_players[m_playerTurn].Position = position;
                gameBoard.setPositionOfAt(m_playerTurn, position);
            }
            else
                position = Position.None;
        }
        Debug.Log("Le joueur " + m_players[m_playerTurn].Name + " se rend sur la carte " + 
            gameBoard.GetAreaNameByPosition(m_players[m_playerTurn].Position) + ".");

    }

    IEnumerator ActivateLocationPower()
    {
        switch (m_players[m_playerTurn].Position)
        {
            // Possibilité de piocher une carte vision
            case Position.Antre:
                visionCardsButton.gameObject.SetActive(true);
                break;
            // Possibilité de choisir quelle carte piocher
            case Position.Porte:
                Debug.Log("Quelle carte voulez-vous piocher ?");
                visionCardsButton.gameObject.SetActive(true);
                darknessCardsButton.gameObject.SetActive(true);
                lightCardsButton.gameObject.SetActive(true);
                break;
            // Possibilité de piocher une carte lumière
            case Position.Monastere:
                lightCardsButton.gameObject.SetActive(true);
                break;
            // Possibilité de piocher une carte ténèbres
            case Position.Cimetiere:
                darknessCardsButton.gameObject.SetActive(true);
                break;
            // Choix d'une personne à qui infliger 2 Blessures ou en soigner une
            case Position.Foret:
                bool isWoundsForestEffect;
                Debug.Log("Souhaitez-vous infliger des Blessures ou en soigner ?");
                woundsForestToggle.gameObject.SetActive(true);
                healForestToggle.gameObject.SetActive(true);
                validateButton.gameObject.SetActive(true);
                yield return WaitUntilEvent(validateButton.onClick);

                if (woundsForestToggle.isOn)
                {
                    isWoundsForestEffect = true;
                    Debug.Log("Vous choisissez d'infliger 2 Blessures.");
                }
                else
                {
                    isWoundsForestEffect = false;
                    Debug.Log("Vous choisissez de soigner 1 Blessure.");
                }
                woundsForestToggle.gameObject.SetActive(false);
                healForestToggle.gameObject.SetActive(false);
                List<string> players = new List<string>();
                foreach (Player player in m_players)
                    if (!player.IsDead())
                        players.Add(player.Name);

                Debug.Log("A qui souhaitez-vous appliquer cet effet ?");
                choiceDropdown.gameObject.SetActive(true);
                choiceDropdown.AddOptions(players);
                yield return WaitUntilEvent(validateButton.onClick);

                string playerAttacked = choiceDropdown.captionText.text;
                choiceDropdown.ClearOptions();
                choiceDropdown.gameObject.SetActive(false);
                validateButton.gameObject.SetActive(false);
                int playerAttackedId = -1;
                for (int i = 0 ; i < m_nbPlayers ; i++)
                    if (m_players[i].Name.Equals(playerAttacked))
                        playerAttackedId = i;

                if (isWoundsForestEffect)
                {
                    m_players[playerAttackedId].Wounded(2);
                    CheckPlayerDeath(playerAttackedId);
                }
                else
                    m_players[playerAttackedId].Healed(1);
                break;
            case Position.Sanctuaire:
            // Possibilité de voler une carte équipement
                yield return StartCoroutine(StealEquipmentCard(m_playerTurn));
                break;
        }
        attackPlayer.gameObject.SetActive(true);
        if(m_players[m_playerTurn].HasSaber==true)   
            endTurn.gameObject.SetActive(false);
        else
            endTurn.gameObject.SetActive(true);
    }
    
    public void MoveVisionCardLocation()
    {
        StartCoroutine(VisionCardLocation());
    }

    IEnumerator VisionCardLocation()
    {
        visionCardsButton.gameObject.SetActive(false);
        darknessCardsButton.gameObject.SetActive(false);
        lightCardsButton.gameObject.SetActive(false);
        attackPlayer.gameObject.SetActive(false);
        endTurn.gameObject.SetActive(false);

        int choosenPlayerId = m_playerTurn;
        Debug.Log("Le joueur " + m_players[m_playerTurn].Name + " choisit de piocher une carte Vision.");
        VisionCard visionCard = gameBoard.DrawCard(CardType.Vision) as VisionCard;
        yield return StartCoroutine(VisionCardPower(visionCard));
        gameBoard.AddDiscard(visionCard, CardType.Vision);
        attackPlayer.gameObject.SetActive(true);
        if(m_players[m_playerTurn].HasSaber==true)
            endTurn.gameObject.SetActive(false);
        else
            endTurn.gameObject.SetActive(true);
    }

    public void MoveDarknessCardLocation()
    {
        StartCoroutine(DarknessCardLocation());
    }

    IEnumerator DarknessCardLocation()
    {
        visionCardsButton.gameObject.SetActive(false);
        darknessCardsButton.gameObject.SetActive(false);
        lightCardsButton.gameObject.SetActive(false);
        attackPlayer.gameObject.SetActive(false);
        endTurn.gameObject.SetActive(false);

        Debug.Log("Le joueur " + m_players[m_playerTurn].Name + " choisit de piocher une carte Ténèbres.");
        DarknessCard darknessCard = gameBoard.DrawCard(CardType.Darkness) as DarknessCard;
        if (darknessCard.isEquipement)
        {
            m_players[m_playerTurn].AddCard(darknessCard);
            Debug.Log("La carte " + darknessCard.cardName + " a été ajoutée à la main du joueur " 
                + m_players[m_playerTurn].Name + ".");
        }
        yield return StartCoroutine(DarknessCardPower(darknessCard));
        gameBoard.AddDiscard(darknessCard, CardType.Darkness);
        attackPlayer.gameObject.SetActive(true);
        if(m_players[m_playerTurn].HasSaber==true)
            endTurn.gameObject.SetActive(false);
        else
            endTurn.gameObject.SetActive(true);
    }

    public void MoveLightCardLocation()
    {
        StartCoroutine(LightCardLocation());
    }

    IEnumerator LightCardLocation()
    {
        visionCardsButton.gameObject.SetActive(false);
        darknessCardsButton.gameObject.SetActive(false);
        lightCardsButton.gameObject.SetActive(false);
        attackPlayer.gameObject.SetActive(false);
        endTurn.gameObject.SetActive(false);

        Debug.Log("Le joueur " + m_players[m_playerTurn].Name + " pioche une carte Lumière.");
        LightCard lightCard = gameBoard.DrawCard(CardType.Light) as LightCard;
        if (lightCard.isEquipement)
        {
            m_players[m_playerTurn].AddCard(lightCard);
            Debug.Log("La carte " + lightCard.cardName + " a été ajoutée à la main du joueur " 
                + m_players[m_playerTurn].Name + ".");
        }
        yield return StartCoroutine(LightCardPower(lightCard));
        gameBoard.AddDiscard(lightCard, CardType.Light);
        attackPlayer.gameObject.SetActive(true);
        if(m_players[m_playerTurn].HasSaber==true)
            endTurn.gameObject.SetActive(false);
        else
            endTurn.gameObject.SetActive(true);
    }

    IEnumerator VisionCardPower(VisionCard pickedCard)
    {
        Debug.Log("Message au joueur " + m_players[m_playerTurn].Name + " : ");
        Debug.Log("Carte Vision piochée : " + pickedCard.cardName);
        Debug.Log(pickedCard.description);
        choiceDropdown.gameObject.SetActive(true);
        validateButton.gameObject.SetActive(true);
        List<string> players = new List<string>();
        for (int i = 0 ; i < m_nbPlayers ; i++)
            if (!m_players[i].IsDead() && i != m_playerTurn)
                players.Add(m_players[i].Name);            
        choiceDropdown.AddOptions(players);
        Debug.Log("A qui voulez-vous donner cette carte vision ?");
        yield return WaitUntilEvent(validateButton.onClick);

        validateButton.gameObject.SetActive(false);
        int playerId = -1;
        string choosenPlayer = choiceDropdown.captionText.text;
        choiceDropdown.ClearOptions();
        choiceDropdown.gameObject.SetActive(false);
        for (int i = 0 ; i < m_nbPlayers ; i++)
            if (m_players[i].Name.Equals(choosenPlayer))
                playerId = i;

        Debug.Log("La carte Vision a été donnée au joueur " + choosenPlayer + ".");
        CharacterTeam team = m_players[playerId].Team;

        // Le metamorphe peut mentir
        bool metamorph = false;
        if(m_players[playerId].Character.characterType == CharacterType.Metamorphe)
        {
            // TODO Le métamorphe choisit de mentir
            if(UnityEngine.Random.Range(0, 1) == 0)
                metamorph = true;
        }
        
        // Cartes applicables en fonction des équipes ?
        if ((team == CharacterTeam.Shadow && !pickedCard.visionEffect.effectOnShadow)
            || (team == CharacterTeam.Hunter && !pickedCard.visionEffect.effectOnHunter)
            || (team == CharacterTeam.Neutral && !pickedCard.visionEffect.effectOnNeutral)
            || (metamorph && pickedCard.visionEffect.effectOnShadow))
        {
            if (pickedCard.visionEffect.effectSupremeVision)
                //TODO montrer la carte personnage
                Debug.Log("C'est une carte Vision Suprême !");
            else
                Debug.Log("Rien ne se passe.");
        }
        else
        {
            // Cas des cartes applicables en fonction des points de vie
            if (pickedCard.visionEffect.effectOnLowHP && CheckLowHPCharacters(m_players[playerId].Character.characterName))
            {
                m_players[playerId].Wounded(1);
                CheckPlayerDeath(playerId);
            }
            else if (pickedCard.visionEffect.effectOnHighHP && CheckHighHPCharacters(m_players[playerId].Character.characterName))
            {
                m_players[playerId].Wounded(2);
                CheckPlayerDeath(playerId);
            }

            // Cas des cartes infligeant des Blessures
            else if (pickedCard.visionEffect.effectTakeWounds)
            {
                m_players[playerId].Wounded(pickedCard.visionEffect.nbWounds);
                CheckPlayerDeath(playerId);
            }
            // Cas des cartes soignant des Blessures
            else if (pickedCard.visionEffect.effectHealingOneWound)
            {
                if (m_players[playerId].Wound == 0)
                {
                    m_players[playerId].Wounded(1);
                    CheckPlayerDeath(playerId);
                }
                else
                {
                    m_players[playerId].Healed(1);
                    CheckPlayerDeath(playerId);
                }
            }
            // Cas des cartes volant une carte équipement ou infligeant des Blessures
            else if (pickedCard.visionEffect.effectGivingEquipementCard)
            {
                if (m_players[playerId].ListCard.Count == 0)
                {
                    Debug.Log("Vous ne possédez pas de carte équipement.");
                    m_players[playerId].Wounded(1);
                }
                else
                // TODO don d'une carte équipement
                {
                    GiveEquipmentCard(playerId);
                }
            }
            else
                Debug.Log("Rien ne se passe.");
        }
    }

    IEnumerator DarknessCardPower(DarknessCard pickedCard)
    {
        switch (pickedCard.darknessEffect)
        {
            case DarknessEffect.Araignee:
                yield return StartCoroutine(TakingWoundsEffect(false, 2, -2));
                break;
            case DarknessEffect.Banane:
                yield return StartCoroutine(GiveEquipmentCard(m_playerTurn));
                break;
            case DarknessEffect.ChauveSouris:
                yield return StartCoroutine(TakingWoundsEffect(false, 2, 1));
                break;
            case DarknessEffect.Dynamite:
                int lancer1 = UnityEngine.Random.Range(1, 6);
                int lancer2 = UnityEngine.Random.Range(1, 4);
                int lancerTotal=lancer1+lancer2;
                Position area=Position.None;

                switch(lancerTotal)
                {
                    case 2:
                    case 3:
                        area=Position.Antre;
                        break;
                    case 4:
                    case 5:
                        area=Position.Porte;
                        break;
                    case 6:
                        area=Position.Monastere;
                        break;
                    case 7:
                        Debug.Log("Rien ne se passe");
                        break;
                    case 8:
                        area=Position.Cimetiere;
                        break;
                    case 9:
                        area=Position.Foret;
                        break;
                    case 10:
                        area=Position.Sanctuaire;
                        break;
                }
                if(lancerTotal!=7)
                {
                    foreach (Player p in m_players)
                    {
                        if(p.Position==area)
                            p.Wounded(3);
                    }
                }
                break;
            case DarknessEffect.Hache:
                m_players[m_playerTurn].BonusAttack++;
                break;
            case DarknessEffect.Mitrailleuse:
                m_players[m_playerTurn].HasGatling = true;
                break;
            case DarknessEffect.Poupee:
                yield return StartCoroutine(TakingWoundsEffect(true, 3, 0));
                break;
            case DarknessEffect.Revolver:
                m_players[m_playerTurn].HasRevolver = true;
                break;
            case DarknessEffect.Rituel:
                Debug.Log("Voulez-vous vous révéler ? Vous avez 6 secondes, sinon la carte se défausse.");
                yield return new WaitForSeconds(6f);
                if (m_players[m_playerTurn].Revealed && m_players[m_playerTurn].Team== CharacterTeam.Shadow)
                {
                    m_players[m_playerTurn].Healed(m_players[m_playerTurn].Wound);
                    Debug.Log("Le joueur "+ m_players[m_playerTurn].Name + " se soigne complètement");
                }
                else
                {
                    Debug.Log("Rien ne se passe.");
                }
                break;
            case DarknessEffect.Sabre:
                m_players[m_playerTurn].HasSaber = true;
                break;
            case DarknessEffect.Succube:
                yield return StartCoroutine(StealEquipmentCard(m_playerTurn));
                break;
        }
    }

    IEnumerator TakingWoundsEffect(bool isPuppet, int nbWoundsTaken, int nbWoundsSelfHealed)
    {
        choiceDropdown.gameObject.SetActive(true);
        validateButton.gameObject.SetActive(true);
        List<string> players = new List<string>();
        foreach (Player player in m_players)
            if (!player.IsDead() && player.Id != m_playerTurn)
                players.Add(player.Name);

        Debug.Log("A qui souhaitez-vous infliger des Blessures ?");
        choiceDropdown.AddOptions(players);
        yield return WaitUntilEvent(validateButton.onClick);

        string playerChoosen = choiceDropdown.captionText.text;
        choiceDropdown.ClearOptions();
        choiceDropdown.gameObject.SetActive(false);
        validateButton.gameObject.SetActive(false);
        int playerChoosenId = -1;
        foreach (Player player in m_players)
            if (player.Name.Equals(playerChoosen))
                playerChoosenId = player.Id;
        
        if(isPuppet)
        {
            int lancer = UnityEngine.Random.Range(1, 6);
            Debug.Log("Le lancer donne " + lancer + ".");
            if(lancer<=4)
            {
                m_players[playerChoosenId].Wounded(nbWoundsTaken);
                CheckPlayerDeath(playerChoosenId);
            }
            else
            {
                m_players[m_playerTurn].Wounded(nbWoundsTaken);
                CheckPlayerDeath(m_playerTurn);
            }
        }
        else
        {
            m_players[playerChoosenId].Wounded(nbWoundsTaken);
            CheckPlayerDeath(playerChoosenId);
            if (nbWoundsSelfHealed < 0)
            {
                m_players[m_playerTurn].Wounded(-nbWoundsSelfHealed);
                CheckPlayerDeath(m_playerTurn);
            }
            else
                m_players[m_playerTurn].Healed(nbWoundsSelfHealed);
        }
    }

    IEnumerator LightCardPower(LightCard lightCard)
    {
        CharacterTeam team = m_players[m_playerTurn].Team;
        string character = m_players[m_playerTurn].Character.characterName;
        bool reaveled = m_players[m_playerTurn].Revealed;
        
        switch (lightCard.lightEffect)
        {
			case LightEffect.Amulette:
			
				Debug.Log("Implémentation en cours");
				break;
				
			case LightEffect.AngeGardien: // A implémenter comme un équipement qui se discard au début du tour
			
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
			
				// TODO choix de la personne à heal (pas soi même)
                int playerChoosenId = m_playerTurn;
                while (playerChoosenId == m_playerTurn)
                    playerChoosenId = UnityEngine.Random.Range(0, m_nbPlayers - 1);
                Debug.Log("Vous choisissez de soigner le joueur " + m_players[playerChoosenId].Name + ".");
                
                int heal = UnityEngine.Random.Range(1, 6);
                
                m_players[playerChoosenId].Healed(heal);
                
				break;
			
			case LightEffect.Boussole:
				
				Debug.Log("Implémentation en cours");
				break;
			
			case LightEffect.Broche:
			
				Debug.Log("Implémentation en cours");
				break;
				
			case LightEffect.Crucifix:
			
				Debug.Log("Implémentation en cours");
				break;
				
			case LightEffect.EauBenite:
				
				m_players[m_playerTurn].Healed(2);
				break;
				
			case LightEffect.Eclair:
			
				foreach (Player p in m_players)
				{
					if (p.Id != m_playerTurn)
						p.Wounded(2);
				}
				break;
			
			case LightEffect.Lance:
				
				Debug.Log("Implémentation en cours");
				break;
				
			case LightEffect.Miroir:
				
				if (team == CharacterTeam.Shadow && character != "Métamorphe")
				{
					m_players[m_playerTurn].Revealed = true;
					Debug.Log("Vous révélez votre rôle à tous, vous êtes : " + character);
				}
				break;
				
			case LightEffect.PremiersSecours:
				
				// TODO : méthode pour choisir le joueur à mettre à 7 Blessures
				playerChoosenId = UnityEngine.Random.Range(0, m_nbPlayers);
				m_players[playerChoosenId].SetWound(7);
				break;
				
			case LightEffect.Savoir: // A implémenter comme un équipement qui se discard au début du tour ou à la mort
			
				Debug.Log("Implémentation en cours");
				break;
			
			case LightEffect.Toge:
				
				m_players[m_playerTurn].MalusAttack++;
				m_players[m_playerTurn].ReductionWounds = 1;
				break;
		}
        yield return null;
    }

    bool CheckLowHPCharacters(string characterName)
    {
        return characterName.StartsWith("A") || characterName.StartsWith("B")
            || characterName.StartsWith("C") || characterName.StartsWith("E")
            || characterName.StartsWith("M");
    }

    bool CheckHighHPCharacters(string characterName)
    {
        return characterName.StartsWith("D") || characterName.StartsWith("F")
            || characterName.StartsWith("G") || characterName.StartsWith("L")
            || characterName.StartsWith("V");
    }

    public void RevealCard()
    {
        m_players[m_playerTurn].Revealed=true;
        Debug.Log("Le joueur "+ m_players[m_playerTurn].Name + " s'est révélé, il s'agissait de : " 
            + m_players[m_playerTurn].Character.characterName + " ! Il est dans l'équipe des " 
            + m_players[m_playerTurn].Character.team + ".");
        revealCardButton.gameObject.SetActive(false);
    }

    public void Attack()
    {
        attackPlayer.gameObject.SetActive(false);
        endTurn.gameObject.SetActive(false);
        visionCardsButton.gameObject.SetActive(false);
        darknessCardsButton.gameObject.SetActive(false);
        lightCardsButton.gameObject.SetActive(false);

        Debug.Log("Quel joueur souhaitez-vous attaquer ?");
        StartCoroutine(AttackCorrespondingPlayer(m_playerTurn));
    }

    public void ChooseNextPlayer()
    {
        visionCardsButton.gameObject.SetActive(false);
        darknessCardsButton.gameObject.SetActive(false);
        lightCardsButton.gameObject.SetActive(false);
        attackPlayer.gameObject.SetActive(false);
        endTurn.gameObject.SetActive(false);

        if (m_playerTurn == -1)
            m_playerTurn = UnityEngine.Random.Range(0, m_nbPlayers - 1);
        else
            m_playerTurn = (m_playerTurn + 1) % m_nbPlayers;
        Debug.Log("C'est au joueur " + m_players[m_playerTurn].Name + " de jouer.");
        rollDicesButton.gameObject.SetActive(true);
        if(m_players[m_playerTurn].Revealed==false)
        {
            revealCardButton.gameObject.SetActive(true);
        }
    }

    public void RollTheDices()
    {
        StartCoroutine(PlayTurn());
    }

    IEnumerator PlayTurn()
    {
        rollDicesButton.gameObject.SetActive(false);
        if(m_players[m_playerTurn].Character.characterType == CharacterType.Franklin)
            PlayerCardPower(m_players[m_playerTurn]);    
        if(m_players[m_playerTurn].Character.characterType == CharacterType.Georges)
            PlayerCardPower(m_players[m_playerTurn]);    

        yield return StartCoroutine(MoveCharacter());
        yield return StartCoroutine(ActivateLocationPower());
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
                if (m_players[playerId].HasCard("Crucifix en argent") != -1)
                    nbCardsOwned++;
                if (m_players[playerId].HasCard("Amulette") != -1)
                    nbCardsOwned++;
                if (m_players[playerId].HasCard("Lance de Longinus") != -1)
                    nbCardsOwned++;
                if (m_players[playerId].HasCard("Toge sainte") != -1)
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

    IEnumerator ChooseLocation()
    {
        choiceDropdown.gameObject.SetActive(true);
        validateButton.gameObject.SetActive(true);
        List<string> locationNames = new List<string>();
        foreach (LocationCard location in gameBoard.Areas)
            if (location.area != m_players[m_playerTurn].Position)
                locationNames.Add(location.cardName);

        choiceDropdown.AddOptions(locationNames);
        yield return StartCoroutine(WaitUntilEvent(validateButton.onClick));

        string selectedLocation = choiceDropdown.captionText.text;
        choiceDropdown.ClearOptions();
        foreach (LocationCard location in gameBoard.Areas)
            if (location.cardName.Equals(selectedLocation))
            {
                m_players[m_playerTurn].Position = location.area;
                gameBoard.setPositionOfAt(m_playerTurn, location.area);
                Debug.Log("Le joueur " + m_players[m_playerTurn].Name + " se rend sur la carte " + location.cardName + ".");
            }
            choiceDropdown.gameObject.SetActive(false);
            validateButton.gameObject.SetActive(false);
    }

    public List<Player> GetPlayersSameSector(int playerId, bool hasRevolver)
    {
        int positionIndex = gameBoard.GetIndexOfPosition(m_players[playerId].Position);
        int positionOtherPlayer;
        List<Player> players = new List<Player>();
        foreach (Player player in m_players)
        {
            if (!player.IsDead() && player.Id != playerId && player.Position != Position.None)
            {
                positionOtherPlayer = gameBoard.GetIndexOfPosition(player.Position);
                if ((positionIndex % 2 == 0 && (positionOtherPlayer == positionIndex || positionOtherPlayer == positionIndex + 1))
                    || (positionIndex % 2 == 1 && (positionOtherPlayer == positionIndex || positionOtherPlayer == positionIndex - 1)))
                    if (!hasRevolver)
                        players.Add(player);
                else if (hasRevolver)
                    players.Add(player);
            }
        }

        return players; 

    }

    IEnumerator AttackCorrespondingPlayer(int playerAttackingId)
    {
        choiceDropdown.gameObject.SetActive(true);
        List<Player> players = GetPlayersSameSector(playerAttackingId, m_players[playerAttackingId].HasRevolver);
        if (players.Count == 0)
        {
            Debug.Log("Vous ne pouvez attaquer aucun joueur.");
            choiceDropdown.gameObject.SetActive(false);
        }
        else
        {
            List<string> playerNames = new List<string>();
            foreach (Player player in players)
                playerNames.Add(player.Name);

            choiceDropdown.AddOptions(playerNames);
            validateButton.gameObject.SetActive(true);
            yield return WaitUntilEvent(validateButton.onClick); 
            int playerAttackedId = -1;
            choiceDropdown.gameObject.SetActive(false);
            validateButton.gameObject.SetActive(false);
            string playerAttacked = choiceDropdown.captionText.text;
            choiceDropdown.ClearOptions();

            int lancer1 = UnityEngine.Random.Range(1, 6);
            int lancer2 = UnityEngine.Random.Range(1, 4);
            int lancerTotal = (m_players[playerAttackingId].HasSaber==true)? lancer2 : Mathf.Abs(lancer1 - lancer2);
            if (lancerTotal == 0)
            {
                Debug.Log("Le lancer vaut 0, vous n'attaquez pas.");
            }
            else
            {
                if (m_players[playerAttackingId].HasGatling)
                {
                    foreach (Player player in players)
                    {
                        if((m_players[playerAttackingId].Character.characterType == CharacterType.Bob) && lancerTotal >= 2 )
                            PlayerCardPower(m_players[playerAttackingId]);
                        else
                        {
                            m_players[player.Id].Wounded(lancerTotal + m_players[playerAttackingId].BonusAttack - m_players[playerAttackingId].MalusAttack);
                            CheckPlayerDeath(player.Id);
                            if(m_players[player.Id].Character.characterType == CharacterType.LoupGarou)
                                PlayerCardPower(m_players[player.Id]);
                            if(m_players[player.Id].Character.characterType == CharacterType.Vampire)
                                PlayerCardPower(m_players[player.Id]);  
                        }    
                    }
                }
                else
                {
                    for (int i = 0 ; i < m_nbPlayers ; i++)
                        if (m_players[i].Name.Equals(playerAttacked))
                            playerAttackedId = i;

                    Debug.Log("Vous choisissez d'attaquer le joueur " + m_players[playerAttackedId].Name + ".");

                    if(m_players[playerAttackingId].Character.characterType == CharacterType.Bob && lancerTotal >= 2 )
                        PlayerCardPower(m_players[playerAttackingId]);
                    else
                    {
                        m_players[playerAttackedId].Wounded(lancerTotal + m_players[playerAttackingId].BonusAttack - m_players[playerAttackingId].MalusAttack);
                        if(m_players[playerAttackedId].Character.characterType == CharacterType.LoupGarou)
                            PlayerCardPower(m_players[playerAttackedId]);
                        if(m_players[playerAttackedId].Character.characterType == CharacterType.Vampire)
                            PlayerCardPower(m_players[playerAttackedId]);  
                        CheckPlayerDeath(playerAttackedId);
                    }    
                }
            }
        }
        endTurn.gameObject.SetActive(true);
    }

    void CheckPlayerDeath(int playerId)
    {
        if (m_players[playerId].IsDead())
        {
            Debug.Log("Le joueur " + m_players[m_playerTurn].Name + " est mort !");

            if(m_nbHuntersDead == 0 && m_nbShadowsDeads == 0 && m_nbNeutralsDeads == 0)
            {
                foreach(Player player in m_players)
                {
                    if(player.Character.characterType == CharacterType.Daniel)
                    {
                        PlayerCardPower(player);
                    }
                }
            }

            if (m_players[playerId].Team == CharacterTeam.Hunter)
                m_nbHuntersDead++;
            else if (m_players[playerId].Team == CharacterTeam.Shadow)
                m_nbShadowsDeads++;
            else
                m_nbNeutralsDeads++;

            // if(m_players[m_playerTurn].Character.characterType == CharacterType.Bryan && m_players[playerId].Life <= 12 && !m_players[m_playerTurn].Revealed)
            // {
            //     PlayerCardPower(m_players[m_playerTurn]);
            // }
        }
    }

    IEnumerator CheckKilledPlayer(int attackerId, int playerId)
    {
        if (m_players[playerId].IsDead())
        {
            Debug.Log("Le joueur " + m_players[attackerId].Name + " a tué le joueur " + m_players[playerId].Name + " !");
            if(m_nbHuntersDead == 0 && m_nbShadowsDeads == 0 && m_nbNeutralsDeads == 0)
            {
                foreach(Player player in m_players)
                {
                    if(player.Character.characterType == CharacterType.Daniel)
                    {
                        PlayerCardPower(player);
                    }
                }
            }

            if (m_players[playerId].Team == CharacterTeam.Hunter)
                m_nbHuntersDead++;
            else if (m_players[playerId].Team == CharacterTeam.Shadow)
                m_nbShadowsDeads++;
            else
                m_nbNeutralsDeads++;

            // if(m_players[m_playerTurn].Character.characterType == CharacterType.Bryan && m_players[playerId].Life <= 12 && !m_players[m_playerTurn].Revealed)
            // {
            //     PlayerCardPower(m_players[m_playerTurn]);
            // }
            if (m_players[playerId].ListCard.Count != 0)
                yield return StartCoroutine(StealEquipmentCard(attackerId, playerId));
        }
    }

    IEnumerator StealEquipmentCard(int thiefId)
    {
        choiceDropdown.gameObject.SetActive(true);
        validateButton.gameObject.SetActive(true);
        List<string> choices = new List<string>();
        foreach (Player player in m_players)
            if (!player.IsDead() && player.Id != thiefId && player.Id != thiefId && player.ListCard.Count > 0)
                choices.Add(player.Name);

        if (choices.Count == 0)
        {
            choiceDropdown.gameObject.SetActive(false);
            validateButton.gameObject.SetActive(false);
            Debug.Log("Il n'y a aucun joueur à qui voler une carte équipement.");
        }
        else
        {
            int playerId = -1;
            Debug.Log("A qui voulez-vous voler une carte équipement ?");
            choiceDropdown.AddOptions(choices);
            yield return WaitUntilEvent(validateButton.onClick);

            string choosenPlayer = choiceDropdown.captionText.text;
            choiceDropdown.ClearOptions();
            validateButton.gameObject.SetActive(false);
            foreach (Player p in m_players)
                if (p.Name.Equals(choosenPlayer))
                    playerId = p.Id;

            choices.Clear();
            for (int i = 0 ; i < m_players[playerId].ListCard.Count ; i++)
                choices.Add(m_players[playerId].ListCard[i].cardName);

            Debug.Log("Quelle carte équipement voulez-vous voler ?");
            choiceDropdown.AddOptions(choices);
            yield return WaitUntilEvent(validateButton.onClick);

            string stealedCard = choiceDropdown.captionText.text;
            choiceDropdown.ClearOptions();
            choiceDropdown.gameObject.SetActive(false);
            validateButton.gameObject.SetActive(false);
            int indexCard = m_players[playerId].HasCard(stealedCard);
            if (indexCard == -1)
            {
                Debug.LogError("Erreur : la carte choisie est invalide.");
                yield return -1;
            }
            else
            {
                m_players[thiefId].AddCard(m_players[playerId].ListCard[indexCard]);
                if (m_players[playerId].ListCard[indexCard].cardType == CardType.Darkness
                    && m_players[playerId].ListCard[indexCard].isEquipement)
                    yield return StartCoroutine(DarknessCardPower(m_players[thiefId].ListCard[m_players[thiefId].ListCard.Count - 1] as DarknessCard));
                else if (m_players[playerId].ListCard[indexCard].cardType == CardType.Light
                    && m_players[playerId].ListCard[indexCard].isEquipement)
                    LightCardPower(m_players[playerId].ListCard[indexCard] as LightCard);
                    
                m_players[playerId].RemoveCard(indexCard);
                Debug.Log("La carte " + stealedCard + " a été volée au joueur "
                    + m_players[playerId].Name + " par le joueur " + m_players[thiefId].Name + " !");
            }
        }
    }

    IEnumerator StealEquipmentCard(int thiefId, int playerId)
    {
        validateButton.gameObject.SetActive(true);
        List<string> choices = new List<string>();
        for (int i = 0 ; i < m_players[playerId].ListCard.Count ; i++)
            choices.Add(m_players[playerId].ListCard[i].cardName);

        Debug.Log("Quelle carte équipement voulez-vous voler ?");
        choiceDropdown.AddOptions(choices);
        yield return StartCoroutine(WaitUntilEvent(validateButton.onClick));

        string stealedCard = choiceDropdown.captionText.text;
        choiceDropdown.ClearOptions();
        choiceDropdown.gameObject.SetActive(false);
        validateButton.gameObject.SetActive(false);
        int indexCard = m_players[playerId].HasCard(stealedCard);
        if (indexCard == -1)
        {
            Debug.LogError("Erreur : la carte choisie est invalide.");
            yield return -1;
        }
        else
        {
            m_players[thiefId].AddCard(m_players[playerId].ListCard[indexCard]);
            if (m_players[playerId].ListCard[indexCard].cardType == CardType.Darkness
                && m_players[playerId].ListCard[indexCard].isEquipement)
                yield return StartCoroutine(DarknessCardPower(m_players[thiefId].ListCard[m_players[thiefId].ListCard.Count - 1] as DarknessCard));
            else if (m_players[playerId].ListCard[indexCard].cardType == CardType.Light
                && m_players[playerId].ListCard[indexCard].isEquipement)
                LightCardPower(m_players[playerId].ListCard[indexCard] as LightCard);
                
            m_players[playerId].RemoveCard(indexCard);
            Debug.Log("La carte " + stealedCard + " a été volée au joueur " 
                + m_players[playerId].Name + " par le joueur " + m_players[thiefId].Name + " !");
        }
    }

    IEnumerator GiveEquipmentCard (int giverPlayerId)
    {
        if (m_players[giverPlayerId].ListCard.Count == 0)
        {
            choiceDropdown.gameObject.SetActive(false);
            Debug.Log("Vous ne possédez aucune carte équipement.");
            m_players[giverPlayerId].Wounded(1);
            CheckPlayerDeath(giverPlayerId);
        }
        else
        {
            int playerId = -1;
            validateButton.gameObject.SetActive(true);
            List<string> choices = new List<string>();
            foreach (Player player in m_players)
                if (!player.IsDead() && player.Id != giverPlayerId)
                    choices.Add(player.Name);

            Debug.Log("A qui souhaitez-vous donner une carte équipement ?");
            choiceDropdown.AddOptions(choices);
            yield return StartCoroutine(WaitUntilEvent(validateButton.onClick));

            string choosenPlayer = choiceDropdown.captionText.text;
            choiceDropdown.ClearOptions();
            validateButton.gameObject.SetActive(false);
            foreach (Player p in m_players)
                if (p.Name.Equals(choosenPlayer))
                    playerId = p.Id;

            choices.Clear();
            foreach (Card card in m_players[giverPlayerId].ListCard)
                choices.Add(card.cardName);

            Debug.Log("Quelle carte équipement souhaitez-vous donner ?");
            choiceDropdown.AddOptions(choices);
            yield return StartCoroutine(WaitUntilEvent(validateButton.onClick));

            string givenCard = choiceDropdown.captionText.text;
            choiceDropdown.ClearOptions();
            choiceDropdown.gameObject.SetActive(false);
            validateButton.gameObject.SetActive(false);
            int indexCard = m_players[giverPlayerId].HasCard(givenCard);
            if (indexCard == -1)
            {
                Debug.LogError("Erreur : la carte choisie est invalide.");
                yield return -1;
            }
            else
            {
                m_players[playerId].AddCard(m_players[giverPlayerId].ListCard[indexCard]);

                if (m_players[giverPlayerId].ListCard[indexCard].cardType == CardType.Darkness
                    && m_players[giverPlayerId].ListCard[indexCard].isEquipement)
                    yield return StartCoroutine(DarknessCardPower(m_players[giverPlayerId].ListCard[m_players[giverPlayerId].ListCard.Count - 1] as DarknessCard));
                else if (m_players[giverPlayerId].ListCard[indexCard].cardType == CardType.Light
                    && m_players[giverPlayerId].ListCard[indexCard].isEquipement)
                    LightCardPower(m_players[giverPlayerId].ListCard[indexCard] as LightCard);
                    
                m_players[giverPlayerId].RemoveCard(indexCard);
                Debug.Log("La carte " + givenCard + " a été donnée au joueur " 
                    + m_players[playerId].Name + " par le joueur " + m_players[giverPlayerId].Name + " !");
            }
            yield return null;
        }
    }

    void PlayerCardPower(Player player)
    {
        switch(player.Character.characterType)
        {
            case CharacterType.Allie:
                // Il faut que le joueur se soit révélé et qu'il n'ait pas encore utilisé son pouvoir
                if(player.Revealed && !player.UsedPower)
                {
                    // Le joueur se soigne de toutes ses blessures
                    player.Healed(player.Wound);
                }
                break;
            /*
             *  PERSONNAGE EXTENSION
             */ 
            // case CharacterType.Bryan:
            //     // Révèle son identité à tous
            //     RevealCard();
            //     break;
            /*
             *  PERSONNAGE EXTENSION
             */ 
            // case CharacterType.David:
            //     // Il faut que le joueur se soit révélé et qu'il n'ait pas encore utilisé son pouvoir
            //     if(player.Revealed && !player.UsedPower)
            //     {
            //         // Liste des cartes équipements des défausses
            //         List<Card> cardList = new List<Card>();

            //         // On ajoute à cardList les cartes équipements de la défausse de cartes ténèbres
            //         foreach (DarknessCard c in gameBoard.Black)
            //             if(c.isEquipement)
			// 	            cardList.Add(c);

            //         // On ajoute à cardList les cartes équipements de la défausse de cartes lumières
            //         foreach (LightCard c in gameBoard.White)
            //             if(c.isEquipement)
			// 	            cardList.Add(c);

            //         // TODO choisir la carte
            //         int choosenCardIndex = UnityEngine.Random.Range(0, cardList.Count - 1);

            //         // Retire la carte de la défausse correspondante
            //         gameBoard.RemoveDiscard(cardList[choosenCardIndex], cardList[choosenCardIndex].cardType);

            //         // Ajoute la carte à la liste de cartes du joueur
            //         player.AddCard(cardList[choosenCardIndex]);

            //         // Le joueur ne peut plus utiliser son pouvoir
            //         player.UsedPower = true;
            //     }
            //     break;
            case CharacterType.Emi:
                // On cherche l'index de la carte Lieu dans la liste des lieux
                int indexEmi = gameBoard.GetIndexOfPosition(player.Position);

                // Le déplacement se fait vers le lieu adjacent
                if(indexEmi % 2 == 0)
                    indexEmi++;
                else
                    indexEmi--;

                // Nouvelle position du joueur
                Position newPosition = gameBoard.GetAreaAt(indexEmi).area;

                // On effectue le déplacement
                player.Position = newPosition;
                gameBoard.setPositionOfAt(player.Id, newPosition);
                break;
            case CharacterType.Metamorphe:
                break;
            case CharacterType.Bob:
                // Il faut que le joueur se soit révélé et qu'il n'ait pas encore utilisé son pouvoir
                if(player.Revealed)
                {
                    // Choix : voler une carte équipement ou infliger les blessures
                    choiceDropdown.gameObject.SetActive(false);
                    //validateChoosenPlayerButton.gameObject.SetActive(false);
                    string playerChoice = choiceDropdown.captionText.text;
                    choiceDropdown.ClearOptions();

                    //if(playerChoice == "steal")
                        // Vole une carte équipement du joueur correspondant
                        //StealEquipmentCard();
                        // TODO Inflige les dégats précédemment calculer dans AttackCorrespondingPlayer
                        // avec le joueur correspondant
                }
                break;
            case CharacterType.Franklin:
                if(player.Revealed && !player.UsedPower)
                {
                    int playerAttackedId = -1;
                    choiceDropdown.gameObject.SetActive(false);
                    //validateChoosenPlayerButton.gameObject.SetActive(false);
                    string playerAttacked = choiceDropdown.captionText.text;
                    choiceDropdown.ClearOptions();
                    for (int i = 0 ; i < m_nbPlayers ; i++)
                        if (m_players[i].Name.Equals(playerAttacked))
                            playerAttackedId = i;

                    Debug.Log("Vous choisissez d'attaquer le joueur " + m_players[playerAttackedId].Name + ".");
                    int lancer = UnityEngine.Random.Range(1, 6);
                    m_players[playerAttackedId].Wounded(lancer + m_players[m_playerTurn].BonusAttack - m_players[m_playerTurn].MalusAttack);
                    if(m_players[playerAttackedId].Character.characterType == CharacterType.LoupGarou)
                        PlayerCardPower(m_players[playerAttackedId]);
                    if(m_players[playerAttackedId].Character.characterType == CharacterType.Vampire)
                        PlayerCardPower(m_players[playerAttackedId]);  
                    CheckPlayerDeath(playerAttackedId);
                    // Utilisation unique du pouvoir
                    player.UsedPower = true;
                }
                break;
            case CharacterType.Georges:
                if(player.Revealed && !player.UsedPower)
                {
                    int playerAttackedId = -1;
                    choiceDropdown.gameObject.SetActive(false);
                    //validateChoosenPlayerButton.gameObject.SetActive(false);
                    string playerAttacked = choiceDropdown.captionText.text;
                    choiceDropdown.ClearOptions();
                    for (int i = 0 ; i < m_nbPlayers ; i++)
                        if (m_players[i].Name.Equals(playerAttacked))
                            playerAttackedId = i;

                    Debug.Log("Vous choisissez d'attaquer le joueur " + m_players[playerAttackedId].Name + ".");
                    int lancer = UnityEngine.Random.Range(1, 4);
                    m_players[playerAttackedId].Wounded(lancer + m_players[m_playerTurn].BonusAttack - m_players[m_playerTurn].MalusAttack);
                    if(m_players[playerAttackedId].Character.characterType == CharacterType.LoupGarou)
                        PlayerCardPower(m_players[playerAttackedId]);
                    if(m_players[playerAttackedId].Character.characterType == CharacterType.Vampire)
                        PlayerCardPower(m_players[playerAttackedId]);    
                    CheckPlayerDeath(playerAttackedId);
                    // Utilisation unique du pouvoir
                    player.UsedPower = true;
                }
                break;
            case CharacterType.LoupGarou:
                if(player.Revealed)
                {
                    //AttackCorrespondingPlayer();
                }
                break;
            case CharacterType.Vampire:
                if(player.Revealed)
                {
                    player.Healed(2);
                }
                break;
            case CharacterType.Charles:
                
                break;
            case CharacterType.Daniel:
                // Il faut que le joueur se soit révélé et qu'il n'ait pas encore utilisé son pouvoir
                if(!player.Revealed)
                {
                    // Révèle le personnage
                    RevealCard();
                }
                break;
        }
    }

    IEnumerator WaitUntilEvent(UnityEvent unityEvent) 
    {
        var trigger = false;
        Action action = () => trigger = true;
        unityEvent.AddListener(action.Invoke);
        yield return new WaitUntil(()=>trigger);
        unityEvent.RemoveListener(action.Invoke);
    }
}