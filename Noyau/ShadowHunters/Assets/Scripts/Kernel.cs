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
    /// Propriété d'accès à l'id du joueur attaqué
    /// </summary>
    public Setting<int> PlayerAttacked { get; private set; } = new Setting<int>(-1);
    /// <summary>
    /// Dégats pris par le joueur attaqué par Bob
    /// </summary>    
    private int m_damageBob = -1;
    /// <summary>
    /// Pouvoir de Emi/Franck/Georges possible
    /// </summary>    
    private bool powerEFG = false;
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
    /// Activation de l'effet d'une carte Ténèbre piochée
    /// </summary>
    /// <param name="pickedCard">Carte Ténèbre piochée</param>
    /// <param name="idPlayer">Joueur qui bénéficie de l'effet</param>
    /// <returns>Itération terminée</returns>
    void DarknessCardPower(DarknessCard pickedCard, int idPlayer)
    {
        switch (pickedCard.darknessEffect)
        {
            case DarknessEffect.Araignee:
                TakingWoundsEffect(false, 2, -2);
                break;
            case DarknessEffect.Banane:
                GiveEquipmentCard(idPlayer);
                break;
            case DarknessEffect.ChauveSouris:
                TakingWoundsEffect(false, 2, 1);
                break;
            case DarknessEffect.Dynamite:
                int lancer1 = UnityEngine.Random.Range(1, 6);
                int lancer2 = UnityEngine.Random.Range(1, 4);
                int lancerTotal = lancer1 + lancer2;
                Position area = Position.None;

                switch (lancerTotal)
                {
                    case 2:
                    case 3:
                        area = Position.Antre;
                        break;
                    case 4:
                    case 5:
                        area = Position.Porte;
                        break;
                    case 6:
                        area = Position.Monastere;
                        break;
                    case 7:
                        Debug.Log("Rien ne se passe");
                        break;
                    case 8:
                        area = Position.Cimetiere;
                        break;
                    case 9:
                        area = Position.Foret;
                        break;
                    case 10:
                        area = Position.Sanctuaire;
                        break;
                }
                if (lancerTotal != 7)
                {
                    foreach (Player p in m_players)
                    {
                        if (p.Position == area && !p.HasAmulet.Value)
                            p.Wounded(3);
                    }
                }
                break;
            case DarknessEffect.Hache:
                m_players[idPlayer].BonusAttack.Value++;
                break;
            case DarknessEffect.Mitrailleuse:
                m_players[idPlayer].HasGatling.Value = true;
                break;
            case DarknessEffect.Poupee:
                TakingWoundsEffect(true, 3, 0);
                break;
            case DarknessEffect.Revolver:
                m_players[idPlayer].HasRevolver.Value = true;
                break;
            case DarknessEffect.Rituel:
                Debug.Log("Voulez-vous vous révéler ? Vous avez 6 secondes, sinon la carte se défausse.");

                if (m_players[idPlayer].Revealed.Value && m_players[idPlayer].Team == CharacterTeam.Shadow)
                {
                    m_players[idPlayer].Healed(m_players[idPlayer].Wound.Value);
                    Debug.Log("Le joueur " + m_players[idPlayer].Name + " se soigne complètement");
                }
                else
                {
                    EventView.Manager.Emit(new SelectRevealOrNotEvent()
                    {
                        PlayerId = idPlayer,
                        EffectCard = pickedCard
                    });
                }
                break;
            case DarknessEffect.Sabre:
                m_players[idPlayer].HasSaber.Value = true;
                break;
            case DarknessEffect.Succube:
                StealEquipmentCard(idPlayer);
                break;
        }
    }

    /// <summary>
    /// Activation de l'effet d'une carte Lumière piochée
    /// </summary>
    /// <param name="pickedCard">Carte Lumière piochée</param>
    /// <param name="idPlayer">Joueur qui bénéficie de l'effet</param>
    /// <returns>Itération terminée</returns>
    void LightCardPower(LightCard pickedCard, int idPlayer)
    {
        CharacterTeam team = m_players[idPlayer].Team;
        CharacterType character = m_players[idPlayer].Character.characterType;
        bool revealed = m_players[idPlayer].Revealed.Value;
        Debug.Log(pickedCard.lightEffect);
        switch (pickedCard.lightEffect)
        {
            case LightEffect.Amulette:
                m_players[idPlayer].HasAmulet.Value = true;
                break;

            case LightEffect.AngeGardien:

                m_players[idPlayer].HasGuardian.Value = true;
                break;

            case LightEffect.Supreme:
                Debug.Log("Voulez-vous vous révéler ? Vous avez 6 secondes, sinon la carte se défausse.");

                if (m_players[idPlayer].Revealed.Value && m_players[idPlayer].Team == CharacterTeam.Hunter)
                {
                    m_players[idPlayer].Healed(m_players[idPlayer].Wound.Value);
                    Debug.Log("Le joueur " + m_players[idPlayer].Name + " se soigne complètement");
                }
                else
                {
                    EventView.Manager.Emit(new SelectRevealOrNotEvent()
                    {
                        PlayerId = idPlayer,
                        EffectCard = pickedCard
                    });
                }
                break;

            case LightEffect.Chocolat:
                Debug.Log("Voulez-vous vous révéler ? Vous avez 6 secondes, sinon la carte se défausse.");

                if (m_players[idPlayer].Revealed.Value
                    && (character == CharacterType.Allie
                        || character == CharacterType.Emi
                        || character == CharacterType.Metamorphe))
                {
                    m_players[idPlayer].Healed(m_players[idPlayer].Wound.Value);
                    Debug.Log("Le joueur " + m_players[idPlayer].Name + " se soigne complètement");
                }
                else
                {
                    EventView.Manager.Emit(new SelectRevealOrNotEvent()
                    {
                        PlayerId = idPlayer,
                        EffectCard = pickedCard
                    });
                }
                break;

            case LightEffect.Benediction:

                Debug.Log("Qui souhaitez-vous soigner ?");
                List<int> players = new List<int>();
                foreach (Player player in m_players)
                {
                    if (!player.IsDead() && player.Id != idPlayer)
                    {
                        players.Add(player.Id);
                    }
                }

                EventView.Manager.Emit(new SelectLightCardTargetEvent()
                {
                    PlayerId = idPlayer,
                    PossibleTargetId = players.ToArray(),
                    LightCard = pickedCard
                });

                break;

            case LightEffect.Boussole:
                m_players[idPlayer].HasCompass.Value = true;
                break;

            case LightEffect.Broche:
                m_players[idPlayer].HasBroche.Value = true;
                break;

            case LightEffect.Crucifix:
                m_players[idPlayer].HasCrucifix.Value = true;
                break;

            case LightEffect.EauBenite:

                m_players[idPlayer].Healed(2);
                break;

            case LightEffect.Eclair:

                foreach (Player p in m_players)
                {
                    if (p.Id != idPlayer)
                        p.Wounded(2);
                }
                break;

            case LightEffect.Lance:
                m_players[idPlayer].HasSpear.Value = true;
                if (team == CharacterTeam.Hunter && revealed)
                {
                    m_players[idPlayer].BonusAttack.Value += 2;
                }
                break;

            case LightEffect.Miroir:

                if (!revealed && team == CharacterTeam.Shadow && character != CharacterType.Metamorphe)
                {
                    RevealCard();
                    Debug.Log("Vous révélez votre rôle à tous, vous êtes : " + character);
                }
                break;

            case LightEffect.PremiersSecours:

                Debug.Log("Qui souhaitez-vous placer à exactement 7 Blessures ?");

                List<int> players2 = new List<int>();
                foreach (Player player in m_players)
                {
                    if (!player.IsDead())
                    {
                        players2.Add(player.Id);
                    }
                }

                EventView.Manager.Emit(new SelectLightCardTargetEvent()
                {
                    PlayerId = idPlayer,
                    PossibleTargetId = players2.ToArray(),
                    LightCard = pickedCard
                });

                break;

            case LightEffect.Savoir:

                m_players[idPlayer].HasAncestral.Value = true;
                break;

            case LightEffect.Toge:

                m_players[idPlayer].HasToge.Value = true;
                m_players[idPlayer].MalusAttack.Value++;
                m_players[idPlayer].ReductionWounds.Value = 1;
                break;
        }
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
    void VisionCardPower(VisionCard pickedCard)
    {
        Debug.Log("Message au joueur " + m_players[PlayerTurn.Value].Name + " : ");
        Debug.Log("Carte Vision piochée : " + pickedCard.cardName);
        Debug.Log(pickedCard.description);

        List<int> players = new List<int>();
        for (int i = 0; i < m_nbPlayers; i++)
            if (!m_players[i].IsDead() && i != PlayerTurn.Value)
                players.Add(m_players[i].Id);

        EventView.Manager.Emit(new SelectVisionPowerEvent()
        {
            PlayerId = PlayerTurn.Value,
            PossiblePlayerTargetId = players.ToArray(),
            VisionCard = pickedCard
        });
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
        m_players[PlayerTurn.Value].Revealed.Value = true;
        Debug.Log("Le joueur " + m_players[PlayerTurn.Value].Name + " s'est révélé, il s'agissait de : "
            + m_players[PlayerTurn.Value].Character.characterName + " ! Il est dans l'équipe des "
            + m_players[PlayerTurn.Value].Character.team + ".");

        if (m_players[PlayerTurn.Value].HasSpear.Value == true && m_players[PlayerTurn.Value].Team == CharacterTeam.Hunter)
        {
            m_players[PlayerTurn.Value].BonusAttack.Value += 2;
            Debug.Log("Le pouvoir de la lance s'active !");
        }

        // Si le joueur est Allie, il peut utiliser son pouvoir à tout moment
        // Si le joueur est Emi, Franklin ou Georges et qu'il est au début de son tour, il peut utiliser son pouvoir
        if (m_players[PlayerTurn.Value].Character.characterType == CharacterType.Allie
            || (powerEFG
                && (m_players[PlayerTurn.Value].Character.characterType == CharacterType.Emi
                    || m_players[PlayerTurn.Value].Character.characterType == CharacterType.Franklin
                    || m_players[PlayerTurn.Value].Character.characterType == CharacterType.Georges)))
        {
            m_players[PlayerTurn.Value].CanUsePower.Value = true;
        }
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
                    PowerFranklin = false,
                    PowerGeorges = false
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
                        if (m_players[playerAttackingId].Character.characterType == CharacterType.Bob 
                            && m_players[playerAttackingId].Revealed.Value
                            && lancerTotal >= 2)
                        {
                            m_damageBob = lancerTotal + m_players[playerAttackingId].BonusAttack.Value - m_players[playerAttackingId].MalusAttack.Value;
                            PlayerAttacked.Value = player.Id;
                            PlayerCardPower(m_players[playerAttackingId]);
                        }
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
                            if (m_players[playerAttackingId].Character.characterType == CharacterType.Bob
                                && m_players[playerAttackingId].Revealed.Value
                                && lancerTotal >= 2)
                            {
                                m_damageBob = lancerTotal + m_players[playerAttackingId].BonusAttack.Value - m_players[playerAttackingId].MalusAttack.Value;
                                PlayerAttacked.Value = player.Id;
                                PlayerCardPower(m_players[playerAttackingId]);
                            }
                            else
                            {
                                m_players[player.Id].Wounded(lancerTotal + m_players[playerAttackingId].BonusAttack.Value - m_players[playerAttackingId].MalusAttack.Value);

                                // On vérifie si le joueur attaqué est mort
                                CheckPlayerDeath(player.Id);

                                // Le Loup-garou peut contre attaquer
                                if (player.Character.characterType == CharacterType.LoupGarou
                                    && player.Revealed.Value)
                                    player.CanUsePower.Value = true;

                                // Le Vampire se soigne 2 blessures s'il est révélé et s'il a infligé des dégats
                                if (m_players[playerAttackingId].Character.characterType == CharacterType.Vampire
                                    && m_players[playerAttackingId].Revealed.Value
                                    && lancerTotal + m_players[playerAttackingId].BonusAttack.Value - m_players[playerAttackingId].MalusAttack.Value > 0)
                                    PlayerCardPower(m_players[playerAttackingId]);
                            }
                        }
                    }
                    else
                    {
                        if (m_players[playerAttackingId].Character.characterType == CharacterType.Bob
                            && m_players[playerAttackingId].Revealed.Value
                            && lancerTotal >= 2)
                        {
                            m_damageBob = lancerTotal + m_players[playerAttackingId].BonusAttack.Value - m_players[playerAttackingId].MalusAttack.Value;
                            PlayerAttacked.Value = targetId;
                            PlayerCardPower(m_players[playerAttackingId]);
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

        if (choices.Count != 0)
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

    /// <summary>
    /// Activation de l'effet d'une carte Personnage
    /// </summary>
    /// <param name="player">Joueur utilisant l'effet de sa carte 
    /// Personnage</param>
    void PlayerCardPower(Player player)
    {
        m_players[player.Id].CanUsePower.Value = false;
        m_players[player.Id].CanNotUsePower.Value = false;

        switch (player.Character.characterType)
        {
            case CharacterType.Allie:
                // Il faut que le joueur se soit révélé et qu'il n'ait pas encore utilisé son pouvoir
                if (player.Revealed.Value && !player.UsedPower.Value)
                {
                    // Le joueur se soigne de toutes ses blessures
                    player.Healed(player.Wound.Value);
                }
                break;
            case CharacterType.Emi:
                // On cherche l'index de la carte Lieu dans la liste des lieux
                int indexEmi = gameBoard.GetIndexOfPosition(player.Position);

                if (indexEmi == -1)
                {
                    List<Position> position = new List<Position>();

                    if (player.HasCompass.Value)
                    {
                        int lancer01 = UnityEngine.Random.Range(1, 6);
                        int lancer02 = UnityEngine.Random.Range(1, 4);
                        int lancer11 = UnityEngine.Random.Range(1, 6);
                        int lancer12 = UnityEngine.Random.Range(1, 4);

                        EventView.Manager.Emit(new SelectDiceThrow()
                        {
                            PlayerId = player.Id,
                            D6Dice1 = lancer01,
                            D4Dice1 = lancer02,
                            D6Dice2 = lancer11,
                            D4Dice2 = lancer12,
                        });
                    }
                    else
                    {
                        int lancer01 = UnityEngine.Random.Range(1, 6);
                        int lancer02 = UnityEngine.Random.Range(1, 4);

                        while (position.Count >= 0)
                        {
                            lancer01 = UnityEngine.Random.Range(1, 6);
                            lancer02 = UnityEngine.Random.Range(1, 4);

                            switch (lancer01 + lancer02)
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
                            if (position.Contains(player.Position))
                            {
                                player.Position = position[0];
                            }
                            else
                                position.Remove(player.Position);

                        }
                        EventView.Manager.Emit(new SelectMovement()
                        {
                            PlayerId = player.Id,
                            D6Dice = lancer01,
                            D4Dice = lancer02,
                            LocationAvailable = position.ToArray()
                        });
                    }
                }
                else
                {
                    List<Position> position = new List<Position>();

                    position.Add(gameBoard.GetAreaAt((indexEmi - 1) % 6).area);
                    position.Add(gameBoard.GetAreaAt((indexEmi + 1) % 6).area);

                    EventView.Manager.Emit(new SelectMovement()
                    {
                        PlayerId = player.Id,
                        D6Dice = UnityEngine.Random.Range(1, 6),
                        D4Dice = UnityEngine.Random.Range(1, 4),
                        LocationAvailable = position.ToArray()
                    });
                }

                break;
            case CharacterType.Metamorphe:
                break;
            case CharacterType.Bob:
                // Il faut que le joueur se soit révélé et qu'il n'ait pas encore utilisé son pouvoir
                if (player.Revealed.Value)
                {
                    EventView.Manager.Emit(new SelectBobPowerEvent()
                    {
                        PlayerId = player.Id
                    });
                }
                break;
            case CharacterType.Franklin:
                if (player.Revealed.Value && !player.UsedPower.Value)
                {
                    List<int> playersId = new List<int>();
                    foreach (Player playerT in m_players)
                        if(playerT.Id != player.Id)
                            playersId.Add(player.Id);

                    player.UsedPower.Value = false;

                    EventView.Manager.Emit(new SelectAttackTargetEvent()
                    {
                        PlayerId = player.Id,
                        PossibleTargetId = playersId.ToArray(),
                        PowerFranklin = true,
                        PowerGeorges = false
                    });
                }
                break;
            case CharacterType.Georges:
                if (player.Revealed.Value && !player.UsedPower.Value)
                {
                    List<int> playersId = new List<int>();
                    foreach (Player playerT in m_players)
                        if (playerT.Id != player.Id)
                            playersId.Add(player.Id);

                    player.UsedPower.Value = false;

                    EventView.Manager.Emit(new SelectAttackTargetEvent()
                    {
                        PlayerId = player.Id,
                        PossibleTargetId = playersId.ToArray(),
                        PowerFranklin = false,
                        PowerGeorges = true
                    });
                }
                break;
            case CharacterType.LoupGarou:
                if (player.Revealed.Value)
                    AttackCorrespondingPlayer(player.Id, PlayerTurn.Value, 0);
                break;
            case CharacterType.Vampire:
                if (player.Revealed.Value)
                    player.Healed(2);
                break;
            case CharacterType.Charles:
                if (player.Revealed.Value)
                    player.Wounded(2);
                    AttackCorrespondingPlayer(player.Id, PlayerAttacked.Value, 0);
                break;
            case CharacterType.Daniel:
                // Il faut que le joueur se soit révélé et qu'il n'ait pas encore utilisé son pouvoir
                if (!player.Revealed.Value)
                    RevealCard();
                break;
        }
    }

    public void OnEvent(PlayerEvent e, string[] tags = null)
    {
        if (e is EndTurnEvent ete)
        {
            Console.WriteLine("Endturn of : " + ete.PlayerId);
            if (PlayerTurn.Value == -1)
                PlayerTurn.Value = UnityEngine.Random.Range(0, m_nbPlayers - 1);
            else if (m_players[PlayerTurn.Value].HasAncestral.Value) // si le joueur a utilisé le savoir ancestral, le joueur suivant reste lui
            {
                Console.WriteLine("Le joueur " + m_players[PlayerTurn.Value].Name + " rejoue grâce au Savoir Ancestral !");
                m_players[PlayerTurn.Value].HasAncestral.Value = false;
            }
            else
                PlayerTurn.Value = (PlayerTurn.Value + 1) % m_nbPlayers;
            Console.WriteLine("C'est au joueur " + m_players[PlayerTurn.Value].Name + " de jouer.");

            Player currentPlayer = m_players[PlayerTurn.Value];

            currentPlayer.RollTheDices.Value = true;

            if (currentPlayer.HasGuardian.Value)
            {
                currentPlayer.HasGuardian.Value = false;
                Console.WriteLine("Le joueur " + currentPlayer.Name + " n'est plus affecté par l'Ange Gardien !");
            }

            if (currentPlayer.Revealed.Value)
            {
                if (currentPlayer.Character.characterType == CharacterType.Emi
                    || currentPlayer.Character.characterType == CharacterType.Franklin
                    || currentPlayer.Character.characterType == CharacterType.Georges)
                {
                    currentPlayer.CanUsePower.Value = true;
                }
            }

            EventView.Manager.Emit(new SelectedNextPlayer()
            {
                PlayerId = PlayerTurn.Value
            });
        }
        else if (e is NewTurnEvent nte)
        {
            Player currentPlayer = m_players[nte.PlayerId];
            currentPlayer.RollTheDices.Value = false;

            if (currentPlayer.Character.characterType == CharacterType.Emi
                || currentPlayer.Character.characterType == CharacterType.Franklin
                || currentPlayer.Character.characterType == CharacterType.Georges)
            {
                currentPlayer.CanUsePower.Value = false;
            }

            List<Position> position = new List<Position>();

            if (currentPlayer.HasCompass.Value)
            {
                int lancer01 = UnityEngine.Random.Range(1, 6);
                int lancer02 = UnityEngine.Random.Range(1, 4);
                int lancer11 = UnityEngine.Random.Range(1, 6);
                int lancer12 = UnityEngine.Random.Range(1, 4);

                EventView.Manager.Emit(new SelectDiceThrow()
                {
                    PlayerId = currentPlayer.Id,
                    D6Dice1 = lancer01,
                    D4Dice1 = lancer02,
                    D6Dice2 = lancer11,
                    D4Dice2 = lancer12,
                });
            }
            else
            {
                int lancer01 = UnityEngine.Random.Range(1, 6);
                int lancer02 = UnityEngine.Random.Range(1, 4);

                while (position.Count >= 0)
                {
                    lancer01 = UnityEngine.Random.Range(1, 6);
                    lancer02 = UnityEngine.Random.Range(1, 4);

                    switch (lancer01 + lancer02)
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
                    if (position.Contains(currentPlayer.Position))
                    {
                        currentPlayer.Position = position[0];
                    }
                    else
                        position.Remove(currentPlayer.Position);

                }
                EventView.Manager.Emit(new SelectMovement()
                {
                    PlayerId = currentPlayer.Id,
                    D6Dice = lancer01,
                    D4Dice = lancer02,
                    LocationAvailable = position.ToArray()
                });
            }

        }
        else if (e is SelectedDiceEvent sde)
        {
            Player currentPlayer = m_players[sde.PlayerId];
            List<Position> position = new List<Position>();

            while (position.Count >= 0)
            {
                switch (sde.D4Dice + sde.D6Dice)
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
                if (position.Contains(currentPlayer.Position))
                {
                    currentPlayer.Position = position[0];
                }
                else
                    position.Remove(currentPlayer.Position);

            }
            EventView.Manager.Emit(new SelectMovement()
            {
                PlayerId = currentPlayer.Id,
                D6Dice = sde.D6Dice,
                D4Dice = sde.D4Dice,
                LocationAvailable = position.ToArray()
            });
        }
        else if (e is MoveOn mo)
        {
            Player currentPlayer = m_players[mo.PlayerId];
            currentPlayer.Position = mo.Location;
            gameBoard.setPositionOfAt(currentPlayer.Id, mo.Location);

            currentPlayer.AttackPlayer.Value = true;
            if (currentPlayer.HasSaber.Value)
                currentPlayer.EndTurn.Value = false;
            else
                currentPlayer.EndTurn.Value = true;

            switch (currentPlayer.Position)
            {
                case Position.Antre:
                    currentPlayer.DrawLightCard.Value = true;
                    break;
                case Position.Porte:
                    currentPlayer.DrawLightCard.Value = true;
                    currentPlayer.DrawDarknessCard.Value = true;
                    currentPlayer.DrawVisionCard.Value = true;
                    break;
                case Position.Monastere:
                    currentPlayer.DrawVisionCard.Value = true;
                    break;
                case Position.Cimetiere:
                    currentPlayer.DrawDarknessCard.Value = true;
                    break;
                case Position.Foret:
                    List<int> target = new List<int>();
                    foreach (Player p in m_players)
                        if (!p.IsDead())
                            target.Add(p.Id);

                    EventView.Manager.Emit(new ForestEvent()
                    {
                        PlayerId = currentPlayer.Id,
                        PossiblePlayerTargetId = target.ToArray(),
                    });
                    break;
                case Position.Sanctuaire:
                    List<int> target2 = new List<int>();
                    foreach (Player p in m_players)
                        if (!p.IsDead() && p.Id != currentPlayer.Id && p.ListCard.Count > 0)
                            target2.Add(p.Id);

                    EventView.Manager.Emit(new SelectStealCardEvent()
                    {
                        PlayerId = currentPlayer.Id,
                        PossiblePlayerTargetId = target2.ToArray()
                    });
                    break;
            }
        }
        else if (e is PowerUsedEvent powerUsed)
        {
            PlayerCardPower(m_players[powerUsed.PlayerId]);
        }
        else if (e is PowerNotUsedEvent powerNotUsed)
        {
            DontUsePower(m_players[powerNotUsed.PlayerId]);
        }
        else if (e is DrawCardEvent drawCard)
        {
            Player player = m_players[drawCard.PlayerId];

            switch (drawCard.SelectedCardType)
            {
                case CardType.Darkness:
                    Debug.Log("Le joueur " + m_players[PlayerTurn.Value].Name + " choisit de piocher une carte Ténèbres.");
                    DarknessCard darknessCard = gameBoard.DrawCard(CardType.Darkness) as DarknessCard;

                    if (darknessCard.isEquipement)
                    {
                        player.AddCard(darknessCard);
                        Debug.Log("La carte " + darknessCard.cardName + " a été ajoutée à la main du joueur "
                            + player.Name + ".");
                    }

                    DarknessCardPower(darknessCard, player.Id);

                    if (!darknessCard.isEquipement)
                        gameBoard.AddDiscard(darknessCard, CardType.Darkness);

                    break;
                case CardType.Light:

                    Debug.Log("Le joueur " + player.Name + " pioche une carte Lumière.");

                    LightCard lightCard = gameBoard.DrawCard(CardType.Light) as LightCard;

                    if (lightCard.isEquipement)
                    {
                        player.AddCard(lightCard);
                        Debug.Log("La carte " + lightCard.cardName + " a été ajoutée à la main du joueur "
                            + player.Name + ".");
                    }

                    LightCardPower(lightCard, player.Id);

                    if (!lightCard.isEquipement)
                        gameBoard.AddDiscard(lightCard, CardType.Light);

                    break;
                case CardType.Vision:

                    Debug.Log("Le joueur " + player.Name + " choisit de piocher une carte Vision.");

                    VisionCard visionCard = gameBoard.DrawCard(CardType.Vision) as VisionCard;

                    VisionCardPower(visionCard);

                    gameBoard.AddDiscard(visionCard, CardType.Vision);

                    break;
            }
        }
        else if (e is AttackEvent attack)
        {
            AttackCorrespondingPlayer(attack.PlayerId);
        }
        else if (e is AttackPlayerEvent attackPlayer)
        {
            Player playerAttacking = m_players[attackPlayer.PlayerId];
            Player playerAttacked = m_players[attackPlayer.PlayerAttackedId];

            int lancer1 = UnityEngine.Random.Range(1, 6);
            int lancer2 = UnityEngine.Random.Range(1, 4);
            int lancerTotal = (playerAttacking.HasSaber.Value == true) ? lancer2 : Mathf.Abs(lancer1 - lancer2);

            if (attackPlayer.PowerFranklin)
                lancerTotal = lancer1;
            else if (attackPlayer.PowerGeorges)
                lancerTotal = lancer2;


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
                    PlayerAttacked.Value = playerAttacked.Id;
                    PlayerCardPower(playerAttacking);
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
                    DarknessCardPower(playerStealed.ListCard[indexCard] as DarknessCard, playerStealing.Id);
                    LooseEquipmentCard(playerStealed.Id, indexCard, 0);
                }
                else if (playerStealed.ListCard[indexCard].cardType == CardType.Light)
                {
                    LightCardPower(playerStealed.ListCard[indexCard] as LightCard, playerStealing.Id);
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
                    DarknessCardPower(playerGiving.ListCard[indexCard] as DarknessCard, playerGived.Id);
                    LooseEquipmentCard(playerGiving.Id, indexCard, 0);
                }
                else if (playerGiving.ListCard[indexCard].cardType == CardType.Light)
                {
                    LightCardPower(playerGiving.ListCard[indexCard] as LightCard, playerGived.Id);
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
        else if (e is RevealOrNotEvent revealOrNot)
        {
            Player player = m_players[revealOrNot.PlayerId];
            bool hasRevealed = revealOrNot.HasRevealed;
            Card effectCard = revealOrNot.EffectCard;

            if (effectCard is DarknessCard
                && hasRevealed
                && player.Team == CharacterTeam.Shadow)
            {
                player.Healed(player.Wound.Value);
                Debug.Log("Le joueur " + player.Name + " se soigne complètement");
            }
            else if (effectCard is LightCard effectLightCard)
            {
                if (effectLightCard.lightEffect == LightEffect.Supreme
                    && hasRevealed
                    && player.Team == CharacterTeam.Hunter)
                {
                    player.Healed(player.Wound.Value);
                    Debug.Log("Le joueur " + player.Name + " se soigne complètement");
                }
                else if (effectLightCard.lightEffect == LightEffect.Chocolat
                            && hasRevealed
                            && (player.Character.characterType == CharacterType.Allie
                                || player.Character.characterType == CharacterType.Emi
                                || player.Character.characterType == CharacterType.Metamorphe))
                {
                    player.Healed(player.Wound.Value);
                    Debug.Log("Le joueur " + player.Name + " se soigne complètement");
                }
            }
            else
                Debug.Log("Rien ne se passe.");
        }
        else if (e is LightCardEffectEvent lcEffect)
        {
            Player player = m_players[lcEffect.PlayerId];
            Player playerChoosed = m_players[lcEffect.PlayerChoosenId];
            LightCard lightCard = lcEffect.LightCard;

            if (lightCard.lightEffect == LightEffect.Benediction)
            {
                Debug.Log("Vous choisissez de soigner le joueur " + playerChoosed.Name + ".");
                playerChoosed.Healed(UnityEngine.Random.Range(1, 6));
            }
            else if (lightCard.lightEffect == LightEffect.Benediction)
            {
                Debug.Log("Vous choisissez d'infliger 7 blessures au joueur " + playerChoosed.Name + ".");
                playerChoosed.SetWound(7);
            }
        }
        else if (e is VisionCardEffectEvent vcEffect)
        {

            Player playerGiving = m_players[vcEffect.PlayerId];
            Player playerGived = m_players[vcEffect.TargetId];
            VisionCard pickedCard = vcEffect.VisionCard;
            bool metaPower = vcEffect.MetamorphePower;

            Debug.Log("La carte Vision a été donnée au joueur " + playerGived.Name + ".");
            CharacterTeam team = playerGived.Team;
            /*
            if (playerGived.Character.characterType == CharacterType.Metamorphe)
            {
                // A enlever plus tard
                usePowerButton.gameObject.SetActive(true);
                dontUsePowerButton.gameObject.SetActive(true);

                playerGived.CanUsePower.Value = true;
                playerGived.CanNotUsePower.Value = true;

                m_pickedVisionCard = pickedCard;
            }
            */
            // Cartes applicables en fonction des équipes ?
            if ((team == CharacterTeam.Shadow && pickedCard.visionEffect.effectOnShadow && !metaPower)
                || (team == CharacterTeam.Hunter && pickedCard.visionEffect.effectOnHunter)
                || (team == CharacterTeam.Neutral && pickedCard.visionEffect.effectOnNeutral)
                || (team == CharacterTeam.Shadow && !pickedCard.visionEffect.effectOnShadow && metaPower))
            {
                // Cas des cartes infligeant des Blessures
                if (pickedCard.visionEffect.effectTakeWounds)
                {
                    playerGived.Wounded(pickedCard.visionEffect.nbWounds);
                    CheckPlayerDeath(playerGived.Id);
                }
                // Cas des cartes soignant des Blessures
                else if (pickedCard.visionEffect.effectHealingOneWound)
                {
                    if (playerGived.Wound.Value == 0)
                    {
                        playerGived.Wounded(1);
                        CheckPlayerDeath(playerGived.Id);
                    }
                    else
                    {
                        playerGived.Healed(1);
                        CheckPlayerDeath(playerGived.Id);
                    }
                }
                // Cas des cartes volant une carte équipement ou infligeant des Blessures
                else if (pickedCard.visionEffect.effectGivingEquipementCard)
                {
                    if (playerGived.ListCard.Count == 0)
                    {
                        Debug.Log("Vous ne possédez pas de carte équipement.");
                        playerGived.Wounded(1);
                    }
                    else
                    {
                        Debug.Log("Voulez-vous donner une carte équipement ou subir une Blessure ?");

                        EventView.Manager.Emit(new SelectGiveOrWoundEvent()
                        {
                            PlayerId = playerGived.Id
                        });
                    }
                }
                // Cas des cartes applicables en fonction des points de vie
                else if (pickedCard.visionEffect.effectOnLowHP && CheckLowHPCharacters(playerGived.Character.characterName))
                {
                    playerGived.Wounded(1);
                    CheckPlayerDeath(playerGived.Id);
                }
                else if (pickedCard.visionEffect.effectOnHighHP && CheckHighHPCharacters(playerGived.Character.characterName))
                {
                    playerGived.Wounded(2);
                    CheckPlayerDeath(playerGived.Id);
                }
                // Cas de la carte Vision Suprême
                else if (pickedCard.visionEffect.effectSupremeVision)
                    //TODO montrer la carte personnage
                    Debug.Log("C'est une carte Vision Suprême !");
            }

            else
                Debug.Log("Rien ne se passe.");
        }
        else if (e is GiveOrWoundEvent giveOrWound)
        {
            Player player = m_players[giveOrWound.PlayerId];
            bool give = giveOrWound.Give;

            if (give)
            {
                Debug.Log("Vous choisissez de donner une carte équipement.");
                GiveEquipmentCard(player.Id);
            }
            else
            {
                Debug.Log("Vous choisissez de subir 1 Blessure.");
                player.Wounded(1);
                CheckPlayerDeath(player.Id);
            }
        }
        else if (e is BobPowerEvent bobPower)
        {
            Player playerBob = m_players[bobPower.PlayerId];
            Player playerBobed = m_players[PlayerAttacked.Value];
            int bobDamages = m_damageBob;
            bool usePower = bobPower.UsePower;

            if (usePower)
            {
                StealEquipmentCard(playerBob.Id, playerBobed.Id);
            }
            else
            {
                AttackCorrespondingPlayer(playerBob.Id, playerBobed.Id, bobDamages);
            }
        }
    }

    private void DontUsePower(Player player)
    {
        throw new NotImplementedException();
    }
}
