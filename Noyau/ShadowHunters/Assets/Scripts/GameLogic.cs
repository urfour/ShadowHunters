//using System;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;
//using UnityEngine.Events;
//using EventSystem;
//using Scripts;
//using Scripts.Settings;
//using Scripts.event_in;
//using Scripts.event_out;
//using Assets.Noyau.Cards.model;
//using Assets.Noyau.Cards.controller;

///// <summary>
///// Classe représentant la logique du jeu, à savoir la gestion des règles et des interactions 
///// </summary>
//public class GameLogic : MonoBehaviour, IListener<PlayerEvent>
//{
//    /// <summary>
//    /// Carte vision donné au métamorphe
//    /// </summary>
//    private VisionCard m_pickedVisionCard;
//    /// <summary>
//    /// Propriété d'accès à l'id du joueur attaqué
//    /// </summary>
//    public Setting<int> PlayerAttacked { get; private set; } = new Setting<int>(-1);
//    /// <summary>
//    /// Dégats pris par le joueur attaqué par Bob
//    /// </summary>    
//    private int m_damageBob = -1;
//    /// <summary>
//    /// Pouvoir de Emi/Franck/Georges possible
//    /// </summary>    
//    private bool powerEFG = false;
//    /// <summary>
//    /// Propriété d'accès à l'id du joueur dont c'est le tour
//    /// </summary>
//    public Setting<int> PlayerTurn { get; private set; } = new Setting<int>(-1);
//    /// <summary>
//    /// Booléen représentant l'état actuel du jeu (terminé ou non)
//    /// </summary>
//    private bool m_isGameOver = false;
//    /// <summary>
//    /// Propriété d'accès à l'état actuel du jeu
//    /// </summary>
//    public bool IsGameOver
//    {
//        get => m_isGameOver;
//        private set => m_isGameOver = value;
//    }
//    /// <summary>
//    /// Liste comportant les informations de tous les joueurs
//    /// </summary>
//    private List<Player> m_players;
//    /// <summary>
//    /// Plateau du jeu comportant les différentes cartes et leur position
//    /// </summary>
//    private GameBoard gameBoard;
//    /// <summary>
//    /// Propriété d'accès au plateau du jeu
//    /// </summary>
//    public GameBoard GameBoard { get; }

//    // Cartes possibles des différents decks
//    /// <summary>
//    /// Liste des différentes cartes Vision
//    /// </summary>
//    public List<VisionCard> m_visionCards;
//    /// <summary>
//    /// Liste des différentes cartes Ténèbre
//    /// </summary>
//    public List<DarknessCard> m_darknessCards;
//    /// <summary>
//    /// Liste des différentes cartes Lumière
//    /// </summary>
//    public List<LightCard> m_lightCards;
//    /// <summary>
//    /// Liste des différentes cartes Lieu
//    /// </summary>
//    public List<LocationCard> m_locationCards;
//    /// <summary>
//    /// Liste des différents personnages Hunter
//    /// </summary>
//    public List<Character> m_hunterCharacters;
//    /// <summary>
//    /// Liste des différents personnages Shadow
//    /// </summary>
//    public List<Character> m_shadowCharacters;
//    /// <summary>
//    /// Liste des différents personnages Neutre
//    /// </summary>
//    public List<Character> m_neutralCharacters;


//    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
//    public static void OnBeforeSceneLoadRuntimeMethod()
//    {
//        EventView.Load();
//    }
//    /*
//    /// <summary>
//    /// Fonction appelée dès l'instanciation du GameObject auquel est lié le script,
//    /// permettant de préparer le jeu.
//    /// </summary>
//    void Start()
//    {
//        instance = this;

//        EventView.Manager.AddListener(this, true);

//        KernelUI kui = gameObject.AddComponent<KernelUI>();

//        EventView.Manager.AddListener(kui, true);

//        Debug.Log("double addlistener");

//        const int NB_PLAYERS = 5;
//        PrepareGame(NB_PLAYERS);
       
//        //InitInterface();
//        //ChooseNextPlayer();
//    }
//    */

//    /// <summary>
//    /// Redistribution des cartes lorsqu'une des pioches est vide
//    /// </summary>
//    /// <param name="oldDeck">Pile de défausse</param>
//    /// <param name="newDeck">Pioche à refaire</param>
//    void ResetDecks(List<Card> oldDeck, List<Card> newDeck)
//    {
//        Debug.Log("Le deck est vide, redistribution des cartes.");
//        for (int i = 0; i < oldDeck.Count; i++)
//        {
//            newDeck.Add(oldDeck[i]);
//            oldDeck.RemoveAt(i);
//        }
//        newDeck.Shuffle<Card>();
//    }

//    /// <summary>
//    /// Perte d'une carte équipement pour un joueur
//    /// </summary>
//    /// <param name="PlayerId">Id du joueur</param>
//    /// <param name="CardId">Id de la carte</param>
//    /// <returns></returns>
//    void LooseEquipmentCard(int PlayerId, int CardId)
//    {
//        CharacterTeam team = m_players[PlayerId].Character.team;
//        string character = m_players[PlayerId].Character.characterName;
//        bool revealed = m_players[PlayerId].Revealed.Value;

//        m_players[PlayerId].PrintCards();

//        // Si l'index de la carte correspond au nombre de cartes du joueur
//        if (m_players[PlayerId].ListCard.Count <= CardId)
//        {
//            Debug.LogError("Erreur : l'index de la carte ne correspond pas à la liste de cartes du joueur.");
//            return;
//        }

//        Card card = m_players[PlayerId].ListCard[CardId];

//        // Si la carte est un equimement
//        if (!card is IEquipment)
//        {
//            Debug.LogError("Erreur : la carte n'est pas un equipement.");
//            return;
//        }

//        // Si la carte est une carte ténèbre
//        if (card.cardType == CardType.Darkness)
//        {
//            DarknessCard dcard = card as DarknessCard;
//            DarknessEffect effect = dcard.darknessEffect;

//            switch (effect)
//            {
//                case DarknessEffect.Mitrailleuse:
//                    m_players[PlayerId].HasGatling.Value = false;
//                    break;

//                case DarknessEffect.Sabre:
//                    m_players[PlayerId].HasSaber.Value = false;
//                    break;

//                case DarknessEffect.Hache:
//                    m_players[PlayerId].BonusAttack.Value--;
//                    break;

//                case DarknessEffect.Revolver:
//                    m_players[PlayerId].HasRevolver.Value = false;
//                    break;
//            }
//        }
//        // Si la carte est une carte lumière
//        else if (card.cardType == CardType.Light)
//        {
//            LightCard lcard = card as LightCard;
//            LightEffect effect = lcard.lightEffect;

//            switch (effect)
//            {
//                case LightEffect.Lance:
//                    m_players[PlayerId].HasSpear.Value = false;
//                    if (team == CharacterTeam.Hunter && revealed)
//                    {
//                        m_players[PlayerId].BonusAttack.Value -= 2;
//                    }
//                    break;

//                case LightEffect.Boussole:
//                    m_players[PlayerId].HasCompass.Value = false;
//                    break;

//                case LightEffect.Broche:
//                    m_players[PlayerId].HasBroche.Value = false;
//                    break;

//                case LightEffect.Toge:
//                    m_players[PlayerId].HasToge.Value = false;
//                    m_players[PlayerId].MalusAttack.Value--;
//                    m_players[PlayerId].ReductionWounds.Value = 0;
//                    break;

//                case LightEffect.Crucifix:
//                    m_players[PlayerId].HasCrucifix.Value = false;
//                    break;

//                case LightEffect.Amulette:
//                    m_players[PlayerId].HasAmulet.Value = false;
//                    break;
//            }
//        }
//        else
//            Debug.LogError("Erreur : type en paramètre invalide.");


//        Debug.Log("Defausse de : " + card.cardName);
//        Debug.Log("Effet : " + card.description);

//        m_players[PlayerId].RemoveCard(CardId);

//        m_players[PlayerId].PrintCards();
//    }


