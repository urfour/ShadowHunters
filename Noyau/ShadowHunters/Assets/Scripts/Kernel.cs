using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using EventSystem;
using EventExemple.Kernel.Players;
using EventExemple.Kernel.Players.event_out;
using Scripts.Settings;
using EventExemple.Kernel.Players.event_in;
using EventExemple.Scripts.Players.event_out;

/// <summary>
/// Classe représentant la logique du jeu, à savoir la gestion des règles et des interactions 
/// </summary>
public class Kernell : MonoBehaviour, IListener<PlayerEvent>
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
        /*visionCardsButton.gameObject.SetActive(false);
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
        takingWoundsToggle.gameObject.SetActive(false);*/
        //InitInterface();
        //ChooseNextPlayer();
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
    /// Test de mort d'un joueur après avoir subi des Blessures
    /// </summary>
    /// <param name="playerId">Id du joueur à tester</param>
    void CheckPlayerDeath(int playerId)
    {
        if (m_players[playerId].IsDead())
        {
            Debug.Log("Le joueur " + m_players[PlayerTurn.Value].Name + " est mort !");

            if (m_nbHuntersDead == 0 && m_nbShadowsDeads == 0 && m_nbNeutralsDeads == 0)
            {
                foreach (Player player in m_players)
                {
                    if (player.Character.characterType == CharacterType.Daniel)
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

            // if(m_players[PlayerTurn.Value].Character.characterType == CharacterType.Bryan && m_players[playerId].Life <= 12 && !m_players[PlayerTurn.Value].Revealed)
            // {
            //     PlayerCardPower(m_players[PlayerTurn.Value]);
            // }
        }
    }


    public void OnEvent(PlayerEvent e, string[] tags = null)
    {
        if (e is EndTurnEvent ete)
        {
            Console.WriteLine("Endturn of : " + ete.PlayerId);
            Player current = Player.GetPlayer(ete.PlayerId);
            if (PlayerTurn.Value == -1)
                PlayerTurn.Value = UnityEngine.Random.Range(0, m_nbPlayers - 1);
            else if (m_players[current.Value].HasAncestral.Value) // si le joueur a utilisé le savoir ancestral, le joueur suivant reste lui
            {
                Console.WriteLine("Le joueur " + m_players[current.Value].Name + " rejoue grâce au Savoir Ancestral !");
                m_players[current.Value].HasAncestral.Value = false;
            }
            else
                current.Value = (current.Value + 1) % m_nbPlayers;
            Console.WriteLine("C'est au joueur " + m_players[current.Value].Name + " de jouer.");

            if (m_players[current.Value].HasGuardian.Value)
            {
                m_players[current.Value].HasGuardian.Value = false;
                Console.WriteLine("Le joueur " + m_players[current.Value].Name + " n'est plus affecté par l'Ange Gardien !");
            }
            EventView.Manager.Emit(new SelectedNextPlayer()
            {
                PlayerId = current.Value
            });
        }
        else if(e is NewTurnEvent nte)
        {
            Player current = Player.GetPlayer(nte.PlayerId);
            List<Position> position = new List<Position>();

            if (m_players[current].HasCompass.Value)
            {   
                int lancer01 = UnityEngine.Random.Range(1, 6);
                int lancer02 = UnityEngine.Random.Range(1, 4);
                int lancer11 = UnityEngine.Random.Range(1, 6);
                int lancer12 = UnityEngine.Random.Range(1, 4);  
                
                EventView.Manager.Emit(new SelectDiceThrow()
                {
                    PlayerId = current.Value,
                    D6Dice1 = lancer01,
                    D4Dice1 = lancer02,
                    D6Dice2 = lancer11,
                    D4Dice2 = lancer12,
                });
            }
            else
            {
                while (!position.Any())
                {
                    int lancer01 = UnityEngine.Random.Range(1, 6);
                    int lancer02 = UnityEngine.Random.Range(1, 4);
                    
                    switch (lancer01+lancer02)
                    {
                        case 2:
                        case 3:
                            position.Add(Position.Antre);
                            break;
                        case 4:
                        case 5:
                            position.Add(Position.Porte);
                            break;
                        case 6:
                            position.Add(Position.Monastere);
                            break;
                        case 7:
                            position.Add(Position.Antre);
                            position.Add(Position.Porte);
                            position.Add(Position.Monastere);
                            position.Add(Position.Cimetiere);
                            position.Add(Position.Foret);
                            position.Add(Position.Foret);
                            break;
                        case 8:
                            position.Add(Position.Cimetiere);
                            break;
                        case 9:
                            position.Add(Position.Foret);
                            break;
                        case 10:
                            position.Add(Position.Sanctuaire);
                            break;
                    }
                    if (position.Contains(m_players[PlayerTurn.Value].Position))
                    {
                        m_players[current.Value].Position = position.ToArray();
                    }
                    else
                        position.Remove(m_players[current.Value].Position);

                }
                EventView.Manager.Emit(new SelectMovement()
                {
                    PlayerId = current.Value,
                    D6Dice = lancer01,
                    D4Dice = lancer02,
                    LocationAvailable = position.ToArray()
                });
            }

        }
        else if(e is SelectedDiceEvent sde)
        {
            Player current=Player.GetPlayer(sde.PlayerId);
            List<Position> position = new List<Position>();

            while (!position.Any())
            {
                switch (sde.D4Dice+sde.D6Dice)
                {
                    case 2:
                    case 3:
                        position.Add(Position.Antre);
                        break;
                    case 4:
                    case 5:
                        position.Add(Position.Porte);
                        break;
                    case 6:
                        position.Add(Position.Monastere);
                        break;
                    case 7:
                        position.Add(Position.Antre);
                        position.Add(Position.Porte);
                        position.Add(Position.Monastere);
                        position.Add(Position.Cimetiere);
                        position.Add(Position.Foret);
                        position.Add(Position.Sanctuaire);
                        break;
                    case 8:
                        position.Add(Position.Cimetiere);
                        break;
                    case 9:
                        position.Add(Position.Foret);
                        break;
                    case 10:
                        position.Add(Position.Sanctuaire);
                        break;
                }
                if (position.Contains(m_players[current.Value].Position))
                {
                    m_players[current.Value].Position = position.ToArray();
                }
                else
                    position.Remove(m_players[current.Value].Position);

            }
            EventView.Manager.Emit(new SelectMovement()
            {
                PlayerId = current.Value,
                D6Dice = lancer01,
                D4Dice = lancer02,
                LocationAvailable = position.ToArray()
            });
        }
        else if (e is MoveOn mo)
        {
            Player current = Player.GetPlayer(mo.PlayerId);
            m_players[current].Position = mo.Location;
        }
        else if (e is PowerUsedEvent powerUsed)
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
                        playerAttacked.CanUsePower.Value = true;
                    }

                    // Charles peut attaquer de nouveau
                    if (playerAttacking.Character.characterType == CharacterType.Charles
                        && playerAttacking.Revealed.Value)
                    {
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
                    DarknessCardPower(playerStealed.ListCard[playerStealed.ListCard.Count - 1] as DarknessCard);
                    LooseEquipmentCard(playerStealed.Id, indexCard, 0);
                }
                else if (playerStealed.ListCard[indexCard].cardType == CardType.Light)
                {
                    LightCardPower(playerStealed.ListCard[playerStealed.ListCard.Count - 1] as LightCard);
                    LooseEquipmentCard(playerStealed.Id, indexCard, 1);
                }
            }
            else
            {
                Debug.LogError("Erreur : la carte choisie n'est pas un équipement et ne devrait pas être là.");
            }

            Debug.Log("La carte " + stealedCard + " a été volée au joueur "
                + playerStealed.Name + " par le joueur " + playerStealing.Name + " !");
        }
        else if (e is GiveCardEvent giveCard)
        {
            Player playerGiving = m_players[giveCard.PlayerId];
            Player playerGived = m_players[giveCard.PlayerGivedId];
            string givedCard = giveCard.CardGivedName;

            int indexCard = playerGiving.HasCard(givedCard);
            playerGived.AddCard(playerGiving.ListCard[indexCard]);

            if (playerGiving.ListCard[indexCard].isEquipement)
            {
                if (playerGiving.ListCard[indexCard].cardType == CardType.Darkness)
                {
                    DarknessCardPower(playerGiving.ListCard[playerGiving.ListCard.Count - 1] as DarknessCard);
                    LooseEquipmentCard(playerGiving.Id, indexCard, 0);
                }
                else if (playerGiving.ListCard[indexCard].cardType == CardType.Light)
                {
                    LightCardPower(playerGiving.ListCard[playerGiving.ListCard.Count - 1] as LightCard);
                    LooseEquipmentCard(playerGiving.Id, indexCard, 1);
                }
            }
            else
                Debug.LogError("Erreur : la carte choisie n'est pas un équipement et ne devrait pas être là.");

            Debug.Log("La carte " + givedCard + " a été donnée au joueur "
                + playerGived.Name + " par le joueur " + playerGiving.Name + " !");
        }
    }
}
