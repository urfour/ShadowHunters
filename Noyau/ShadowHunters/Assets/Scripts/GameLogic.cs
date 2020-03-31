using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

/// <summary>
/// Classe représentant la logique du jeu, à savoir la gestion des règles et des interactions 
/// </summary>
public class GameLogic : MonoBehaviour
{
    /// <summary>
    /// Nombre de joueurs de la partie courante
    /// </summary>
    private int m_nbPlayers = 0;
    /// <summary>
    /// Propriété d'accès au nombre de joueurs
    /// </summary>
    public int NbPlayers
    {
        get => m_nbPlayers;
        private set => m_nbPlayers = value;
    }
    /// <summary>
    /// Nombre de Hunters dans la partie
    /// </summary>
    private int m_nbHunters = 0;
    /// <summary>
    /// Propriété d'accès au nombre de Hunters
    /// </summary>
    public int NbHunters
    {
        get => m_nbHunters;
        private set => m_nbHunters = value;
    }
    /// <summary>
    /// Nombre de Hunters morts
    /// </summary>
    private int m_nbHuntersDead = 0;
    /// <summary>
    /// Propriété d'accès au nombre de Hunters morts
    /// </summary>
    public int NbHuntersDead
    {
        get => m_nbHuntersDead;
        private set => m_nbHuntersDead = value;
    }
    /// <summary>
    /// Nombre de Shadows dans la partie
    /// </summary>
    private int m_nbShadows = 0;
    /// <summary>
    /// Propriété d'accès au nombre de Shadows
    /// </summary>
    public int NbShadows
    {
        get => m_nbShadows;
        private set => m_nbShadows = value;
    }
    /// <summary>
    /// Nombre de Shadow morts
    /// </summary>
    private int m_nbShadowsDeads = 0;
    /// <summary>
    /// Propriété d'accès au nombre de Shadow morts
    /// </summary>
    public int NbShadowsDeads
    {
        get => m_nbShadowsDeads;
        private set => m_nbShadowsDeads = value;
    }
    /// <summary>
    /// Nombre de Neutres dans la partie
    /// </summary>
    private int m_nbNeutrals = 0;
    /// <summary>
    /// Propriété d'accès au nombre de Neutres
    /// </summary>
    public int NbNeutrals
    {
        get => m_nbNeutrals;
        private set => m_nbNeutrals = value;
    }
    /// <summary>
    /// Nombre de Neutres morts
    /// </summary>
    private int m_nbNeutralsDeads = 0;
    /// <summary>
    /// Propriété d'accès au nombre de Neutres morts
    /// </summary>
    public int NeutralsDeads
    {
        get => m_nbNeutralsDeads;
        private set => m_nbNeutralsDeads = value;
    }
    /// <summary>
    /// Carte vision donné au métamorphe
    /// </summary>
    private VisionCard m_pickedVisionCard;
    /// <summary>
    /// Id du joueur dont c'est le tour
    /// </summary>
    private int m_playerTurn = -1;
    /// <summary>
    /// Id du joueur récemment attaqué
    /// </summary>    
    private int m_playerAttackedId = -1;
    /// <summary>
    /// Propriété d'accès à l'id du joueur attaqué
    /// </summary>
    public int PlayerAttacked
    {
        get => m_playerAttackedId;
        private set => m_playerAttackedId = value;
    }
     /// <summary>
    /// Dégats pris par le joueur attaqué par Bob
    /// </summary>    
    private int m_damageBob = -1;
    /// <summary>
    /// Propriété d'accès à l'id du joueur dont c'est le tour
    /// </summary>
    public int PlayerTurn
    {
        get => m_playerTurn;
        private set => m_playerTurn = value;
    }
    /// <summary>
    /// Booléen représentant l'état actuel du jeu (terminé ou non)
    /// </summary>
    private bool m_isGameOver = false;
    /// <summary>
    /// Propriété d'accès à l'état actuel du jeu
    /// </summary>
    public bool IsGameOver
    {
        get => m_isGameOver;
        private set => m_isGameOver = value;
    }
    /// <summary>
    /// Liste comportant les informations de tous les joueurs
    /// </summary>
    private List<Player> m_players;
    /// <summary>
    /// Plateau du jeu comportant les différentes cartes et leur position
    /// </summary>
    private GameBoard gameBoard;
    /// <summary>
    /// Propriété d'accès au plateau du jeu
    /// </summary>
    public GameBoard GameBoard { get; }

    // Cartes possibles des différents decks
    /// <summary>
    /// Liste des différentes cartes Vision
    /// </summary>
    public List<VisionCard> m_visionCards;
    /// <summary>
    /// Liste des différentes cartes Ténèbre
    /// </summary>
    public List<DarknessCard> m_darknessCards;
    /// <summary>
    /// Liste des différentes cartes Lumière
    /// </summary>
    public List<LightCard> m_lightCards;
    /// <summary>
    /// Liste des différentes cartes Lieu
    /// </summary>
    public List<LocationCard> m_locationCards;
    /// <summary>
    /// Liste des différents personnages Hunter
    /// </summary>
    public List<Character> m_hunterCharacters;
    /// <summary>
    /// Liste des différents personnages Shadow
    /// </summary>
    public List<Character> m_shadowCharacters;
    /// <summary>
    /// Liste des différents personnages Neutre
    /// </summary>
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
    public Button usePowerButton;
    public Button dontUsePowerButton;
    