//    /// <summary>
//    /// Récupération des joueurs se trouvant dans le même secteur qu'un
//    /// autre joueur
//    /// </summary>
//    /// <param name="playerId">Id du joueur</param>
//    /// <param name="hasRevolver">Booléen représentant la possesion du 
//    /// Revolver</param>
//    /// <returns>Liste des joueurs se trouvant dans le même secteur</returns>
//    public List<Player> GetPlayersSameSector(int playerId, bool hasRevolver)
//    {
//        int positionIndex = gameBoard.GetIndexOfPosition(m_players[playerId].Position);
//        int positionOtherPlayer;
//        List<Player> players = new List<Player>();
//        foreach (Player player in m_players)
//        {
//            if (!player.Dead.Value && player.Id != playerId && player.Position != Position.None)
//            {
//                positionOtherPlayer = gameBoard.GetIndexOfPosition(player.Position);
//                if ((positionIndex % 2 == 0 
//                    && (positionOtherPlayer == positionIndex 
//                        || positionOtherPlayer == positionIndex + 1))
//                    || (positionIndex % 2 == 1 
//                        && (positionOtherPlayer == positionIndex 
//                            || positionOtherPlayer == positionIndex - 1)))

//                    if (!hasRevolver)
//                        players.Add(player);
//                    else if (hasRevolver)
//                        players.Add(player);
//            }
//        }

//        return players;
//    }

//    /// <summary>
//    /// Activation de l'effet d'une carte Ténèbre piochée
//    /// </summary>
//    /// <param name="pickedCard">Carte Ténèbre piochée</param>
//    /// <param name="idPlayer">Joueur qui bénéficie de l'effet</param>
//    /// <returns>Itération terminée</returns>
//    void DarknessCardPower(DarknessCard pickedCard, int idPlayer)
//    {
//        switch (pickedCard.darknessEffect)
//        {
//            case DarknessEffect.Araignee:
//                TakingWoundsEffect(idPlayer, false, 2, -2);
//                break;
//            case DarknessEffect.Banane:
//                bool hasEquip = false;

//                foreach (Card card in m_players[idPlayer].ListCard)
//                    if (card is IEquipment)
//                        hasEquip = true;

//                if (hasEquip)
//                    GiveEquipmentCard(idPlayer);
//                else
//                {
//                    m_players[idPlayer].Wounded(1,m_players[PlayerTurn],false);
//                }
//                break;
//            case DarknessEffect.ChauveSouris:
//                TakingWoundsEffect(idPlayer, false, 2, 1);
//                break;
//            case DarknessEffect.Dynamite:
//                int lancer1 = UnityEngine.Random.Range(1, 6);
//                int lancer2 = UnityEngine.Random.Range(1, 4);
//                int lancerTotal = lancer1 + lancer2;
//                Position area = Position.None;

//                switch (lancerTotal)
//                {
//                    case 2:
//                    case 3:
//                        area = Position.Antre;
//                        break;
//                    case 4:
//                    case 5:
//                        area = Position.Porte;
//                        break;
//                    case 6:
//                        area = Position.Monastere;
//                        break;
//                    case 7:
//                        Debug.Log("Rien ne se passe");
//                        break;
//                    case 8:
//                        area = Position.Cimetiere;
//                        break;
//                    case 9:
//                        area = Position.Foret;
//                        break;
//                    case 10:
//                        area = Position.Sanctuaire;
//                        break;
//                }
//                if (lancerTotal != 7)
//                {
//                    foreach (Player p in m_players)
//                    {
//                        if (p.Position == area && !p.HasAmulet.Value)
//                            p.Wounded(3,m_players[PlayerTurn],false);
//                    }
//                }
//                break;
//            case DarknessEffect.Hache:
//                m_players[idPlayer].BonusAttack.Value++;
//                break;
//            case DarknessEffect.Mitrailleuse:
//                m_players[idPlayer].HasGatling.Value = true;
//                break;
//            case DarknessEffect.Poupee:
//                TakingWoundsEffect(idPlayer, true, 3, 0);
//                break;
//            case DarknessEffect.Revolver:
//                m_players[idPlayer].HasRevolver.Value = true;
//                break;
//            case DarknessEffect.Rituel:
//                Debug.Log("Voulez-vous vous révéler ? Vous avez 6 secondes, sinon la carte se défausse.");

//                if (m_players[idPlayer].Revealed.Value && m_players[idPlayer].Character.team == CharacterTeam.Shadow)
//                {
//                    m_players[idPlayer].Healed(m_players[idPlayer].Wound.Value);
//                    Debug.Log("Le joueur " + m_players[idPlayer].Name + " se soigne complètement");
//                }
//                else
//                {
//                    EventView.Manager.Emit(new SelectRevealOrNotEvent()
//                    {
//                        PlayerId = idPlayer,
//                        EffectCard = pickedCard
//                    });
//                }
//                break;
//            case DarknessEffect.Sabre:
//                m_players[idPlayer].HasSaber.Value = true;
//                break;
//            case DarknessEffect.Succube:
//                StealEquipmentCard(idPlayer);
//                break;
//        }
//    }

//    /// <summary>
//    /// Activation de l'effet d'une carte Lumière piochée
//    /// </summary>
//    /// <param name="pickedCard">Carte Lumière piochée</param>
//    /// <param name="idPlayer">Joueur qui bénéficie de l'effet</param>
//    /// <returns>Itération terminée</returns>
//    void LightCardPower(LightCard pickedCard, int idPlayer)
//    {
//        CharacterTeam team = m_players[idPlayer].Character.team;
//        String character = m_players[idPlayer].Character.characterName;

//        bool revealed = m_players[idPlayer].Revealed.Value;

//        Debug.Log(pickedCard.lightEffect);

//        switch (pickedCard.lightEffect)
//        {
//            case LightEffect.Amulette:
//                m_players[idPlayer].HasAmulet.Value = true;
//                break;

//            case LightEffect.AngeGardien:

//                m_players[idPlayer].HasGuardian.Value = true;
//                break;

//            case LightEffect.Supreme:
//                Debug.Log("Voulez-vous vous révéler ? Vous avez 6 secondes, sinon la carte se défausse.");

//                if (m_players[idPlayer].Revealed.Value && m_players[idPlayer].Character.team == CharacterTeam.Hunter)
//                {
//                    m_players[idPlayer].Healed(m_players[idPlayer].Wound.Value);
//                    Debug.Log("Le joueur " + m_players[idPlayer].Name + " se soigne complètement");
//                }
//                else
//                {
//                    EventView.Manager.Emit(new SelectRevealOrNotEvent()
//                    {
//                        PlayerId = idPlayer,
//                        EffectCard = pickedCard
//                    });
//                }
//                break;

//            case LightEffect.Chocolat:
//                Debug.Log("Voulez-vous vous révéler ? Vous avez 6 secondes, sinon la carte se défausse.");

//                if (m_players[idPlayer].Revealed.Value
//                    && (character == "Allie"
//                        || character == "Emi"
//                        || character == "Metamorphe"))
//                {
//                    m_players[idPlayer].Healed(m_players[idPlayer].Wound.Value);
//                    Debug.Log("Le joueur " + m_players[idPlayer].Name + " se soigne complètement");
//                }
//                else
//                {
//                    EventView.Manager.Emit(new SelectRevealOrNotEvent()
//                    {
//                        PlayerId = idPlayer,
//                        EffectCard = pickedCard
//                    });
//                }
//                break;

//            case LightEffect.Benediction:

//                Debug.Log("Qui souhaitez-vous soigner ?");
//                List<int> players = new List<int>();
//                foreach (Player player in m_players)
//                {
//                    if (!player.Dead.Value && player.Id != idPlayer)
//                    {
//                        players.Add(player.Id);
//                    }
//                }

//                EventView.Manager.Emit(new SelectLightCardTargetEvent()
//                {
//                    PlayerId = idPlayer,
//                    PossibleTargetId = players.ToArray(),
//                    LightCard = pickedCard
//                });

//                break;

//            case LightEffect.Boussole:
//                m_players[idPlayer].HasCompass.Value = true;
//                break;

//            case LightEffect.Broche:
//                m_players[idPlayer].HasBroche.Value = true;
//                break;

//            case LightEffect.Crucifix:
//                m_players[idPlayer].HasCrucifix.Value = true;
//                break;

//            case LightEffect.EauBenite:

//                m_players[idPlayer].Healed(2);
//                break;

//            case LightEffect.Eclair:

//                foreach (Player p in m_players)
//                {
//                    if (p.Id != idPlayer)
//                        p.Wounded(2,m_players[PlayerTurn],false);
//                }
//                break;

//            case LightEffect.Lance:
//                m_players[idPlayer].HasSpear.Value = true;
//                if (team == CharacterTeam.Hunter && revealed)
//                {
//                    m_players[idPlayer].BonusAttack.Value += 2;
//                }
//                break;

//            case LightEffect.Miroir:

//                if (!revealed && team == CharacterTeam.Shadow && character != "Metamorphe")
//                {
//                    RevealCard(m_players[idPlayer]);
//                    Debug.Log("Vous révélez votre rôle à tous, vous êtes : " + character);
//                }
//                break;

