using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using EventSystem;
using EventExemple.Kernel.Players;
using EventExemple.Kernel.Players.event_out;
using Kernel.Settings;

/// <summary>
/// Classe représentant la logique du jeu, à savoir la gestion des règles et des interactions 
/// </summary>
public class GameLogic : MonoBehaviour, IListener<PlayerEvent>
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
    //private int m_playerTurn = -1;
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
    public Setting<int> PlayerTurn { get; private set; } = new Setting<int>(-1);
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


    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    public static void OnBeforeSceneLoadRuntimeMethod()
    {
        EventView.Load();
    }

    /// <summary>
    /// Fonction appelée dès l'instanciation du GameObject auquel est lié le script,
    /// permettant de préparer le jeu.
    /// </summary>
    void Start()
    {
        EventView.Manager.AddListener(this, true);
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
        woundsForestToggle.gameObject.SetActive(false);
        healForestToggle.gameObject.SetActive(false);
        giveEquipmentToggle.gameObject.SetActive(false);
        takingWoundsToggle.gameObject.SetActive(false);
        InitInterface();
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


    public void OnEvent(PlayerEvent e, string[] tags = null)
    {
        if (e is PowerUsedEvent powerUsed)
        {
            PlayerCardPower(m_players[powerUsed.playerId]);
        }
        else if (e is PowerNotUsedEvent powerNotUsed)
        {
            DontUsePower(m_players[powerNotUsed.playerId]);
        }
        else if (e is AttackPlayerEvent attackPlayer)
        {
            Player playerAttacking = m_players[attackPlayer.playerId];
            Player playerAttacked = m_players[attackPlayer.playerAttackedId];

            int lancer1 = UnityEngine.Random.Range(1, 6);
            int lancer2 = UnityEngine.Random.Range(1, 4);
            int lancerTotal = (playerAttacking.HasSaber.Value == true) ? lancer2 : Mathf.Abs(lancer1 - lancer2);
            
            if (lancerTotal == 0)
                Debug.Log("Le lancer vaut 0, vous n'attaquez pas.");
            else
            {
                Debug.Log("Vous choisissez d'attaquer le joueur " + playerAttacked.Name + ".");

                int dommageTotal = lancerTotal + playerAttacking.BonusAttack.Value - playerAttacking.MalusAttack.Value;

                // Si Bob est révélé et inflige 2 dégats ou plus, il peut voler une arme 
                if (playerAttacking.Character.characterType == CharacterType.Bob
                    && playerAttacking.Revealed.Value
                    && dommageTotal >= 2)
                {
                    m_damageBob = dommageTotal;

                    usePowerButton.gameObject.SetActive(true);
                    dontUsePowerButton.gameObject.SetActive(true);

                    playerAttacking.CanUsePower.Value = true;
                    playerAttacking.CanNotUsePower.Value = true;
                }
                else
                {
                    // Le joueur attaqué se prend des dégats
                    playerAttacked.Wounded(dommageTotal);

                    // Le Vampire se soigne 2 blessures s'il est révélé et s'il a infligé des dégats
                    if (playerAttacking.Character.characterType == CharacterType.Vampire
                        && playerAttacking.Revealed.Value
                        && dommageTotal > 0)
                        PlayerCardPower(playerAttacking);

                    // On vérifie si le joueur attaqué est mort
                    CheckPlayerDeath(playerAttacked.Id);

                    // Le Loup-garou peut contre attaquer
                    if (playerAttacked.Character.characterType == CharacterType.LoupGarou
                        && playerAttacked.Revealed.Value)
                    {
                        usePowerButton.gameObject.SetActive(true);

                        playerAttacked.CanUsePower.Value = true;
                    }

                    // Charles peut attaquer de nouveau
                    if (playerAttacking.Character.characterType == CharacterType.Charles
                        && playerAttacking.Revealed.Value)
                    {
                        usePowerButton.gameObject.SetActive(true);

                        playerAttacking.CanUsePower.Value = true;
                    }
                }
            }
        }
        else if (e is StealCardEvent stealTarget)
        {
            Player playerStealing = m_players[stealTarget.playerId];
            Player playerStealed = m_players[stealTarget.playerStealedId];
            string stealedCard = stealTarget.cardStealedName;
            
            int indexCard = playerStealed.HasCard(stealedCard);
            playerStealing.AddCard(playerStealed.ListCard[indexCard]);

            if (playerStealed.ListCard[indexCard].isEquipement)
            {
                if (playerStealed.ListCard[indexCard].cardType == CardType.Darkness)
                {
                    StartCoroutine(DarknessCardPower(playerStealed.ListCard[playerStealed.ListCard.Count - 1] as DarknessCard));
                    StartCoroutine(LooseEquipmentCard(playerStealed.Id, indexCard, 0));
                }
                else if (playerStealed.ListCard[indexCard].cardType == CardType.Light)
                {
                    StartCoroutine(LightCardPower(playerStealed.ListCard[playerStealed.ListCard.Count - 1] as LightCard));
                    StartCoroutine(LooseEquipmentCard(playerStealed.Id, indexCard, 1));
                }
            }
            else
            {
                Debug.LogError("Erreur : la carte choisie n'est pas un équipement et ne devrait pas être là.");
            }

            Debug.Log("La carte " + stealedCard + " a été volée au joueur "
                + playerStealed.Name + " par le joueur " + playerStealing.Name + " !");
        }
    }
}
