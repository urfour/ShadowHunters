using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using EventSystem;
using Scripts;
using Scripts.Settings;
using Scripts.event_in;
using Scripts.event_out;

/// <summary>
/// Classe représentant la logique du jeu, à savoir la gestion des règles et des interactions 
/// </summary>
public class Kernel : MonoBehaviour, IListener<PlayerEvent>
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

    /// <summary>
    /// Perte d'une carte équipement pour un joueur
    /// </summary>
    /// <param name="PlayerId">Id du joueur</param>
    /// <param name="CardId">Id de la carte</param>
    /// <param name="type">Type de la carte, 0=Dark 1=Light</param>
    /// <returns></returns>
    void LooseEquipmentCard(int PlayerId, int CardId, int type)
    {
        CharacterTeam team = m_players[PlayerId].Team;
        string character = m_players[PlayerId].Character.characterName;
        bool revealed = m_players[PlayerId].Revealed.Value;

        if (type == 0)
        {
            DarknessCard card = m_players[PlayerId].ListCard[CardId] as DarknessCard;
            DarknessEffect effect = card.darknessEffect;

            switch (effect)
            {
                case DarknessEffect.Mitrailleuse:
                    m_players[PlayerId].HasGatling.Value = false;
                    break;

                case DarknessEffect.Sabre:
                    m_players[PlayerId].HasSaber.Value = false;
                    break;

                case DarknessEffect.Hache:
                    m_players[PlayerId].BonusAttack.Value--;
                    break;

                case DarknessEffect.Revolver:
                    m_players[PlayerId].HasRevolver.Value = false;
                    break;
            }
        }
        else if (type == 1)
        {
            LightCard card = m_players[PlayerId].ListCard[CardId] as LightCard;
            LightEffect effect = card.lightEffect;

            switch (effect)
            {
                case LightEffect.Lance:
                    m_players[PlayerId].HasSpear.Value = false;
                    if (team == CharacterTeam.Hunter && revealed)
                    {
                        m_players[PlayerId].BonusAttack.Value -= 2;
                    }
                    break;

                case LightEffect.Boussole:
                    m_players[PlayerId].HasCompass.Value = false;
                    break;

                case LightEffect.Broche:
                    m_players[PlayerId].HasBroche.Value = false;
                    break;

                case LightEffect.Toge:
                    m_players[PlayerId].HasToge.Value = false;
                    m_players[PlayerId].MalusAttack.Value--;
                    m_players[PlayerId].ReductionWounds.Value = 0;
                    break;

                case LightEffect.Crucifix:
                    m_players[PlayerId].HasCrucifix.Value = false;
                    break;

                case LightEffect.Amulette:
                    m_players[PlayerId].HasAmulet.Value = false;
                    break;
            }
        }
        else
        {
            Debug.LogError("Erreur : type en paramètre invalide.");
        }

        m_players[PlayerId].RemoveCard(CardId);
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
    void AttackCorrespondingPlayer(int playerAttackingId)
    {
        List<Player> players = GetPlayersSameSector(playerAttackingId, m_players[playerAttackingId].HasRevolver.Value);

        if (players.Count != 0)
        {
            if (!m_players[playerAttackingId].HasGatling.Value)
            {
                List<int> playersId = new List<int>();
                foreach (Player player in players)
                    playersId.Add(player.Id);

                EventView.Manager.Emit(new SelectAttackTargetEvent()
                {
                    PlayerId = playerAttackingId,
                    PossibleTargetId = playersId.ToArray(),
                });
            }
            else
            {
                int lancer1 = UnityEngine.Random.Range(1, 6);
                int lancer2 = UnityEngine.Random.Range(1, 4);
                int lancerTotal = (m_players[playerAttackingId].HasSaber.Value == true) ? lancer2 : Mathf.Abs(lancer1 - lancer2);
                if (lancerTotal == 0)
                    Debug.Log("Le lancer vaut 0, vous n'attaquez pas.");
                else
                {
                    foreach (Player player in players)
                    {
                        if ((m_players[playerAttackingId].Character.characterType == CharacterType.Bob) && lancerTotal >= 2)
                            PlayerCardPower(m_players[playerAttackingId]);
                        else
                        {
                            m_players[player.Id].Wounded(lancerTotal + m_players[playerAttackingId].BonusAttack.Value - m_players[playerAttackingId].MalusAttack.Value);

                            // On vérifie si le joueur attaqué est mort
                            CheckPlayerDeath(player.Id);

                            // Le Vampire se soigne 2 blessures s'il est révélé et s'il a infligé des dégats
                            if (m_players[playerAttackingId].Character.characterType == CharacterType.Vampire
                                && m_players[playerAttackingId].Revealed.Value
                                && lancerTotal + m_players[playerAttackingId].BonusAttack.Value - m_players[playerAttackingId].MalusAttack.Value > 0)
                                PlayerCardPower(m_players[playerAttackingId]);

                            // Le Loup-garou peut contre attaquer
                            if (m_players[player.Id].Character.characterType == CharacterType.LoupGarou
                                && m_players[player.Id].Revealed.Value)
                            {
                                m_players[player.Id].CanUsePower.Value = true;
                            }
                        }
                    }
                }
            }
        }
        else
        {
            Debug.Log("Vous ne pouvez attaquer aucun joueur.");
        }
    }

    /// <summary>
    /// Fonction du choix d'un joueur à attaquer
    /// </summary>
    /// <param name="playerAttackingId">Id du joueur attaquant</param>
    /// <param name="targetId">Id du joueur attaqué</param>
	/// <param name="damage"> Dommage précedemment occasioné</param>
    /// <returns>Iteration terminée</returns>
    void AttackCorrespondingPlayer(int playerAttackingId, int targetId, int damage)
    {
        if (damage > 0)
        {
            m_players[targetId].Wounded(damage);
            CheckPlayerDeath(targetId);
            if (m_players[targetId].Character.characterType == CharacterType.LoupGarou
                            && m_players[targetId].Revealed.Value)
                m_players[targetId].CanUsePower.Value = true;
        }
        else
        {
            List<Player> players = GetPlayersSameSector(playerAttackingId, m_players[playerAttackingId].HasRevolver.Value);
            if (players.Count == 0)
            {
                Debug.Log("Vous ne pouvez attaquer aucun joueur.");
            }
            else
            {
                m_playerAttackedId = targetId;

                int lancer1 = UnityEngine.Random.Range(1, 6);
                int lancer2 = UnityEngine.Random.Range(1, 4);
                int lancerTotal = (m_players[playerAttackingId].HasSaber.Value == true) ? lancer2 : Mathf.Abs(lancer1 - lancer2);
                if (lancerTotal == 0)
                {
                    Debug.Log("Le lancer vaut 0, vous n'attaquez pas.");
                }
                else
                {
                    if (m_players[playerAttackingId].HasGatling.Value)
                    {
                        foreach (Player player in players)
                        {
                            m_players[player.Id].Wounded(lancerTotal + m_players[playerAttackingId].BonusAttack.Value - m_players[playerAttackingId].MalusAttack.Value);

                            // On vérifie si le joueur attaqué est mort
                            CheckPlayerDeath(player.Id);

                            // Le Loup-garou peut contre attaquer
                            if (m_players[targetId].Character.characterType == CharacterType.LoupGarou
                                && m_players[targetId].Revealed.Value)
                                m_players[targetId].CanUsePower.Value = true;

                            // Le Vampire se soigne 2 blessures s'il est révélé et s'il a infligé des dégats
                            if (m_players[playerAttackingId].Character.characterType == CharacterType.Vampire
                                && m_players[playerAttackingId].Revealed.Value
                                && lancerTotal + m_players[playerAttackingId].BonusAttack.Value - m_players[playerAttackingId].MalusAttack.Value > 0)
                                PlayerCardPower(m_players[playerAttackingId]);
                        }
                    }
                    else
                    {

                        Debug.Log("Vous choisissez d'attaquer le joueur " + m_players[targetId].Name + ".");

                        m_players[targetId].Wounded(lancerTotal + m_players[playerAttackingId].BonusAttack.Value - m_players[playerAttackingId].MalusAttack.Value);

                        // Le Loup-garou peut contre attaquer
                        if (m_players[targetId].Character.characterType == CharacterType.LoupGarou
                            && m_players[targetId].Revealed.Value)
                            m_players[targetId].CanUsePower.Value = true;

                        // Le Vampire se soigne 2 blessures s'il est révélé et s'il a infligé des dégats
                        if (m_players[playerAttackingId].Character.characterType == CharacterType.Vampire
                            && m_players[playerAttackingId].Revealed.Value
                            && lancerTotal + m_players[playerAttackingId].BonusAttack.Value - m_players[playerAttackingId].MalusAttack.Value > 0)
                            PlayerCardPower(m_players[playerAttackingId]);

                        CheckPlayerDeath(targetId);
                    }
                }
            }
        }
    }

    private void PlayerCardPower(Player player)
    {
        throw new NotImplementedException();
    }


    /// <summary>
    /// Choix du joueur à qui voler une carte équipement
    /// </summary>
    /// <param name="thiefId">Id du joueur voleur</param>
    /// <returns>Itération terminée</returns>
    void StealEquipmentCard(int thiefId)
    {
        List<int> choices = new List<int>();
        foreach (Player player in m_players)
            if (!player.IsDead() && player.Id != thiefId && player.ListCard.Count > 0)
                choices.Add(player.Id);

        if(choices.Count != 0)
        {
            EventView.Manager.Emit(new SelectStealCardEvent()
            {
                PlayerId = thiefId,
                PossiblePlayerTargetId = choices.ToArray()
            });
        }
        else
        {
            Debug.Log("Il n'y a aucun joueur à qui voler une carte équipement.");
        }
    }

    /// <summary>
    /// Vol d'une carte équipement à un joueur précis
    /// </summary>
    /// <param name="thiefId">Id du joueur voleur</param>
    /// <param name="playerId">Id du joueur à qui voler une carte</param>
    /// <returns></returns>
    void StealEquipmentCard(int thiefId, int playerId)
    {
        EventView.Manager.Emit(new SelectStealCardFromPlayerEvent()
        {
            PlayerId = thiefId,
            PlayerStealedId = playerId
        });
    }

    /// <summary>
    /// Choix d'une carte équipement à donner et du joueur à qui la donner
    /// </summary>
    /// <param name="giverPlayerId">Id du joueur donneur</param>
    /// <returns>Itération terminée</returns>
    void GiveEquipmentCard(int giverPlayerId)
    {

        if (m_players[giverPlayerId].ListCard.Count == 0)
        {
            Debug.Log("Vous ne possédez aucune carte équipement.");
            m_players[giverPlayerId].Wounded(1);
            CheckPlayerDeath(giverPlayerId);
        }
        else
        {
            List<int> choices = new List<int>();
            foreach (Player player in m_players)
                if (!player.IsDead() && player.Id != giverPlayerId && player.ListCard.Count > 0)
                    choices.Add(player.Id);

            EventView.Manager.Emit(new SelectGiveCardEvent()
            {
                PlayerId = giverPlayerId,
                PossibleTargetId = choices.ToArray()
            });
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
    void TakingWoundsEffect(bool isPuppet, int nbWoundsTaken, int nbWoundsSelfHealed)
    {
        List<int> players = new List<int>();
        foreach (Player player in m_players)
        {
            if (!player.IsDead() && player.Id != PlayerTurn.Value)
            {
                if (isPuppet)
                    players.Add(player.Id);
                else
                {
                    if (!player.HasAmulet.Value)
                        players.Add(player.Id);
                }
            }
        }

        EventView.Manager.Emit(new SelectPlayerTakingWoundsEvent()
        {
            PlayerId = PlayerTurn.Value,
            PossibleTargetId = players.ToArray(),
            IsPuppet = true,
            NbWoundsTaken = nbWoundsTaken,
            NbWoundsSelfHealed = nbWoundsSelfHealed
        });
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
            PlayerCardPower(m_players[powerUsed.PlayerId]);
        }
        else if (e is PowerNotUsedEvent powerNotUsed)
        {
            DontUsePower(m_players[powerNotUsed.PlayerId]);
        }
        else if (e is AttackPlayerEvent attackPlayer)
        {
            Player playerAttacking = m_players[attackPlayer.PlayerId];
            Player playerAttacked = m_players[attackPlayer.PlayerAttackedId];

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
            Player playerStealing = m_players[stealTarget.PlayerId];
            Player playerStealed = m_players[stealTarget.PlayerStealedId];
            string stealedCard = stealTarget.CardStealedName;
            
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
        else if (e is TakingWoundsEffectEvent takingWounds)
        {
            Player playerAttacking = m_players[takingWounds.PlayerId];
            Player playerAttacked = m_players[takingWounds.PlayerAttackedId];
            bool isPuppet = takingWounds.IsPuppet;
            int nbWoundsTaken = takingWounds.NbWoundsTaken;
            int nbWoundsSelfHealed = takingWounds.NbWoundsSelfHealed;

            if (isPuppet)
            {
                int lancer = UnityEngine.Random.Range(1, 6);
                Debug.Log("Le lancer donne " + lancer + ".");
                if (lancer <= 4)
                {
                    playerAttacked.Wounded(nbWoundsTaken);
                    CheckPlayerDeath(playerAttacked.Id);
                }
                else
                {
                    playerAttacking.Wounded(nbWoundsTaken);
                    CheckPlayerDeath(playerAttacking.Id);
                }
            }
            else
            {
                playerAttacked.Wounded(nbWoundsTaken);
                CheckPlayerDeath(playerAttacked.Id);
                if (nbWoundsSelfHealed < 0)
                {
                    playerAttacking.Wounded(-nbWoundsSelfHealed);
                    CheckPlayerDeath(playerAttacking.Id);
                }
                else
                    playerAttacking.Healed(nbWoundsSelfHealed);
            }
        }
    }

    private void LightCardPower(LightCard lightCard)
    {
        throw new NotImplementedException();
    }

    private void DarknessCardPower(DarknessCard darknessCard)
    {
        throw new NotImplementedException();
    }
}