//            case LightEffect.PremiersSecours:

//                Debug.Log("Qui souhaitez-vous placer à exactement 7 Blessures ?");

//                List<int> players2 = new List<int>();
//                foreach (Player player in m_players)
//                {
//                    if (!player.Dead.Value)
//                    {
//                        players2.Add(player.Id);
//                    }
//                }

//                EventView.Manager.Emit(new SelectLightCardTargetEvent()
//                {
//                    PlayerId = idPlayer,
//                    PossibleTargetId = players2.ToArray(),
//                    LightCard = pickedCard
//                });

//                break;

//            case LightEffect.Savoir:

//                m_players[idPlayer].HasAncestral.Value = true;
//                break;

//            case LightEffect.Toge:

//                m_players[idPlayer].HasToge.Value = true;
//                m_players[idPlayer].MalusAttack.Value++;
//                m_players[idPlayer].ReductionWounds.Value = 1;
//                break;
//        }
//    }

//    /// <summary>
//    /// Activation de l'effet d'une carte Vision piochée
//    /// </summary>
//    /// <remarks>
//    /// Le déroulement suit le procédé suivant : le jeu va afficher au joueur
//    /// la liste des joueurs à qui il peut donner la carte vision (donc tous
//    /// les joueurs vivants sauf lui-même) et va attendre que le joueur
//    /// choisisse la personne à qui la donner. Dès lors, l'effet va s'activer
//    /// (ou non) automatiquement en fonction du type de la carte Vision.
//    /// Metamorphe, quant à lui, aura la possibilité de choisir
//    /// s'il souhaite déclencher la carte ou non
//    /// </remarks>
//    /// <param name="pickedCard">Carte Vision piochée</param>
//    /// <returns>Itération terminée</returns>
//    void VisionCardPower(VisionCard pickedCard, int idPlayer)
//    {
//        Debug.Log("Message au joueur " + m_players[idPlayer].Name + " : ");
//        Debug.Log("Carte Vision piochée : " + pickedCard.cardName);
//        Debug.Log(pickedCard.description);

//        List<int> players = new List<int>();
//        for (int i = 0; i < m_players.Count; i++)
//            if (!m_players[i].Dead.Value && i != idPlayer)
//                players.Add(m_players[i].Id);

//        EventView.Manager.Emit(new SelectVisionPowerEvent()
//        {
//            PlayerId = idPlayer,
//            PossiblePlayerTargetId = players.ToArray(),
//            VisionCard = pickedCard
//        });
//    }

//    /// <summary>
//    /// Vérifie si un personnage est un personnage avec 11 PDV ou moins
//    /// </summary>
//    /// <param name="characterName">Nom du personnage</param>
//    /// <returns>Booléen représentant le type du personnage</returns>
//    bool CheckLowHPCharacters(string characterName)
//    {
//        return characterName.StartsWith("A") || characterName.StartsWith("B")
//            || characterName.StartsWith("C") || characterName.StartsWith("E")
//            || characterName.StartsWith("M");
//    }

//    /// <summary>
//    /// Vérifie si un personnage est un personnage avec 12 PDV ou plus
//    /// </summary>
//    /// <param name="characterName">Nom du personnage</param>
//    /// <returns>Booléen représentant le type du personnage</returns>
//    bool CheckHighHPCharacters(string characterName)
//    {
//        return characterName.StartsWith("D") || characterName.StartsWith("F")
//            || characterName.StartsWith("G") || characterName.StartsWith("L")
//            || characterName.StartsWith("V");
//    }


//    /// <summary>
//    /// Fonction permettant de révéler son identité
//    /// </summary>
//    public void RevealCard(Player p)
//    {
//        p.Revealed.Value = true;
//        Debug.Log("Le joueur " + p.Name + " s'est révélé, il s'agissait de : "
//            + p.Character.characterName + " ! Il est dans l'équipe des "
//            + p.Character.team + ".");

//        if (p.HasSpear.Value == true && p.Character.team == CharacterTeam.Hunter)
//        {
//            p.BonusAttack.Value += 2;
//            Debug.Log("Le pouvoir de la lance s'active !");
//        }

//        // Si le joueur est Allie, il peut utiliser son pouvoir à tout moment
//        // Si le joueur est Emi, Franklin ou Georges et qu'il est au début de son tour, il peut utiliser son pouvoir
//        if (p.Character.characterName == "Allie"
//            || (powerEFG
//                && (p.Character.characterName == "Emi"
//                    || p.Character.characterName == "Franklin"
//                    || p.Character.characterName == "Georges")))
//        {
//            p.CanUsePower.Value = true;
//        }
//    }


//    /// <summary>
//    /// Fonction du choix d'un joueur à attaquer
//    /// </summary>
//    /// <param name="playerAttackingId">Id du joueur attaquant</param>
//    /// <returns>Iteration terminée</returns>
//    void AttackCorrespondingPlayer(int playerAttackingId)
//    {
//        List<Player> players = GetPlayersSameSector(playerAttackingId, m_players[playerAttackingId].HasRevolver.Value);

//        if (players.Count != 0)
//        {
//            if (!m_players[playerAttackingId].HasGatling.Value)
//            {
//                List<int> playersId = new List<int>();
//                foreach (Player player in players)
//                    playersId.Add(player.Id);

//                EventView.Manager.Emit(new SelectAttackTargetEvent()
//                {
//                    PlayerId = playerAttackingId,
//                    PossibleTargetId = playersId.ToArray(),
//                    PowerFranklin = false,
//                    PowerGeorges = false
//                });
//            }
//            else
//            {
//                int lancer1 = UnityEngine.Random.Range(1, 6);
//                int lancer2 = UnityEngine.Random.Range(1, 4);
//                int lancerTotal = (m_players[playerAttackingId].HasSaber.Value == true) ? lancer2 : Mathf.Abs(lancer1 - lancer2);
//                if (lancerTotal == 0)
//                    Debug.Log("Le lancer vaut 0, vous n'attaquez pas.");
//                else
//                {
//                    foreach (Player player in players)
//                    {
//                        if (m_players[playerAttackingId].Character.characterName == "Bob"
//                            && m_players[playerAttackingId].Revealed.Value
//                            && lancerTotal >= 2)
//                        {
//                            m_damageBob = lancerTotal + m_players[playerAttackingId].BonusAttack.Value - m_players[playerAttackingId].MalusAttack.Value;
//                            PlayerAttacked.Value = player.Id;
//                            PlayerCardPower(m_players[playerAttackingId]);
//                        }
//                        else
//                        {
//                            m_players[player.Id].Wounded(lancerTotal + m_players[playerAttackingId].BonusAttack.Value - m_players[playerAttackingId].MalusAttack.Value,m_players[PlayerTurn],true);
                            
//                            // Le Vampire se soigne 2 blessures s'il est révélé et s'il a infligé des dégats
//                            if (m_players[playerAttackingId].Character.characterName == "Vampire"
//                                && m_players[playerAttackingId].Revealed.Value
//                                && lancerTotal + m_players[playerAttackingId].BonusAttack.Value - m_players[playerAttackingId].MalusAttack.Value > 0)
//                                PlayerCardPower(m_players[playerAttackingId]);

//                            // Le Loup-garou peut contre attaquer
//                            if (m_players[player.Id].Character.characterName == "LoupGarou"
//                                && m_players[player.Id].Revealed.Value)
//                            {
//                                m_players[player.Id].CanUsePower.Value = true;
//                            }
//                        }
//                    }
//                }
//            }
//        }
//        else
//        {
//            Debug.Log("Vous ne pouvez attaquer aucun joueur.");
//        }
//    }

//    /// <summary>
//    /// Fonction du choix d'un joueur à attaquer
//    /// </summary>
//    /// <param name="playerAttackingId">Id du joueur attaquant</param>
//    /// <param name="targetId">Id du joueur attaqué</param>
//	/// <param name="damage"> Dommage précedemment occasioné</param>
//    /// <returns>Iteration terminée</returns>
//    void AttackCorrespondingPlayer(int playerAttackingId, int targetId, int damage)
//    {
//        if (damage > 0)
//        {
//            m_players[targetId].Wounded(damage,m_players[playerAttackingId],true);