    /// <summary>
    /// Fonction appelée dès l'instanciation du GameObject auquel est lié le script,
    /// permettant de préparer le jeu.
    /// </summary>
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
        usePowerButton.gameObject.SetActive(false);
        dontUsePowerButton.gameObject.SetActive(false);
        woundsForestToggle.gameObject.gameObject.SetActive(false);
        healForestToggle.gameObject.gameObject.SetActive(false);
        ChooseNextPlayer();
    }

    /// <summary>
    /// Ajout d'un joueur à la partie
    /// </summary>
    /// <param name="p">Joueur à ajouter dans la partie</param>
    void AddPlayer(Player p)
    {
        this.m_players.Add(p);
        m_nbPlayers++;
    }

    /// <summary>
    /// Suppression d'un joueur à la partie (lors d'une déconnexion par exemple)
    /// </summary>
    /// <param name="p">Joueur à supprimer de la partie</param>
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

    /// <summary>
    /// Préparation des decks de cartes et répartition des personnages aux joueurs
    /// </summary>
    /// <param name="nbPlayers">Nombre de joueurs de la partie</param>
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
        characters = PrepareCharacterCards();
        for (int i = 0; i < nbPlayers; i++)
        {
            m_players[i].SetCharacter(characters[0]);
            characters.RemoveAt(0);
        }

    }

    /// <summary>
    /// Redistribution des cartes lorsqu'une des pioches est vide
    /// </summary>
    /// <param name="oldDeck">Pile de défausse</param>
    /// <param name="newDeck">Pioche à refaire</param>
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

    /// <summary>
    /// Fonction permettant de préparer un deck de cartes en fonction du nombre de joueurs
    /// </summary>
    /// <param name="deck">Deck final construit</param>
    /// <param name="deckCharacter">Deck comportant les cartes personnages du même type</param>
    /// <param name="nb">Nombre de cartes à ajouter dans la pile finale</param>
    /// <param name="addBob">Booléen représentant la nécessité d'ajouter Bob ou non</param>
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

    /// <summary>
    /// Préparation des cartes Personnage en fonction du nombre de joueurs
    /// </summary>
    /// <returns>Liste des cartes Personnage préparée</returns>
    List<Character> PrepareCharacterCards()
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
        HuntersCards = m_hunterCharacters.PrepareDecks<Character>();
        ShadowsCards = m_shadowCharacters.PrepareDecks<Character>();
        NeutralsCards = m_shadowCharacters.PrepareDecks<Character>();

        AddCharacterCards(characterCards, HuntersCards, NbHunters, addBob);
        AddCharacterCards(characterCards, ShadowsCards, NbShadows, addBob);
        AddCharacterCards(characterCards, NeutralsCards, NbNeutrals, addBob);
        return characterCards;
    }

    /// <summary>
    /// Lancer de dé d'un personnage et déplacement en fonction du lancer de dés
    /// </summary>
    /// <returns>Itération terminée</returns>
    IEnumerator MoveCharacter()
    {
        int lancer1 = 0, lancer2 = 0, lancerTotal = 0;
        Position position = Position.None;

        /* POUVOIR EMI SANS BOUTON 
        if(m_players[m_playerTurn].Character.characterType == CharacterType.Emi && m_players[m_playerTurn].Revealed)
        {
            int usePowerEmi = UnityEngine.Random.Range(0, 1);

            if(usePowerEmi == 1)
                PlayerCardPower(m_players[m_playerTurn]);
        }
        */

        while (position == Position.None)
        {
            if (m_players[m_playerTurn].HasCompass)
            {
                int lancer01 = UnityEngine.Random.Range(1, 6);
                int lancer02 = UnityEngine.Random.Range(1, 4);
                int lancerTotal0 = lancer01 + lancer02;

                int lancer11 = UnityEngine.Random.Range(1, 6);
                int lancer12 = UnityEngine.Random.Range(1, 4);
                int lancerTotal1 = lancer11 + lancer12;

                choiceDropdown.gameObject.SetActive(true);
                validateButton.gameObject.SetActive(true);
                List<string> rolls = new List<string>();

                rolls.Add(lancerTotal0.ToString());
                rolls.Add(lancerTotal1.ToString());

                Debug.Log("Quel jet préférez-vous effectuer ?");
                choiceDropdown.AddOptions(rolls);
                yield return WaitUntilEvent(validateButton.onClick);

                string rollChoosen = choiceDropdown.captionText.text;
                choiceDropdown.ClearOptions();
                choiceDropdown.gameObject.SetActive(false);
                validateButton.gameObject.SetActive(false);

                lancerTotal = int.Parse(rollChoosen);

                if (lancerTotal == lancerTotal0)
                {
                    lancer1 = lancer01;
                    lancer2 = lancer02;
                }
                else if (lancerTotal == lancerTotal1)
                {
                    lancer1 = lancer11;
                    lancer2 = lancer12;
                }

            }
            else
            {
                lancer1 = UnityEngine.Random.Range(1, 6);
                lancer2 = UnityEngine.Random.Range(1, 4);
                lancerTotal = lancer1 + lancer2;
            }

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

    /// <summary>
    /// Activation de l'effet des cartes Lieu en fonction de la position du personnage courant
    /// </summary>
    /// <returns>Itération terminée</returns>
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
                {
                    if (!player.IsDead())
                    {
                        if(isWoundsForestEffect)
                            if(!player.HasBroche)
                                players.Add(player.Name);
                            else
                                continue;
                        else
                            players.Add(player.Name);
                    }
                }
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
    
    /// <summary>
    /// Fonction appelée par le bouton de pioche d'une carte Vision
    /// </summary>
    public void MoveVisionCardLocation()
    {
        StartCoroutine(VisionCardLocation());
    }

    /// <summary>
    /// Pioche d'une carte vision
    /// </summary>
    /// <returns>Itération terminée</returns>
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
    
    /// <summary>
    /// Fonction appelée par le bouton de pioche d'une carte Ténèbre
    /// </summary>
    public void MoveDarknessCardLocation()
    {
        StartCoroutine(DarknessCardLocation());
    }

    /// <summary>
    /// Pioche d'une carte Ténèbre
    /// </summary>
    /// <returns>Itération terminée</returns>
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
        if (!darknessCard.isEquipement)
            gameBoard.AddDiscard(darknessCard, CardType.Darkness);
        attackPlayer.gameObject.SetActive(true);
        if(m_players[m_playerTurn].HasSaber==true)
            endTurn.gameObject.SetActive(false);
        else
            endTurn.gameObject.SetActive(true);
    }

    /// <summary>
    /// Fonction appelée par le bouton de pioche d'une carte Lumière
    /// </summary>
    public void MoveLightCardLocation()
    {
        StartCoroutine(LightCardLocation());
    }

    /// <summary>
    /// Pioche d'une carte Lumière
    /// </summary>
    /// <returns>Itération terminée</returns>
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
        if (!lightCard.isEquipement)
            gameBoard.AddDiscard(lightCard, CardType.Light);
        attackPlayer.gameObject.SetActive(true);
        if(m_players[m_playerTurn].HasSaber==true)
            endTurn.gameObject.SetActive(false);
        else
            endTurn.gameObject.SetActive(true);
    }

    /// <summary>
    /// Activation de l'effet d'une carte Vision piochée
    /// </summary>
    /// <remarks>
    /// Le déroulement suit le procédé suivant : le jeu va afficher au joueur
    /// la liste des joueurs à qui il peut donner la carte vision (donc tous
    /// les joueurs vivants sauf lui-même) et va attendre que le joueur
    /// choisisse la personne à qui la donner. Dès lors, l'effet va s'activer
    /// (ou non) automatiquement en fonction du type de la carte Vision.
    /// Metamorphe, quant à lui, aura la possibilité de choisir
    /// s'il souhaite déclencher la carte ou non
    /// </remarks>
    /// <param name="pickedCard">Carte Vision piochée</param>
    /// <returns>Itération terminée</returns>
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

        if(m_players[playerId].Character.characterType == CharacterType.Metamorphe)
        {
            usePowerButton.gameObject.SetActive(true);
            dontUsePowerButton.gameObject.SetActive(true);
            m_pickedVisionCard = pickedCard;
        }
        // Cartes applicables en fonction des équipes ?
        else if ((team == CharacterTeam.Shadow && !pickedCard.visionEffect.effectOnShadow)
            || (team == CharacterTeam.Hunter && !pickedCard.visionEffect.effectOnHunter)
            || (team == CharacterTeam.Neutral && !pickedCard.visionEffect.effectOnNeutral))
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

    /// <summary>
    /// Activation de l'effet d'une carte Ténèbre piochée
    /// </summary>
    /// <param name="pickedCard">Carte Ténèbre piochée</param>
    /// <returns>Itération terminée</returns>
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
                        if(p.Position==area && !p.HasAmulet)
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

    /// <summary>
    /// Choix du joueur à qui infliger des Blessures
    /// </summary>
    /// <param name="isPuppet">Booléen représentant si l'effet est issu de la
    /// carte Poupée sanguinaire ou non</param>
    /// <param name="nbWoundsTaken">Nombre de Blessures à infliger</param>
    /// <param name="nbWoundsSelfHealed">Nombre de Blessures éventuellement soignées</param>
    /// <returns>Itération terminée</returns>
    IEnumerator TakingWoundsEffect(bool isPuppet, int nbWoundsTaken, int nbWoundsSelfHealed)
    {
        choiceDropdown.gameObject.SetActive(true);
        validateButton.gameObject.SetActive(true);
        List<string> players = new List<string>();
        foreach (Player player in m_players)
        {
            if (!player.IsDead() && player.Id != m_playerTurn)
            {
                if(isPuppet)
                    players.Add(player.Name);
                else
                {
                    if(!player.HasAmulet)
                        players.Add(player.Name);
                }
            }
        }
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

    /// <summary>
    /// Activation de l'effet d'une carte Lumière piochée
    /// </summary>
    /// <param name="pickedCard">Carte Lumière piochée</param>
    /// <returns>Itération terminée</returns>
    IEnumerator LightCardPower(LightCard pickedCard)
    {
        CharacterTeam team = m_players[m_playerTurn].Team;
        string character = m_players[m_playerTurn].Character.characterName;
        bool revealed = m_players[m_playerTurn].Revealed;
        Debug.Log(pickedCard.lightEffect);
        switch (pickedCard.lightEffect)
        {
			case LightEffect.Amulette:
                m_players[m_playerTurn].HasAmulet=true;
                break;
				
			case LightEffect.AngeGardien:

                m_players[m_playerTurn].HasGuardian = true;
                break;
				
			case LightEffect.Supreme:
                Debug.Log("Voulez-vous vous révéler ? Vous avez 6 secondes, sinon la carte se défausse.");
                yield return new WaitForSeconds(6f);
                if (revealed && m_players[m_playerTurn].Team== CharacterTeam.Hunter)
                {
                    m_players[m_playerTurn].Healed(m_players[m_playerTurn].Wound);
                    Debug.Log("Le joueur "+ m_players[m_playerTurn].Name + " se soigne complètement");
                }
                else
                {
                    Debug.Log("Rien ne se passe.");
                }
                break;

			case LightEffect.Chocolat:
                Debug.Log("Voulez-vous vous révéler ? Vous avez 6 secondes, sinon la carte se défausse.");
                yield return new WaitForSeconds(6f);
                if (revealed && (character == "Allie" || character == "Emi" || character == "Metamorphe"))
                {
                    m_players[m_playerTurn].Healed(m_players[m_playerTurn].Wound);
                    Debug.Log("Le joueur "+ m_players[m_playerTurn].Name + " se soigne complètement");
                }
                else
                {
                    Debug.Log("Rien ne se passe.");
                }
                break;
			
			case LightEffect.Benediction:

                choiceDropdown.gameObject.SetActive(true);
                validateButton.gameObject.SetActive(true);
                List<string> players = new List<string>();
                foreach (Player player in m_players)
                {
                    if (!player.IsDead() && player.Id != m_playerTurn)
                    {
                        players.Add(player.Name);
                    }
                }
                Debug.Log("Qui souhaitez-vous soigner ?");
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

                Debug.Log("Vous choisissez de soigner le joueur " + m_players[playerChoosenId].Name + ".");
                
                int heal = UnityEngine.Random.Range(1, 6);
                
                m_players[playerChoosenId].Healed(heal);
                
				break;
			
			case LightEffect.Boussole:
				m_players[m_playerTurn].HasCompass=true;
				break;
			
			case LightEffect.Broche:
                m_players[m_playerTurn].HasBroche=true;
                break;
				
			case LightEffect.Crucifix:
                m_players[m_playerTurn].HasCrucifix=true;
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
				m_players[m_playerTurn].HasSpear=true;
				if(team== CharacterTeam.Hunter && revealed)
                {
                    m_players[m_playerTurn].BonusAttack+=2;
                }
				break;
				
			case LightEffect.Miroir:
				
				if (team == CharacterTeam.Shadow && character != "Metamorphe")
				{
					m_players[m_playerTurn].Revealed = true;
					Debug.Log("Vous révélez votre rôle à tous, vous êtes : " + character);
				}
				break;
				
			case LightEffect.PremiersSecours:

                choiceDropdown.gameObject.SetActive(true);
                validateButton.gameObject.SetActive(true);
                List<string> players2 = new List<string>();
                foreach (Player player in m_players)
                {
                    if (!player.IsDead())
                    {
                        players2.Add(player.Name);
                    }
                }
                Debug.Log("Qui souhaitez-vous placer à exactement 7 Blessures ?");
                choiceDropdown.AddOptions(players2);
                yield return WaitUntilEvent(validateButton.onClick);

                string playerChoosen2 = choiceDropdown.captionText.text;
                choiceDropdown.ClearOptions();
                choiceDropdown.gameObject.SetActive(false);
                validateButton.gameObject.SetActive(false);
                int playerChoosenId2 = -1;
                foreach (Player player in m_players)
                    if (player.Name.Equals(playerChoosen2))
                        playerChoosenId2 = player.Id;

                m_players[playerChoosenId2].SetWound(7);
				break;
				
			case LightEffect.Savoir:

                m_players[m_playerTurn].HasAncestral = true;
                break;

            case LightEffect.Toge:

				m_players[m_playerTurn].HasToge=true;
				m_players[m_playerTurn].MalusAttack++;
				m_players[m_playerTurn].ReductionWounds = 1;
				break;
		}
    }

    /// <summary>
    /// Vérifie si un personnage est un personnage avec 11 PDV ou moins
    /// </summary>
    /// <param name="characterName">Nom du personnage</param>
    /// <returns>Booléen représentant le type du personnage</returns>
    bool CheckLowHPCharacters(string characterName)
    {
        return characterName.StartsWith("A") || characterName.StartsWith("B")
            || characterName.StartsWith("C") || characterName.StartsWith("E")
            || characterName.StartsWith("M");
    }

    /// <summary>
    /// Vérifie si un personnage est un personnage avec 12 PDV ou plus
    /// </summary>
    /// <param name="characterName">Nom du personnage</param>
    /// <returns>Booléen représentant le type du personnage</returns>
    bool CheckHighHPCharacters(string characterName)
    {
        return characterName.StartsWith("D") || characterName.StartsWith("F")
            || characterName.StartsWith("G") || characterName.StartsWith("L")
            || characterName.StartsWith("V");
    }

    /// <summary>
    /// Fonction permettant de révéler son identité
    /// </summary>
    public void RevealCard()
    {
        m_players[m_playerTurn].Revealed=true;
        Debug.Log("Le joueur "+ m_players[m_playerTurn].Name + " s'est révélé, il s'agissait de : " 
            + m_players[m_playerTurn].Character.characterName + " ! Il est dans l'équipe des " 
            + m_players[m_playerTurn].Character.team + ".");
        revealCardButton.gameObject.SetActive(false);

        // Si le joueur est Allie, il peut utiliser son pouvoir à tout moment
        // Si le joueur est Emi, Franklin ou Georges et qu'il est au début de son tour, il peut utiliser son pouvoir
        if(m_players[m_playerTurn].Character.characterType == CharacterType.Allie 
            || (rollDicesButton.gameObject.activeInHierarchy 
                && (m_players[m_playerTurn].Character.characterType == CharacterType.Emi
                    || m_players[m_playerTurn].Character.characterType == CharacterType.Franklin 
                    || m_players[m_playerTurn].Character.characterType == CharacterType.Georges)))
        {
            usePowerButton.gameObject.SetActive(true);
        }
    }

    /// <summary>
    /// Fonction appelée par le bouton permettant d'attaquer
    /// </summary>
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

    /// <summary>
    /// Permet à un joueur de finir son tour, pour pouvoir choisir le prochain joueur
    /// </summary>
    public void ChooseNextPlayer()
    {
        visionCardsButton.gameObject.SetActive(false);
        darknessCardsButton.gameObject.SetActive(false);
        lightCardsButton.gameObject.SetActive(false);
        attackPlayer.gameObject.SetActive(false);
        endTurn.gameObject.SetActive(false);

        if (m_playerTurn == -1)
            m_playerTurn = UnityEngine.Random.Range(0, m_nbPlayers - 1);
        else if (m_players[m_playerTurn].HasAncestral) // si le joueur a utilisé le savoir ancestral, le joueur suivant reste lui
        {
            Debug.Log("Le joueur " + m_players[m_playerTurn].Name + " rejoue grâce au Savoir Ancestral !");
            m_players[m_playerTurn].HasAncestral = false;
        }
        else
            m_playerTurn = (m_playerTurn + 1) % m_nbPlayers;
        Debug.Log("C'est au joueur " + m_players[m_playerTurn].Name + " de jouer.");

        if (m_players[m_playerTurn].HasGuardian)
        {
            m_players[m_playerTurn].HasGuardian = false;
            Debug.Log("Le joueur " + m_players[m_playerTurn].Name + " n'est plus affecté par l'Ange Gardien !");
        }

        rollDicesButton.gameObject.SetActive(true);
        if(m_players[m_playerTurn].Revealed==false)
        {
            revealCardButton.gameObject.SetActive(true);
        }
        else if(m_players[m_playerTurn].Character.characterType == CharacterType.Emi
                || m_players[m_playerTurn].Character.characterType == CharacterType.Franklin
                || m_players[m_playerTurn].Character.characterType == CharacterType.Georges)
        {
            usePowerButton.gameObject.SetActive(true);
        }
    }

    /// <summary>
    /// Fonction permetant de lancer le tour d'un joueur
    /// </summary>
    public void RollTheDices()
    {
        if(m_players[m_playerTurn].Character.characterType == CharacterType.Emi
            || m_players[m_playerTurn].Character.characterType == CharacterType.Franklin
            || m_players[m_playerTurn].Character.characterType == CharacterType.Georges)
        {
            usePowerButton.gameObject.SetActive(false);
        }
        StartCoroutine(PlayTurn());
    }

    /// <summary>
    /// Exécution du tour d'un joueur
    /// </summary>
    /// <returns>Itération terminée</returns>
    IEnumerator PlayTurn()
    {
        rollDicesButton.gameObject.SetActive(false);

        /* POUVOIR FRANKLIN ET GEORGES SANS BOUTON

        if(m_players[m_playerTurn].Character.characterType == CharacterType.Franklin)
            PlayerCardPower(m_players[m_playerTurn]);    
        if(m_players[m_playerTurn].Character.characterType == CharacterType.Georges)
            PlayerCardPower(m_players[m_playerTurn]);

        */

        yield return StartCoroutine(MoveCharacter());
        yield return StartCoroutine(ActivateLocationPower());
    }

    /// <summary>
    /// Test de victoire d'un joueur
    /// </summary>
    /// <param name="playerId">Id du joueur à tester</param>
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
                if (m_players[playerId].HasCrucifix)
                    nbCardsOwned++;
                if (m_players[playerId].HasAmulet)
                    nbCardsOwned++;
                if (m_players[playerId].HasSpear)
                    nbCardsOwned++;
                if (m_players[playerId].HasToge)
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

    /// <summary>
    /// Choix de la carte Lieu sur laquelle se rendre après avoir fait 7
    /// à son lancer de dés
    /// </summary>
    /// <returns>Itération terminée</returns>
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

    /// <summary>
    /// Récupération des joueurs se trouvant dans le même secteur qu'un
    /// autre joueur
    /// </summary>
    /// <param name="playerId">Id du joueur</param>
    /// <param name="hasRevolver">Booléen représentant la possesion du 
    /// Revolver</param>
    /// <returns>Liste des joueurs se trouvant dans le même secteur</returns>
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

    /// <summary>
    /// Fonction du choix d'un joueur à attaquer
    /// </summary>
    /// <param name="playerAttackingId">Id du joueur attaquant</param>
    /// <returns>Iteration terminée</returns>
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
                // Si le joueur a la gatling, il attaque tous les joueurs qui ne sont pas dans sa zone
                if (m_players[playerAttackingId].HasGatling)
                {
                    foreach (Player player in players)
                    {
                        if((m_players[playerAttackingId].Character.characterType == CharacterType.Bob) && lancerTotal >= 2 )
                            PlayerCardPower(m_players[playerAttackingId]);
                        else
                        {
                            m_players[player.Id].Wounded(lancerTotal + m_players[playerAttackingId].BonusAttack - m_players[playerAttackingId].MalusAttack);
                            
                            // On vérifie si le joueur attaqué est mort
                            CheckPlayerDeath(player.Id);
                            
                            // Le Loup-garou peut contre attaquer
                            if(m_players[playerAttackedId].Character.characterType == CharacterType.LoupGarou
                                && m_players[playerAttackingId].Revealed)
                            {
                                usePowerButton.gameObject.SetActive(true);
                            }
                            
                            // Le Vampire se soigne 2 blessures s'il est révélé et s'il a infligé des dégats
                            if(m_players[playerAttackingId].Character.characterType == CharacterType.Vampire
                                && m_players[playerAttackingId].Revealed
                                && lancerTotal + m_players[playerAttackingId].BonusAttack - m_players[playerAttackingId].MalusAttack > 0)
                                PlayerCardPower(m_players[playerAttackingId]); 
                        }    
                    }
                }
                else
                {
                    for (int i = 0 ; i < m_nbPlayers ; i++)
                        if (m_players[i].Name.Equals(playerAttacked))
                            playerAttackedId = i;
                    
                    m_playerAttackedId = playerAttackedId;

                    Debug.Log("Vous choisissez d'attaquer le joueur " + m_players[playerAttackedId].Name + ".");

                    // Si Bob est révélé et inflige 2 dégats ou plus, il peut voler une arme 
                    if(m_players[playerAttackingId].Character.characterType == CharacterType.Bob
                        && m_players[playerAttackingId].Revealed
                        && lancerTotal + m_players[playerAttackingId].BonusAttack - m_players[playerAttackingId].MalusAttack >= 2)
                    {
                        m_damageBob = lancerTotal + m_players[playerAttackingId].BonusAttack - m_players[playerAttackingId].MalusAttack;
                        usePowerButton.gameObject.SetActive(true);
                        dontUsePowerButton.gameObject.SetActive(true);
                    }
                    else
                    {
                        // Le joueur attaqué se prend des dégats
                        m_players[playerAttackedId].Wounded(lancerTotal + m_players[playerAttackingId].BonusAttack - m_players[playerAttackingId].MalusAttack);
                        
                        // Le Vampire se soigne 2 blessures s'il est révélé et s'il a infligé des dégats
                        if(m_players[playerAttackingId].Character.characterType == CharacterType.Vampire
                            && m_players[playerAttackingId].Revealed
                            && lancerTotal + m_players[playerAttackingId].BonusAttack - m_players[playerAttackingId].MalusAttack > 0)
                            PlayerCardPower(m_players[playerAttackingId]);

                        // On vérifie si le joueur attaqué est mort
                        CheckPlayerDeath(playerAttackedId);

                        // Charles peut attaquer de nouveau
                        // Le Loup-garou peut contre attaquer
                        if((m_players[playerAttackingId].Character.characterType == CharacterType.Charles
                            && m_players[playerAttackingId].Revealed)
                            || (m_players[playerAttackedId].Character.characterType == CharacterType.LoupGarou
                            && m_players[playerAttackedId].Revealed))
                        {
                            usePowerButton.gameObject.SetActive(true);
                        }
                    }
                }
            }
        }

        endTurn.gameObject.SetActive(true);
    }

    /// <summary>
    /// Fonction du choix d'un joueur à attaquer
    /// </summary>
    /// <param name="playerAttackingId">Id du joueur attaquant</param>
    /// <param name="targetId">Id du joueur attaqué</param>
    /// <returns>Iteration terminée</returns>
    IEnumerator AttackCorrespondingPlayer(int playerAttackingId, int targetId)
    {
        List<Player> players = GetPlayersSameSector(playerAttackingId, m_players[playerAttackingId].HasRevolver);
        if (players.Count == 0)
        {
            Debug.Log("Vous ne pouvez attaquer aucun joueur.");
        }
        else
        {
            m_playerAttackedId = targetId;

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
                        if((m_players[playerAttackingId].Character.characterType == CharacterType.Bob) && lancerTotal >= 2 )
                            PlayerCardPower(m_players[playerAttackingId]);
                        else
                        {
                            m_players[targetId].Wounded(lancerTotal + m_players[playerAttackingId].BonusAttack - m_players[playerAttackingId].MalusAttack);
                            
                            CheckPlayerDeath(targetId);
                            
                            // Le Loup-garou peut contre attaquer
                            if(m_players[targetId].Character.characterType == CharacterType.LoupGarou
                                && m_players[targetId].Revealed)
                            {
                                usePowerButton.gameObject.SetActive(true);
                            }
                            
                            // Le Vampire se soigne 2 blessures s'il est révélé et s'il a infligé des dégats
                            if(m_players[playerAttackingId].Character.characterType == CharacterType.Vampire
                                && m_players[playerAttackingId].Revealed
                                && lancerTotal + m_players[playerAttackingId].BonusAttack - m_players[playerAttackingId].MalusAttack > 0)
                                PlayerCardPower(m_players[playerAttackingId]);
                        }    
                }
                else
                {

                    Debug.Log("Vous choisissez d'attaquer le joueur " + m_players[targetId].Name + ".");

                    if(m_players[playerAttackingId].Character.characterType == CharacterType.Bob && lancerTotal >= 2 )
                        PlayerCardPower(m_players[playerAttackingId]);
                    else
                    {
                        m_players[targetId].Wounded(lancerTotal + m_players[playerAttackingId].BonusAttack - m_players[playerAttackingId].MalusAttack);
                        
                        // Le Loup-garou peut contre attaquer
                        if(m_players[targetId].Character.characterType == CharacterType.LoupGarou
                            && m_players[targetId].Revealed)
                        {
                            usePowerButton.gameObject.SetActive(true);
                        }

                        // Le Vampire se soigne 2 blessures s'il est révélé et s'il a infligé des dégats
                        if(m_players[playerAttackingId].Character.characterType == CharacterType.Vampire
                            && m_players[playerAttackingId].Revealed
                            && lancerTotal + m_players[playerAttackingId].BonusAttack - m_players[playerAttackingId].MalusAttack > 0)
                            PlayerCardPower(m_players[playerAttackingId]);

                        CheckPlayerDeath(targetId);
                    }    
                }
            }
        }
        endTurn.gameObject.SetActive(true);
        yield return null;
    }

    /// <summary>
    /// Test de mort d'un joueur après avoir subi des Blessures
    /// </summary>
    /// <param name="playerId">Id du joueur à tester</param>
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

    /// <summary>
    /// Test de l'assassinat d'un joueur par un autre
    /// </summary>
    /// <param name="attackerId">Id du joueur attaquant</param>
    /// <param name="playerId">Id du joueur attaqué</param>
    /// <returns>Itération terminée</returns>
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
            {
                if (m_players[attackerId].HasCrucifix) // vole tous les équipements
                {
                    for (int i = 0; i < m_players[playerId].ListCard.Count; i++)
                    {
                        m_players[attackerId].AddCard(m_players[playerId].ListCard[i]);
                        if (m_players[playerId].ListCard[i].cardType == CardType.Darkness
                            && m_players[playerId].ListCard[i].isEquipement)
                            yield return StartCoroutine(DarknessCardPower(m_players[attackerId].ListCard[m_players[attackerId].ListCard.Count - 1] as DarknessCard));
                        else if (m_players[playerId].ListCard[i].cardType == CardType.Light
                            && m_players[playerId].ListCard[i].isEquipement)
                            yield return StartCoroutine(LightCardPower(m_players[attackerId].ListCard[m_players[attackerId].ListCard.Count - 1] as LightCard));

                        m_players[playerId].RemoveCard(i);
                        Debug.Log("La carte " + m_players[attackerId].ListCard[m_players[attackerId].ListCard.Count - 1] + " a été volée au joueur "
                            + m_players[playerId].Name + " par le joueur " + m_players[attackerId].Name + " !");
                    }
                }
                else
                    yield return StartCoroutine(StealEquipmentCard(attackerId, playerId));
            }
        }
    }

    /// <summary>
    /// Choix du joueur à qui voler une carte équipement
    /// </summary>
    /// <param name="thiefId">Id du joueur voleur</param>
    /// <returns>Itération terminée</returns>
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

    /// <summary>
    /// Vol d'une carte équipement à un joueur précis
    /// </summary>
    /// <param name="thiefId">Id du joueur voleur</param>
    /// <param name="playerId">Id du joueur à qui voler une carte</param>
    /// <returns></returns>
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

    /// <summary>
    /// Choix d'une carte équipement à donner et du joueur à qui la donner
    /// </summary>
    /// <param name="giverPlayerId">Id du joueur donneur</param>
    /// <returns>Itération terminée</returns>
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
            choiceDropdown.gameObject.SetActive(true);
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

    /// <summary>
    /// Utilisation du pouvoir d'un personnage
    /// </summary>
    public void UsePower()
    {
        usePowerButton.gameObject.SetActive(false);
        dontUsePowerButton.gameObject.SetActive(false);

        Debug.Log("Player : " + m_players[m_playerTurn].Id + " effectue son pouvoir");

        // A changer avec le personnage du joueur quand le réseau sera fait
        PlayerCardPower(m_players[m_playerTurn]);
    }

    /// <summary>
    /// Non utilisation du pouvoir d'un personnage
    /// </summary>
    public void DontUsePower()
    {
        usePowerButton.gameObject.SetActive(false);
        dontUsePowerButton.gameObject.SetActive(false);

        Player p = m_players[m_playerTurn];

        Debug.Log("Player : " + p.Id + " n'effectue pas son pouvoir");
        switch(p.Character.characterType)
        {
            case CharacterType.Metamorphe:
                if (!m_pickedVisionCard.visionEffect.effectOnShadow)
                {
                    if (m_pickedVisionCard.visionEffect.effectSupremeVision)
                        //TODO montrer la carte personnage
                        Debug.Log("C'est une carte Vision Suprême !");
                    else
                        Debug.Log("Rien ne se passe.");
                }
                else
                {
                    // Cas des cartes applicables en fonction des points de vie
                    if (m_pickedVisionCard.visionEffect.effectOnLowHP && CheckLowHPCharacters(p.Character.characterName))
                    {
                        p.Wounded(1);
                        CheckPlayerDeath(p.Id);
                    }
                    else if (m_pickedVisionCard.visionEffect.effectOnHighHP && CheckHighHPCharacters(p.Character.characterName))
                    {
                        p.Wounded(2);
                        CheckPlayerDeath(p.Id);
                    }

                    // Cas des cartes infligeant des Blessures
                    else if (m_pickedVisionCard.visionEffect.effectTakeWounds)
                    {
                        p.Wounded(m_pickedVisionCard.visionEffect.nbWounds);
                        CheckPlayerDeath(p.Id);
                    }
                    // Cas des cartes soignant des Blessures
                    else if (m_pickedVisionCard.visionEffect.effectHealingOneWound)
                    {
                        if (p.Wound == 0)
                        {
                            p.Wounded(1);
                            CheckPlayerDeath(p.Id);
                        }
                        else
                        {
                            p.Healed(1);
                            CheckPlayerDeath(p.Id);
                        }
                    }
                    // Cas des cartes volant une carte équipement ou infligeant des Blessures
                    else if (m_pickedVisionCard.visionEffect.effectGivingEquipementCard)
                    {
                        if (p.ListCard.Count == 0)
                        {
                            Debug.Log("Vous ne possédez pas de carte équipement.");
                            p.Wounded(1);
                        }
                        else
                        // TODO don d'une carte équipement
                        {
                            GiveEquipmentCard(p.Id);
                        }
                    }
                    else
                        Debug.Log("Rien ne se passe.");
                }
                break;
            case CharacterType.Bob:
                m_players[m_playerAttackedId].Wounded(m_damageBob);

                CheckPlayerDeath(m_playerAttackedId);

                if(m_players[m_playerAttackedId].Character.characterType == CharacterType.LoupGarou
                    && m_players[m_playerAttackedId].Revealed)
                {
                    usePowerButton.gameObject.SetActive(true);
                }
                break;
        }
    }

    /// <summary>
    /// Activation de l'effet d'une carte Personnage
    /// </summary>
    /// <param name="player">Joueur utilisant l'effet de sa carte 
    /// Personnage</param>
    IEnumerator PlayerCardPower(Player player)
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

                if(indexEmi == -1)
                {
                    StartCoroutine(MoveCharacter());
                    StartCoroutine(ActivateLocationPower());
                }
                else
                {
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

                    // Activation du pouvoir du lieu
                    StartCoroutine(ActivateLocationPower());
                }
                
                break;
            case CharacterType.Metamorphe:
                if (m_pickedVisionCard.visionEffect.effectOnShadow)
                {
                    if (m_pickedVisionCard.visionEffect.effectSupremeVision)
                        //TODO montrer la carte personnage
                        Debug.Log("C'est une carte Vision Suprême !");
                    else
                        Debug.Log("Rien ne se passe.");
                }
                else
                {
                    // Cas des cartes applicables en fonction des points de vie
                    if (m_pickedVisionCard.visionEffect.effectOnLowHP && CheckLowHPCharacters(player.Character.characterName))
                    {
                        player.Wounded(1);
                        CheckPlayerDeath(player.Id);
                    }
                    else if (m_pickedVisionCard.visionEffect.effectOnHighHP && CheckHighHPCharacters(player.Character.characterName))
                    {
                        player.Wounded(2);
                        CheckPlayerDeath(player.Id);
                    }

                    // Cas des cartes infligeant des Blessures
                    else if (m_pickedVisionCard.visionEffect.effectTakeWounds)
                    {
                        player.Wounded(m_pickedVisionCard.visionEffect.nbWounds);
                        CheckPlayerDeath(player.Id);
                    }
                    // Cas des cartes soignant des Blessures
                    else if (m_pickedVisionCard.visionEffect.effectHealingOneWound)
                    {
                        if (player.Wound == 0)
                        {
                            player.Wounded(1);
                            CheckPlayerDeath(player.Id);
                        }
                        else
                        {
                            player.Healed(1);
                            CheckPlayerDeath(player.Id);
                        }
                    }
                    // Cas des cartes volant une carte équipement ou infligeant des Blessures
                    else if (m_pickedVisionCard.visionEffect.effectGivingEquipementCard)
                    {
                        if (player.ListCard.Count == 0)
                        {
                            Debug.Log("Vous ne possédez pas de carte équipement.");
                            player.Wounded(1);
                        }
                        else
                        // TODO don d'une carte équipement
                        {
                            GiveEquipmentCard(player.Id);
                        }
                    }
                    else
                        Debug.Log("Rien ne se passe.");
                }
                break;
            case CharacterType.Bob:
                // Il faut que le joueur se soit révélé et qu'il n'ait pas encore utilisé son pouvoir
                if(player.Revealed)
                {
                    // Choix : voler une carte équipement ou infliger les blessures
                    choiceDropdown.gameObject.SetActive(true);
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
                    choiceDropdown.gameObject.SetActive(true);
                    List<Player> players = GetPlayersSameSector(player.Id, m_players[player.Id].HasRevolver);
                    if (players.Count == 0)
                    {
                        Debug.Log("Vous ne pouvez attaquer aucun joueur.");
                        choiceDropdown.gameObject.SetActive(false);
                    }
                    else
                    {
                        List<string> playerNames = new List<string>();
                        foreach (Player playerSector in players)
                            playerNames.Add(playerSector.Name);

                        choiceDropdown.AddOptions(playerNames);
                        validateButton.gameObject.SetActive(true);
                        yield return WaitUntilEvent(validateButton.onClick);
                        int playerAttackedId = -1;
                        choiceDropdown.gameObject.SetActive(false);
                        validateButton.gameObject.SetActive(false);
                        string playerAttacked = choiceDropdown.captionText.text;
                        choiceDropdown.ClearOptions();

                        int lancer = UnityEngine.Random.Range(1, 6);
                        int lancerTotal = lancer;
                        if (lancerTotal == 0)
                        {
                            Debug.Log("Le lancer vaut 0, vous n'attaquez pas.");
                        }
                        else
                        {
                            // Si le joueur a la gatling, il attaque tous les joueurs qui ne sont pas dans sa zone
                            if (m_players[player.Id].HasGatling)
                            {
                                foreach (Player playerSector in players)
                                {
                                    m_players[playerSector.Id].Wounded(lancerTotal + m_players[player.Id].BonusAttack - m_players[player.Id].MalusAttack);
                                    
                                    // On vérifie si le joueur attaqué est mort
                                    CheckPlayerDeath(playerSector.Id);
                                    
                                    // Le Loup-garou peut contre attaquer
                                    if(m_players[playerAttackedId].Character.characterType == CharacterType.LoupGarou
                                        && m_players[player.Id].Revealed)
                                    {
                                        usePowerButton.gameObject.SetActive(true);
                                    }
                                }
                            }
                            else
                            {
                                for (int i = 0 ; i < m_nbPlayers ; i++)
                                    if (m_players[i].Name.Equals(playerAttacked))
                                        playerAttackedId = i;
                                
                                m_playerAttackedId = playerAttackedId;

                                Debug.Log("Vous choisissez d'attaquer le joueur " + m_players[playerAttackedId].Name + ".");

                                // Le joueur attaqué se prend des dégats
                                m_players[playerAttackedId].Wounded(lancerTotal + m_players[player.Id].BonusAttack - m_players[player.Id].MalusAttack);

                                // On vérifie si le joueur attaqué est mort
                                CheckPlayerDeath(playerAttackedId);

                                // Charles peut attaquer de nouveau
                                // Le Loup-garou peut contre attaquer
                                if((m_players[player.Id].Character.characterType == CharacterType.Charles
                                    && m_players[player.Id].Revealed)
                                    || (m_players[playerAttackedId].Character.characterType == CharacterType.LoupGarou
                                    && m_players[playerAttackedId].Revealed))
                                {
                                    usePowerButton.gameObject.SetActive(true);
                                }
                            }
                        }
                    }
                    player.UsedPower = true;
                }
                break;
            case CharacterType.Georges:
                if(player.Revealed && !player.UsedPower)
                {
                    choiceDropdown.gameObject.SetActive(true);
                    List<Player> players = GetPlayersSameSector(player.Id, m_players[player.Id].HasRevolver);
                    if (players.Count == 0)
                    {
                        Debug.Log("Vous ne pouvez attaquer aucun joueur.");
                        choiceDropdown.gameObject.SetActive(false);
                    }
                    else
                    {
                        List<string> playerNames = new List<string>();
                        foreach (Player playerSector in players)
                            playerNames.Add(playerSector.Name);

                        choiceDropdown.AddOptions(playerNames);
                        validateButton.gameObject.SetActive(true);
                        yield return WaitUntilEvent(validateButton.onClick); 
                        int playerAttackedId = -1;
                        choiceDropdown.gameObject.SetActive(false);
                        validateButton.gameObject.SetActive(false);
                        string playerAttacked = choiceDropdown.captionText.text;
                        choiceDropdown.ClearOptions();

                        int lancer = UnityEngine.Random.Range(1, 4);
                        int lancerTotal = lancer;
                        if (lancerTotal == 0)
                        {
                            Debug.Log("Le lancer vaut 0, vous n'attaquez pas.");
                        }
                        else
                        {
                            // Si le joueur a la gatling, il attaque tous les joueurs qui ne sont pas dans sa zone
                            if (m_players[player.Id].HasGatling)
                            {
                                foreach (Player playerSector in players)
                                {
                                    m_players[playerSector.Id].Wounded(lancerTotal + m_players[player.Id].BonusAttack - m_players[player.Id].MalusAttack);
                                    
                                    // On vérifie si le joueur attaqué est mort
                                    CheckPlayerDeath(playerSector.Id);
                                    
                                    // Le Loup-garou peut contre attaquer
                                    if(m_players[playerAttackedId].Character.characterType == CharacterType.LoupGarou
                                        && m_players[player.Id].Revealed)
                                    {
                                        usePowerButton.gameObject.SetActive(true);
                                    }
                                }
                            }
                            else
                            {
                                for (int i = 0 ; i < m_nbPlayers ; i++)
                                    if (m_players[i].Name.Equals(playerAttacked))
                                        playerAttackedId = i;
                                
                                m_playerAttackedId = playerAttackedId;

                                Debug.Log("Vous choisissez d'attaquer le joueur " + m_players[playerAttackedId].Name + ".");

                                // Le joueur attaqué se prend des dégats
                                m_players[playerAttackedId].Wounded(lancerTotal + m_players[player.Id].BonusAttack - m_players[player.Id].MalusAttack);

                                // On vérifie si le joueur attaqué est mort
                                CheckPlayerDeath(playerAttackedId);

                                // Charles peut attaquer de nouveau
                                // Le Loup-garou peut contre attaquer
                                if((m_players[player.Id].Character.characterType == CharacterType.Charles
                                    && m_players[player.Id].Revealed)
                                    || (m_players[playerAttackedId].Character.characterType == CharacterType.LoupGarou
                                    && m_players[playerAttackedId].Revealed))
                                {
                                    usePowerButton.gameObject.SetActive(true);
                                }
                            }
                        }
                    }
                    player.UsedPower = true;
                }
                break;
            case CharacterType.LoupGarou:
                if(player.Revealed)
                {
                    StartCoroutine(AttackCorrespondingPlayer(player.Id, m_playerTurn));
                }
                break;
            case CharacterType.Vampire:
                if(player.Revealed)
                {
                    player.Healed(2);
                }
                break;
            case CharacterType.Charles:
                if(player.Revealed)
                {
                    player.Wounded(2);
                    StartCoroutine(AttackCorrespondingPlayer(player.Id, m_playerAttackedId));
                }
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

    /// <summary>
    /// Fonction d'attente de l'activation d'un événement Unity
    /// </summary>
    /// <param name="unityEvent">Événement Unity à attendre</param>
    /// <returns>Itération terminée</returns>
    IEnumerator WaitUntilEvent(UnityEvent unityEvent) 
    {
        var trigger = false;
        Action action = () => trigger = true;
        unityEvent.AddListener(action.Invoke);
        yield return new WaitUntil(()=>trigger);
        unityEvent.RemoveListener(action.Invoke);
    }
}