//            if (m_players[targetId].Character.characterName == "LoupGarou"
//                            && m_players[targetId].Revealed.Value)
//                m_players[targetId].CanUsePower.Value = true;
//        }
//        else
//        {
//            List<Player> players = GetPlayersSameSector(playerAttackingId, m_players[playerAttackingId].HasRevolver.Value);
//            if (players.Count == 0)
//            {
//                Debug.Log("Vous ne pouvez attaquer aucun joueur.");
//            }
//            else
//            {
//                int lancer1 = UnityEngine.Random.Range(1, 6);
//                int lancer2 = UnityEngine.Random.Range(1, 4);
//                int lancerTotal = (m_players[playerAttackingId].HasSaber.Value == true) ? lancer2 : Mathf.Abs(lancer1 - lancer2);
//                if (lancerTotal == 0)
//                {
//                    Debug.Log("Le lancer vaut 0, vous n'attaquez pas.");
//                }
//                else
//                {
//                    if (m_players[playerAttackingId].HasGatling.Value)
//                    {
//                        foreach (Player player in players)
//                        {
//                            if (m_players[playerAttackingId].Character.characterName == "Bob"
//                                && m_players[playerAttackingId].Revealed.Value
//                                && lancerTotal >= 2)
//                            {
//                                m_damageBob = lancerTotal + m_players[playerAttackingId].BonusAttack.Value - m_players[playerAttackingId].MalusAttack.Value;
//                                PlayerAttacked.Value = player.Id;
//                                PlayerCardPower(m_players[playerAttackingId]);
//                            }
//                            else
//                            {
//                                m_players[player.Id].Wounded(lancerTotal + m_players[playerAttackingId].BonusAttack.Value - m_players[playerAttackingId].MalusAttack.Value,m_players[playerAttackingId],true);

//                                // Le Loup-garou peut contre attaquer
//                                if (player.Character.characterName == "LoupGarou"
//                                    && player.Revealed.Value)
//                                    player.CanUsePower.Value = true;

//                                // Le Vampire se soigne 2 blessures s'il est révélé et s'il a infligé des dégats
//                                if (m_players[playerAttackingId].Character.characterName == "Vampire"
//                                    && m_players[playerAttackingId].Revealed.Value
//                                    && lancerTotal + m_players[playerAttackingId].BonusAttack.Value - m_players[playerAttackingId].MalusAttack.Value > 0)
//                                    PlayerCardPower(m_players[playerAttackingId]);
//                            }
//                        }
//                    }
//                    else
//                    {
//                        if (m_players[playerAttackingId].Character.characterName == "Bob"
//                            && m_players[playerAttackingId].Revealed.Value
//                            && lancerTotal >= 2)
//                        {
//                            m_damageBob = lancerTotal + m_players[playerAttackingId].BonusAttack.Value - m_players[playerAttackingId].MalusAttack.Value;
//                            PlayerAttacked.Value = targetId;
//                            PlayerCardPower(m_players[playerAttackingId]);
//                        }
//                        else
//                        {
//                            Debug.Log("Vous choisissez d'attaquer le joueur " + m_players[targetId].Name + ".");

//                            m_players[targetId].Wounded(lancerTotal + m_players[playerAttackingId].BonusAttack.Value - m_players[playerAttackingId].MalusAttack.Value,m_players[playerAttackingId],true);

//                            // Le Loup-garou peut contre attaquer
//                            if (m_players[targetId].Character.characterName == "LoupGarou"
//                                && m_players[targetId].Revealed.Value)
//                                m_players[targetId].CanUsePower.Value = true;

//                            // Le Vampire se soigne 2 blessures s'il est révélé et s'il a infligé des dégats
//                            if (m_players[playerAttackingId].Character.characterName == "Vampire"
//                                && m_players[playerAttackingId].Revealed.Value
//                                && lancerTotal + m_players[playerAttackingId].BonusAttack.Value - m_players[playerAttackingId].MalusAttack.Value > 0)
//                                PlayerCardPower(m_players[playerAttackingId]);
//                        }
//                    }
//                }
//            }
//        }
//    }

//    /// <summary>
//    /// Choix du joueur à qui voler une carte équipement
//    /// </summary>
//    /// <param name="thiefId">Id du joueur voleur</param>
//    /// <returns>Itération terminée</returns>
//    void StealEquipmentCard(int thiefId)
//    {
//        // On peut seulement choisir les joueurs vivants qui ont au moins une carte equipement
//        List<int> choices = new List<int>();
//        foreach (Player player in m_players)
//            if (!player.Dead.Value && player.Id != thiefId && player.ListCard.Count > 0)
//                foreach (Card card in player.ListCard)
//                    if (card is IEquipment)
//                    {
//                        choices.Add(player.Id);
//                        break;
//                    }

//        if (choices.Count != 0)
//        {
//            EventView.Manager.Emit(new SelectStealCardEvent()
//            {
//                PlayerId = thiefId,
//                PossiblePlayerTargetId = choices.ToArray()
//            });
//        }
//        else
//        {
//            Debug.Log("Il n'y a aucun joueur à qui voler une carte équipement.");
//        }
//    }

//    /// <summary>
//    /// Vol d'une carte équipement à un joueur précis
//    /// </summary>
//    /// <param name="thiefId">Id du joueur voleur</param>
//    /// <param name="playerId">Id du joueur à qui voler une carte</param>
//    /// <returns></returns>
//    void StealEquipmentCard(int thiefId, int playerId)
//    {
//        bool hasEquip = false;
//        foreach (Card card in m_players[playerId].ListCard)
//            if (card is IEquipment)
//                hasEquip = true;

//        if (!hasEquip)
//        {
//            Debug.LogError("Erreur : Le joueur choisi n'a pas de cartes équipement.");
//            return;
//        }

//        EventView.Manager.Emit(new SelectStealCardFromPlayerEvent()
//        {
//            PlayerId = thiefId,
//            PlayerStealedId = playerId
//        });
//    }

//    /// <summary>
//    /// Choix d'une carte équipement à donner et du joueur à qui la donner
//    /// </summary>
//    /// <param name="giverPlayerId">Id du joueur donneur</param>
//    /// <returns>Itération terminée</returns>
//    void GiveEquipmentCard(int giverPlayerId)
//    {
//        if (m_players[giverPlayerId].ListCard.Count == 0)
//        {
//            Debug.LogError("Vous ne possédez aucune carte equipement.");
//            return;
//        }

//        List<int> choices = new List<int>();
//        foreach (Player player in m_players)
//            if (!player.Dead.Value && player.Id != giverPlayerId)
//                choices.Add(player.Id);

//        if (choices.Count == 0)
//        {
//            Debug.LogError("Erreur : Vous ne pouvez donner de cartes equipements à personne");
//            return;
//        }

//        EventView.Manager.Emit(new SelectGiveCardEvent()
//        {
//            PlayerId = giverPlayerId,
//            PossibleTargetId = choices.ToArray()
//        });
//    }

//    /// <summary>
//    /// Choix du joueur à qui infliger des Blessures
//    /// </summary>
//    /// <param name="isPuppet">Booléen représentant si l'effet est issu de la
//    /// carte Poupée sanguinaire ou non</param>
//    /// <param name="nbWoundsTaken">Nombre de Blessures à infliger</param>
//    /// <param name="nbWoundsSelfHealed">Nombre de Blessures éventuellement soignées</param>
//    /// <returns>Itération terminée</returns>
//    void TakingWoundsEffect(int idPlayer, bool isPuppet, int nbWoundsTaken, int nbWoundsSelfHealed)
//    {
//        List<int> players = new List<int>();
//        foreach (Player player in m_players)
//        {
//            if (!player.Dead.Value && player.Id != idPlayer)
//            {
//                if (isPuppet)
//                    players.Add(player.Id);
//                else
//                {
//                    if (!player.HasAmulet.Value)
//                        players.Add(player.Id);
//                }
//            }
//        }

//        EventView.Manager.Emit(new SelectPlayerTakingWoundsEvent()
//        {
//            PlayerId = idPlayer,
//            PossibleTargetId = players.ToArray(),
//            IsPuppet = true,
//            NbWoundsTaken = nbWoundsTaken,
//            NbWoundsSelfHealed = nbWoundsSelfHealed
//        });
//    }

//    /// <summary>
//    /// Activation de l'effet d'une carte Personnage
//    /// </summary>
//    /// <param name="player">Joueur utilisant l'effet de sa carte 
//    /// Personnage</param>
//    void PlayerCardPower(Player player)
//    {
//        m_players[player.Id].CanUsePower.Value = false;
//        m_players[player.Id].CanNotUsePower.Value = false;

//        switch (player.Character.characterName)
//        {
//            case "Allie":
//                // Il faut que le joueur se soit révélé et qu'il n'ait pas encore utilisé son pouvoir
//                if (player.Revealed.Value && !player.UsedPower.Value)
//                {
//                    // Le joueur se soigne de toutes ses blessures
//                    player.Healed(player.Wound.Value);
//                }
//                break;
//            case "Emi":
//                // On cherche l'index de la carte Lieu dans la liste des lieux
//                int indexEmi = gameBoard.GetIndexOfPosition(player.Position);

//                if (indexEmi == -1)
//                {
//                    List<Position> position = new List<Position>();

//                    if (player.HasCompass.Value)
//                    {
//                        int lancer01 = UnityEngine.Random.Range(1, 6);
//                        int lancer02 = UnityEngine.Random.Range(1, 4);
//                        int lancer11 = UnityEngine.Random.Range(1, 6);
//                        int lancer12 = UnityEngine.Random.Range(1, 4);

//                        EventView.Manager.Emit(new SelectDiceThrow()
//                        {
//                            PlayerId = player.Id,
//                            D6Dice1 = lancer01,
//                            D4Dice1 = lancer02,
//                            D6Dice2 = lancer11,
//                            D4Dice2 = lancer12,
//                        });
//                    }
//                    else
//                    {
//                        int lancer01 = UnityEngine.Random.Range(1, 6);
//                        int lancer02 = UnityEngine.Random.Range(1, 4);

//                        while (position.Count >= 0)
//                        {
//                            lancer01 = UnityEngine.Random.Range(1, 6);
//                            lancer02 = UnityEngine.Random.Range(1, 4);

//                            switch (lancer01 + lancer02)
//                            {
//                                case 2:
//                                case 3:
//                                    position.Add(Position.Antre);
//                                    break;
//                                case 4:
//                                case 5:
//                                    position.Add(Position.Porte);
//                                    break;
//                                case 6:
//                                    position.Add(Position.Monastere);
//                                    break;
//                                case 7:
//                                    position.Add(Position.Antre);
//                                    position.Add(Position.Porte);
//                                    position.Add(Position.Monastere);
//                                    position.Add(Position.Cimetiere);
//                                    position.Add(Position.Foret);
//                                    position.Add(Position.Foret);
//                                    break;
//                                case 8:
//                                    position.Add(Position.Cimetiere);
//                                    break;
//                                case 9:
//                                    position.Add(Position.Foret);
//                                    break;
//                                case 10:
//                                    position.Add(Position.Sanctuaire);
//                                    break;
//                            }
//                            if (position.Contains(player.Position))
//                            {
//                                player.Position = position[0];
//                            }
//                            else
//                                position.Remove(player.Position);

//                        }
//                        EventView.Manager.Emit(new SelectMovement()
//                        {
//                            PlayerId = player.Id,
//                            D6Dice = lancer01,
//                            D4Dice = lancer02,
//                            LocationAvailable = position.ToArray()
//                        });
//                    }
//                }
//                else
//                {
//                    List<Position> position = new List<Position>();

//                    position.Add(gameBoard.GetAreaAt((indexEmi - 1) % 6).area);
//                    position.Add(gameBoard.GetAreaAt((indexEmi + 1) % 6).area);

//                    EventView.Manager.Emit(new SelectMovement()
//                    {
//                        PlayerId = player.Id,
//                        D6Dice = UnityEngine.Random.Range(1, 6),
//                        D4Dice = UnityEngine.Random.Range(1, 4),
//                        LocationAvailable = position.ToArray()
//                    });
//                }

//                break;
//            case "Metamorphe":
//                break;
//            case "Bob":
//                // Il faut que le joueur se soit révélé et qu'il n'ait pas encore utilisé son pouvoir
//                if (player.Revealed.Value)
//                {
//                    EventView.Manager.Emit(new SelectBobPowerEvent()
//                    {
//                        PlayerId = player.Id
//                    });
//                }
//                break;
//            case "Franklin":
//                if (player.Revealed.Value && !player.UsedPower.Value)
//                {
//                    List<int> playersId = new List<int>();
//                    foreach (Player playerT in m_players)
//                        if (playerT.Id != player.Id)
//                            playersId.Add(player.Id);

//                    player.UsedPower.Value = false;

//                    EventView.Manager.Emit(new SelectAttackTargetEvent()
//                    {
//                        PlayerId = player.Id,
//                        PossibleTargetId = playersId.ToArray(),
//                        PowerFranklin = true,
//                        PowerGeorges = false
//                    });
//                }
//                break;
//            case "Georges":
//                if (player.Revealed.Value && !player.UsedPower.Value)
//                {
//                    List<int> playersId = new List<int>();
//                    foreach (Player playerT in m_players)
//                        if (playerT.Id != player.Id)
//                            playersId.Add(player.Id);

//                    player.UsedPower.Value = false;

//                    EventView.Manager.Emit(new SelectAttackTargetEvent()
//                    {
//                        PlayerId = player.Id,
//                        PossibleTargetId = playersId.ToArray(),
//                        PowerFranklin = false,
//                        PowerGeorges = true
//                    });
//                }
//                break;
//            case "LoupGarou":
//                if (player.Revealed.Value)
//                    AttackCorrespondingPlayer(player.Id, PlayerTurn.Value, 0);
//                break;
//            case "Vampire":
//                if (player.Revealed.Value)
//                    player.Healed(2);
//                break;
//            case "Charles":
//                if (player.Revealed.Value)
//                    player.Wounded(2,player,false);
//                AttackCorrespondingPlayer(player.Id, PlayerAttacked.Value, 0);
//                break;
//            case "Daniel":
//                // Il faut que le joueur se soit révélé et qu'il n'ait pas encore utilisé son pouvoir
//                if (!player.Revealed.Value)
//                    RevealCard(player);
//                break;
//        }
//    }

//    /*
//    /// <summary>
//    //    /// Test de victoire d'un joueur
//    //    /// </summary>
//    //    /// <param name="playerId">Id du joueur à tester</param>
//    void HasWon(int playerId)
//    {
//        switch (m_players[playerId].Character.characterWinningCondition)
//        {
//            case WinningCondition.BeingAlive:
//                if (!m_players[playerId].Dead.Value && m_isGameOver)
//                    m_players[playerId].HasWon.Value = true;
//                break;
//            case WinningCondition.HavingEquipement:
//                if (m_players[playerId].ListCard.Count >= 5)
//                {
//                    m_players[playerId].HasWon.Value = true;
//                    m_isGameOver = true;
//                }
//                break;
//            case WinningCondition.Bryan:
//                // TODO vérifier si tue un perso de 13 HP ou plus
//                if (m_players[playerId].Position == Position.Sanctuaire && m_isGameOver)
//                    m_players[playerId].HasWon.Value = true;
//                break;
//            case WinningCondition.David:
//                int nbCardsOwned = 0;
//                if (m_players[playerId].HasCrucifix.Value)
//                    nbCardsOwned++;
//                if (m_players[playerId].HasAmulet.Value)
//                    nbCardsOwned++;
//                if (m_players[playerId].HasSpear.Value)
//                    nbCardsOwned++;
//                if (m_players[playerId].HasToge.Value)
//                    nbCardsOwned++;

//                if (nbCardsOwned >= 3)
//                {
//                    m_players[playerId].HasWon.Value = true;
//                    m_isGameOver = true;
//                }
//                break;
//            case WinningCondition.HunterCondition:
//                if (m_nbShadowsDeads == m_nbShadows)
//                {
//                    m_players[playerId].HasWon.Value = true;
//                    m_isGameOver = true;
//                }
//                break;
//            case WinningCondition.ShadowCondition:
//                if (m_nbHuntersDead == m_nbHunters || m_nbNeutralsDeads == 3)
//                {
//                    m_players[playerId].HasWon.Value = true;
//                    m_isGameOver = true;
//                }
//                break;
//        }
//    }
//    */


//    public void OnEvent(PlayerEvent e, string[] tags = null)
//    {
//        if (e is EndTurnEvent ete)
//        {
//            Console.WriteLine("Endturn of : " + ete.PlayerId);
//            if (PlayerTurn.Value == -1)
//                PlayerTurn.Value = UnityEngine.Random.Range(0, m_players.Count - 1);
//            else if (m_players[PlayerTurn.Value].HasAncestral.Value) // si le joueur a utilisé le savoir ancestral, le joueur suivant reste lui
//            {
//                Console.WriteLine("Le joueur " + m_players[PlayerTurn.Value].Name + " rejoue grâce au Savoir Ancestral !");
//                m_players[PlayerTurn.Value].HasAncestral.Value = false;
//            }
//            else
//                PlayerTurn.Value = (PlayerTurn.Value + 1) % m_players.Count;
//            Console.WriteLine("C'est au joueur " + m_players[PlayerTurn.Value].Name + " de jouer.");

//            Player currentPlayer = m_players[PlayerTurn.Value];

//            currentPlayer.RollTheDices.Value = true;

//            if (currentPlayer.HasGuardian.Value)
//            {
//                currentPlayer.HasGuardian.Value = false;
//                Console.WriteLine("Le joueur " + currentPlayer.Name + " n'est plus affecté par l'Ange Gardien !");
//            }

//            if (currentPlayer.Revealed.Value)
//            {
//                if (currentPlayer.Character.characterName == "Emi"
//                    || currentPlayer.Character.characterName == "Franklin"
//                    || currentPlayer.Character.characterName == "Georges")
//                {
//                    currentPlayer.CanUsePower.Value = true;
//                }
//            }

//            EventView.Manager.Emit(new SelectedNextPlayer()
//            {
//                PlayerId = PlayerTurn.Value
//            });
//        }
//        else if (e is NewTurnEvent nte)
//        {
//            Player currentPlayer = m_players[nte.PlayerId];
//            currentPlayer.RollTheDices.Value = false;

//            if (currentPlayer.Character.characterName == "Emi"
//                || currentPlayer.Character.characterName == "Franklin"
//                || currentPlayer.Character.characterName == "Georges")
//            {
//                currentPlayer.CanUsePower.Value = false;
//            }

//            List<Position> position = new List<Position>();

//            if (currentPlayer.HasCompass.Value)
//            {
//                int lancer01 = UnityEngine.Random.Range(1, 6);
//                int lancer02 = UnityEngine.Random.Range(1, 4);
//                int lancer11 = UnityEngine.Random.Range(1, 6);
//                int lancer12 = UnityEngine.Random.Range(1, 4);

//                EventView.Manager.Emit(new SelectDiceThrow()
//                {
//                    PlayerId = currentPlayer.Id,
//                    D6Dice1 = lancer01,
//                    D4Dice1 = lancer02,
//                    D6Dice2 = lancer11,
//                    D4Dice2 = lancer12,
//                });
//            }
//            else
//            {
//                int lancer01 = UnityEngine.Random.Range(1, 6);
//                int lancer02 = UnityEngine.Random.Range(1, 4);

//                while (position.Count >= 0)
//                {
//                    lancer01 = UnityEngine.Random.Range(1, 6);
//                    lancer02 = UnityEngine.Random.Range(1, 4);

//                    switch (lancer01 + lancer02)
//                    {
//                        case 2:
//                        case 3:
//                            position.Add(Position.Antre);
//                            break;
//                        case 4:
//                        case 5:
//                            position.Add(Position.Porte);
//                            break;
//                        case 6:
//                            position.Add(Position.Monastere);
//                            break;
//                        case 7:
//                            position.Add(Position.Antre);
//                            position.Add(Position.Porte);
//                            position.Add(Position.Monastere);
//                            position.Add(Position.Cimetiere);
//                            position.Add(Position.Foret);
//                            position.Add(Position.Foret);
//                            break;
//                        case 8:
//                            position.Add(Position.Cimetiere);
//                            break;
//                        case 9:
//                            position.Add(Position.Foret);
//                            break;
//                        case 10:
//                            position.Add(Position.Sanctuaire);
//                            break;
//                    }
//                    if (position.Contains(currentPlayer.Position))
//                    {
//                        currentPlayer.Position = position[0];
//                    }
//                    else
//                        position.Remove(currentPlayer.Position);

//                }
//                EventView.Manager.Emit(new SelectMovement()
//                {
//                    PlayerId = currentPlayer.Id,
//                    D6Dice = lancer01,
//                    D4Dice = lancer02,
//                    LocationAvailable = position.ToArray()
//                });
//            }

//        }
//        else if (e is SelectedDiceEvent sde)
//        {
//            Player currentPlayer = m_players[sde.PlayerId];
//            List<Position> position = new List<Position>();

//            while (position.Count >= 0)
//            {
//                switch (sde.D4Dice + sde.D6Dice)
//                {
//                    case 2:
//                    case 3:
//                        position.Add(Position.Antre);
//                        break;
//                    case 4:
//                    case 5:
//                        position.Add(Position.Porte);
//                        break;
//                    case 6:
//                        position.Add(Position.Monastere);
//                        break;
//                    case 7:
//                        position.Add(Position.Antre);
//                        position.Add(Position.Porte);
//                        position.Add(Position.Monastere);
//                        position.Add(Position.Cimetiere);
//                        position.Add(Position.Foret);
//                        position.Add(Position.Sanctuaire);
//                        break;
//                    case 8:
//                        position.Add(Position.Cimetiere);
//                        break;
//                    case 9:
//                        position.Add(Position.Foret);
//                        break;
//                    case 10:
//                        position.Add(Position.Sanctuaire);
//                        break;
//                }
//                if (position.Contains(currentPlayer.Position))
//                {
//                    currentPlayer.Position = position[0];
//                }
//                else
//                    position.Remove(currentPlayer.Position);

//            }
//            EventView.Manager.Emit(new SelectMovement()
//            {
//                PlayerId = currentPlayer.Id,
//                D6Dice = sde.D6Dice,
//                D4Dice = sde.D4Dice,
//                LocationAvailable = position.ToArray()
//            });
//        }
//        else if (e is MoveOn mo)
//        {
//            Player currentPlayer = m_players[mo.PlayerId];
//            currentPlayer.Position = mo.Location;
//            gameBoard.setPositionOfAt(currentPlayer.Id, mo.Location);

//            currentPlayer.AttackPlayer.Value = true;
//            if (currentPlayer.HasSaber.Value)
//                currentPlayer.EndTurn.Value = false;
//            else
//                currentPlayer.EndTurn.Value = true;

//            switch (currentPlayer.Position)
//            {
//                case Position.Antre:
//                    currentPlayer.DrawLightCard.Value = true;
//                    break;
//                case Position.Porte:
//                    currentPlayer.DrawLightCard.Value = true;
//                    currentPlayer.DrawDarknessCard.Value = true;
//                    currentPlayer.DrawVisionCard.Value = true;
//                    break;
//                case Position.Monastere:
//                    currentPlayer.DrawVisionCard.Value = true;
//                    break;
//                case Position.Cimetiere:
//                    currentPlayer.DrawDarknessCard.Value = true;
//                    break;
//                case Position.Foret:
//                    currentPlayer.ForestHeal.Value = true;
//                    currentPlayer.ForestWounds.Value = true;
//                    break;
//                case Position.Sanctuaire:
//                    List<int> target2 = new List<int>();
//                    foreach (Player p in m_players)
//                        if (!p.Dead.Value && p.Id != currentPlayer.Id && p.ListCard.Count > 0)
//                            target2.Add(p.Id);

//                    EventView.Manager.Emit(new SelectStealCardEvent()
//                    {
//                        PlayerId = currentPlayer.Id,
//                        PossiblePlayerTargetId = target2.ToArray()
//                    });
//                    break;
//            }
//        }
//        else if (e is ForestSelectTargetEvent fste)
//        {
//            List<int> target = new List<int>();

//            foreach (Player p in m_players)
//            {
//                if (!p.Dead.Value)
//                {
//                    if(fste.Hurt)
//                        if(!p.HasBroche.Value)
//                            target.Add(p.Id);
//                        else
//                            continue;
//                    else
//                        target.Add(p.Id);
//                }
//            }   
//            int wounds;
//            if(fste.Hurt)
//            {
//                wounds=2;
//            }
//            else
//            {
//                wounds=-1;
//            }
//            EventView.Manager.Emit(new SelectPlayerTakingWoundsEvent()
//            {
//                PlayerId = fste.PlayerId,
//                PossibleTargetId = target.ToArray(),
//                IsPuppet = false,
//                NbWoundsTaken = wounds,
//                NbWoundsSelfHealed = 0
//            });
//        }
//        else if (e is PowerUsedEvent powerUsed)
//        {
//            PlayerCardPower(m_players[powerUsed.PlayerId]);
//        }
//        else if (e is PowerNotUsedEvent powerNotUsed)
//        {
//            DontUsePower(m_players[powerNotUsed.PlayerId]);
//        }
//        else if (e is DrawCardEvent drawCard)
//        {
//            Player player = m_players[drawCard.PlayerId];
            
//            switch (drawCard.SelectedCardType)
//            {
//                case CardType.Darkness:
//                    Debug.Log("Le joueur " + player.Name + " choisit de piocher une carte Ténèbres.");
//                    DarknessCard darknessCard = gameBoard.DrawCard(CardType.Darkness) as DarknessCard;

//                    if (darknessCard is IEquipment)
//                    {
//                        player.AddCard(darknessCard);
//                        Debug.Log("La carte " + darknessCard.cardName + " a été ajoutée à la main du joueur "
//                            + player.Name + ".");
//                    }

//                    DarknessCardPower(darknessCard, player.Id);

//                    if (!darknessCard is IEquipment)
//                        gameBoard.AddDiscard(darknessCard, CardType.Darkness);

//                    break;
//                case CardType.Light:

//                    Debug.Log("Le joueur " + player.Name + " pioche une carte Lumière.");

//                    LightCard lightCard = gameBoard.DrawCard(CardType.Light) as LightCard;

//                    if (lightCard is IEquipment)
//                    {
//                        player.AddCard(lightCard);
//                        Debug.Log("La carte " + lightCard.cardName + " a été ajoutée à la main du joueur "
//                            + player.Name + ".");
//                    }

//                    LightCardPower(lightCard, player.Id);

//                    if (!lightCard is IEquipment)
//                        gameBoard.AddDiscard(lightCard, CardType.Light);

//                    break;
//                case CardType.Vision:

//                    Debug.Log("Le joueur " + player.Name + " choisit de piocher une carte Vision.");

//                    VisionCard visionCard = gameBoard.DrawCard(CardType.Vision) as VisionCard;

//                    VisionCardPower(visionCard, player.Id);

//                    gameBoard.AddDiscard(visionCard, CardType.Vision);

//                    break;
//            }
//        }
//        else if (e is AttackEvent attack)
//        {
//            AttackCorrespondingPlayer(attack.PlayerId);
//        }
//        else if (e is AttackPlayerEvent attackPlayer)
//        {
//            Player playerAttacking = m_players[attackPlayer.PlayerId];
//            Player playerAttacked = m_players[attackPlayer.PlayerAttackedId];

//            Debug.Log("Joueur " + playerAttacking.Id + " (" + playerAttacking.Character.characterName 
//                        + ") attaque joueur " + playerAttacked.Id + " (" + playerAttacked.Character.characterName + ")");

//            int lancer1 = UnityEngine.Random.Range(1, 6);
//            int lancer2 = UnityEngine.Random.Range(1, 4);
//            int lancerTotal = (playerAttacking.HasSaber.Value == true) ? lancer2 : Mathf.Abs(lancer1 - lancer2);

//            if (attackPlayer.PowerFranklin)
//                lancerTotal = lancer1;
//            else if (attackPlayer.PowerGeorges)
//                lancerTotal = lancer2;

//            Debug.Log("Le lancer vaut : " + lancerTotal);

//            if (lancerTotal == 0)
//                Debug.Log("Le lancer vaut 0, vous n'attaquez pas.");
//            else
//            {
//                Debug.Log("Vous choisissez d'attaquer le joueur " + playerAttacked.Name + ".");

//                int dommageTotal = lancerTotal + playerAttacking.BonusAttack.Value - playerAttacking.MalusAttack.Value;

//                // Si Bob est révélé et inflige 2 dégats ou plus, il peut voler une arme 
//                if (playerAttacking.Character.characterName == "Bob"
//                    && playerAttacking.Revealed.Value
//                    && dommageTotal >= 2)
//                {
//                    m_damageBob = dommageTotal;
//                    PlayerAttacked.Value = playerAttacked.Id;
//                    PlayerCardPower(playerAttacking);
//                }
//                else
//                {
//                    // Le joueur attaqué se prend des dégats
//                    playerAttacked.Wounded(dommageTotal,playerAttacking,true);

//                    // Le Vampire se soigne 2 blessures s'il est révélé et s'il a infligé des dégats
//                    if (playerAttacking.Character.characterName == "Vampire"
//                        && playerAttacking.Revealed.Value
//                        && dommageTotal > 0)
//                        PlayerCardPower(playerAttacking);

//                    // Le Loup-garou peut contre attaquer
//                    if (playerAttacked.Character.characterName == "LoupGarou"
//                        && playerAttacked.Revealed.Value)
//                    {
//                        playerAttacked.CanUsePower.Value = true;
//                    }

//                    // Charles peut attaquer de nouveau
//                    if (playerAttacking.Character.characterName == "Charles"
//                        && playerAttacking.Revealed.Value)
//                    {
//                        playerAttacking.CanUsePower.Value = true;
//                    }
//                }
//            }
//        }
//        else if (e is StealCardEvent stealTarget)
//        {
//            Player playerStealing = m_players[stealTarget.PlayerId];
//            Player playerStealed = m_players[stealTarget.PlayerStealedId];
//            string stealedCard = stealTarget.CardStealedName;
//            int indexCard = playerStealed.HasCard(stealedCard);

//            if(!playerStealed.ListCard[indexCard] is IEquipment)
//            {
//                Debug.LogError("Erreur : la carte choisie n'est pas un équipement et ne devrait pas être là.");
//                return;
//            }

//            playerStealing.PrintCards();

//            playerStealing.AddCard(playerStealed.ListCard[indexCard]);

//            if (playerStealed.ListCard[indexCard].cardType == CardType.Darkness)
//                DarknessCardPower(playerStealed.ListCard[indexCard] as DarknessCard, playerStealing.Id);

//            else if (playerStealed.ListCard[indexCard].cardType == CardType.Light)
//                LightCardPower(playerStealed.ListCard[indexCard] as LightCard, playerStealing.Id);

//            LooseEquipmentCard(playerStealed.Id, indexCard);

//            Debug.Log("La carte " + stealedCard + " a été volée au joueur "
//                + playerStealed.Name + " par le joueur " + playerStealing.Name + " !");


//            playerStealing.PrintCards();
//        }
//        else if (e is GiveCardEvent giveCard)
//        {
//            Player playerGiving = m_players[giveCard.PlayerId];
//            Player playerGived = m_players[giveCard.PlayerGivedId];
//            string givedCard = giveCard.CardGivedName;

//            int indexCard = playerGiving.HasCard(givedCard);
//            playerGived.AddCard(playerGiving.ListCard[indexCard]);

//            playerGiving.PrintCards();
//            playerGived.PrintCards();

//            if (playerGiving.ListCard[indexCard] is IEquipment)
//            {
//                if (playerGiving.ListCard[indexCard].cardType == CardType.Darkness)
//                    DarknessCardPower(playerGiving.ListCard[indexCard] as DarknessCard, playerGived.Id);

//                else if (playerGiving.ListCard[indexCard].cardType == CardType.Light)
//                    LightCardPower(playerGiving.ListCard[indexCard] as LightCard, playerGived.Id);

//                LooseEquipmentCard(playerGiving.Id, indexCard);
//            }
//            else
//                Debug.LogError("Erreur : la carte choisie n'est pas un équipement et ne devrait pas être là.");

//            Debug.Log("La carte " + givedCard + " a été donnée au joueur "
//                + playerGived.Name + " par le joueur " + playerGiving.Name + " !");

//            playerGiving.PrintCards();
//            playerGived.PrintCards();
//        }
//        else if (e is TakingWoundsEffectEvent takingWounds)
//        {
//            Player playerAttacking = m_players[takingWounds.PlayerId];
//            Player playerAttacked = m_players[takingWounds.PlayerAttackedId];
//            bool isPuppet = takingWounds.IsPuppet;
//            int nbWoundsTaken = takingWounds.NbWoundsTaken;
//            int nbWoundsSelfHealed = takingWounds.NbWoundsSelfHealed;

//            if (isPuppet)
//            {
//                int lancer = UnityEngine.Random.Range(1, 6);
//                Debug.Log("Le lancer donne " + lancer + ".");

//                if (lancer <= 4)
//                    playerAttacked.Wounded(nbWoundsTaken,playerAttacking,false);
//                else
//                    playerAttacking.Wounded(nbWoundsTaken,playerAttacking,false);
//            }
//            else
//            {
//                if (nbWoundsSelfHealed < 0)
//                    playerAttacking.Wounded(-nbWoundsSelfHealed,playerAttacking,false);
//                else
//                    playerAttacking.Healed(nbWoundsSelfHealed);

//                if (nbWoundsTaken < 0)
//                    playerAttacked.Healed(-nbWoundsTaken);
//                else
//                    playerAttacked.Wounded(nbWoundsTaken,playerAttacking,false);
//            }
//        }
//        else if (e is RevealOrNotEvent revealOrNot)
//        {
//            Player player = m_players[revealOrNot.PlayerId];
//            bool hasRevealed = revealOrNot.HasRevealed;
//            Card effectCard = revealOrNot.EffectCard;

//            if (effectCard is DarknessCard
//                && hasRevealed
//                && player.Character.team == CharacterTeam.Shadow)
//            {
//                player.Healed(player.Wound.Value);
//                Debug.Log("Le joueur " + player.Name + " se soigne complètement");
//            }
//            else if (effectCard is LightCard effectLightCard)
//            {
//                if (effectLightCard.lightEffect == LightEffect.Supreme
//                    && hasRevealed
//                    && player.Character.team == CharacterTeam.Hunter)
//                {
//                    player.Healed(player.Wound.Value);
//                    Debug.Log("Le joueur " + player.Name + " se soigne complètement");
//                }
//                else if (effectLightCard.lightEffect == LightEffect.Chocolat
//                            && hasRevealed
//                            && (player.Character.characterName == "Allie"
//                                || player.Character.characterName == "Emi"
//                                || player.Character.characterName == "Metamorphe"))
//                {
//                    player.Healed(player.Wound.Value);
//                    Debug.Log("Le joueur " + player.Name + " se soigne complètement");
//                }
//            }
//            else
//                Debug.Log("Rien ne se passe.");
//        }
//        else if (e is LightCardEffectEvent lcEffect)
//        {
//            Player player = m_players[lcEffect.PlayerId];
//            Player playerChoosed = m_players[lcEffect.PlayerChoosenId];
//            LightCard lightCard = lcEffect.LightCard;

//            if (lightCard.lightEffect == LightEffect.Benediction)
//            {
//                Debug.Log("Vous choisissez de soigner le joueur " + playerChoosed.Name + ".");
//                playerChoosed.Healed(UnityEngine.Random.Range(1, 6));
//            }
//            else if (lightCard.lightEffect == LightEffect.PremiersSecours)
//            {
//                Debug.Log("Vous choisissez d'infliger 7 blessures au joueur " + playerChoosed.Name + ".");
//                playerChoosed.Wound.Value = 7;
//            }
//        }
//        else if (e is VisionCardEffectEvent vcEffect)
//        {

//            Player playerGiving = m_players[vcEffect.PlayerId];
//            Player playerGived = m_players[vcEffect.TargetId];
//            VisionCard pickedCard = vcEffect.VisionCard;
//            bool metaPower = vcEffect.MetamorphePower;

//            Debug.Log("La carte Vision a été donnée au joueur " + playerGived.Name + ".");
//            Debug.Log("Joueur " + playerGived.Name + " :\n" +
//                        playerGived.Character.characterName + "\n" +
//                        playerGived.Character.team + "\n" +
//                        playerGived.Character.characterHP + "\n" +
//                        metaPower);
//            Debug.Log("Effet carte :" +
//                        "\neffectOnShadow" + pickedCard.visionEffect.effectOnShadow +
//                        "\neffectOnHunter" + pickedCard.visionEffect.effectOnHunter +
//                        "\neffectOnNeutral" + pickedCard.visionEffect.effectOnNeutral +
//                        "\neffectOnHighHP" + pickedCard.visionEffect.effectOnHighHP +
//                        "\neffectOnLowHP" + pickedCard.visionEffect.effectOnLowHP);

//            CharacterTeam team = playerGived.Character.team;
//            /*
//            if (playerGived.Character.characterName == "Metamorphe)
//            {
//                // A enlever plus tard
//                usePowerButton.gameObject.SetActive(true);
//                dontUsePowerButton.gameObject.SetActive(true);

//                playerGived.CanUsePower.Value = true;
//                playerGived.CanNotUsePower.Value = true;

//                m_pickedVisionCard = pickedCard;
//            }
//            */
//            // Cartes applicables en fonction des équipes ?
//            if ((team == CharacterTeam.Shadow && pickedCard.visionEffect.effectOnShadow && !metaPower)
//                || (team == CharacterTeam.Hunter && pickedCard.visionEffect.effectOnHunter)
//                || (team == CharacterTeam.Neutral && pickedCard.visionEffect.effectOnNeutral)
//                || (team == CharacterTeam.Shadow && !pickedCard.visionEffect.effectOnShadow && metaPower))
//            {
//                // Cas des cartes infligeant des Blessures
//                if (pickedCard.visionEffect.effectTakeWounds)
//                    playerGived.Wounded(pickedCard.visionEffect.nbWounds,playerGiving,false);

//                // Cas des cartes soignant des Blessures
//                else if (pickedCard.visionEffect.effectHealingOneWound)
//                {
//                    if (playerGived.Wound.Value == 0)
//                        playerGived.Wounded(1,playerGiving,false);
//                    else
//                        playerGived.Healed(1);
//                }
//                // Cas des cartes volant une carte équipement ou infligeant des Blessures
//                else if (pickedCard.visionEffect.effectGivingEquipementCard)
//                {
//                    if (playerGived.ListCard.Count == 0)
//                    {
//                        Debug.Log("Vous ne possédez pas de carte équipement.");
//                        playerGived.Wounded(1,playerGiving,false);
//                    }
//                    else
//                    {
//                        Debug.Log("Voulez-vous donner une carte équipement ou subir une Blessure ?");

//                        EventView.Manager.Emit(new SelectGiveOrWoundEvent()
//                        {
//                            PlayerId = playerGived.Id
//                        });
//                    }
//                }
//            }
//            // Cas des cartes applicables en fonction des points de vie
//            else if (pickedCard.visionEffect.effectOnLowHP && CheckLowHPCharacters(playerGived.Character.characterName))
//                playerGived.Wounded(1,playerGiving,false);
//            else if (pickedCard.visionEffect.effectOnHighHP && CheckHighHPCharacters(playerGived.Character.characterName))
//                playerGived.Wounded(2,playerGiving,false);

//            // Cas de la carte Vision Suprême
//            else if (pickedCard.visionEffect.effectSupremeVision)
//                //TODO montrer la carte personnage
//                Debug.Log("C'est une carte Vision Suprême !");
//            else
//                Debug.Log("Rien ne se passe.");
//        }
//        else if (e is GiveOrWoundEvent giveOrWound)
//        {
//            Player player = m_players[giveOrWound.PlayerId];
//            bool give = giveOrWound.Give;

//            if (give)
//            {
//                Debug.Log("Vous choisissez de donner une carte équipement.");
//                GiveEquipmentCard(player.Id);
//            }
//            else
//            {
//                Debug.Log("Vous choisissez de subir 1 Blessure.");
//                player.Wounded(1,m_players[PlayerTurn],false);
//            }
//        }
//        else if (e is BobPowerEvent bobPower)
//        {
//            Player playerBob = m_players[bobPower.PlayerId];
//            Player playerBobed = m_players[PlayerAttacked.Value];
//            int bobDamages = m_damageBob;
//            bool usePower = bobPower.UsePower;

//            if (usePower)
//            {
//                StealEquipmentCard(playerBob.Id, playerBobed.Id);
//            }
//            else
//            {
//                AttackCorrespondingPlayer(playerBob.Id, playerBobed.Id, bobDamages);
//            }
//        }
//        else if (e is RevealCard reveal)
//        {
//            RevealCard(m_players[reveal.PlayerId]);
//        }
//        else if (e is TestEvent test)
//        {
//            Player psing = m_players[test.PlayerId];

//            GiveEquipmentCard(psing.Id);

//            /*
//             * Player psed1 = m_players[test.PlayerId + 1];
//            Player psed2 = m_players[test.PlayerId + 2];
//            Player psed3 = m_players[test.PlayerId + 3];

//            for (int i = 0; i < 2; i++)
//            {
//                LightCard dc = gameBoard.DrawCard(CardType.Light) as LightCard;
//                while (!dc is IEquipment)
//                {
//                    dc = gameBoard.DrawCard(CardType.Light) as LightCard;
//                }
//                psed1.AddCard(dc);
//                psed2.AddCard(dc);
//                psed3.AddCard(dc);
//            }

//            StealEquipmentCard(psing.Id);
//            */

//            /*
//            // Test carte lumière non equipement
//            LightCard dc = gameBoard.DrawCard(CardType.Light) as LightCard;
//            while(!dc is IEquipment)
//            {
//                dc = gameBoard.DrawCard(CardType.Light) as LightCard;
//            }
//            psing.AddCard(dc);

//            LooseEquipmentCard(psing.Id, 0);
//            */

//        }
//    }

//    private void DontUsePower(Player player)
//    {
//        throw new NotImplementedException();
//    }
//}