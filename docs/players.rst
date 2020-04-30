=======
Players
=======

Implemented in the software design pattern MVC (Model View Controller).

.. default-domain:: csharp

Controller
==========

This is where the different classes in the Model folder are instanciated.

.. class:: GCharacter

    Class that insanciate every character in the game.

    .. method:: public GCharacter(int nbPlayers)

        Constructor of the class.

    .. method:: public Character PickCharacter()

        Function that pick randomly a character in a list of character.

.. class:: GGoal

    Static class that insanciate the winning conditions of every character.

    .. code-block:: csharp
        
        public static Goal HunterGoal = new Goal( checkWinning, setWinningListeners );
        public static Goal ShadowGoal = new Goal( checkWinning, setWinningListeners );
        public static Goal AllieGoal = new Goal( checkWinning, setWinningListeners );
        public static Goal BobGoal = new Goal( checkWinning, setWinningListeners );
        public static Goal CharlesGoal = new Goal( checkWinning, setWinningListeners );
        public static Goal DanielGoal = new Goal( checkWinning, setWinningListeners );

.. class:: GPlayer
    
    Class that insanciate a character to a player.
    
    .. code-block:: csharp
    
        public Player[] Players { get; private set; }
        
    Array of players.
    
    .. method:: public GPlayer(int nbPlayers)

        Constructor of the class.

.. class:: GPower

    Class that insanciate the power of a character.

    .. code-block:: csharp

        public static Power George = new Power( power, addListeners, availability );
        public static Power Franklin = new Power( power, addListeners, availability );
        public static Power Emi = new Power( power, addListeners, availability );
        public static Power Loup = new Power( power, addListeners, availability );
        public static Power Vampire = new Power( power, addListeners, availability );
        public static Power Allie = new Power( power, addListeners, availability );
        public static Power Bob = new Power( power, addListeners, availability );
        public static Power Charles = new Power( power, addListeners, availability );
        public static Power Daniel = new Power( power, addListeners, availability );

.. class:: PlayerListener 
    
    .. inherits:: IListener<PlayerEvent>

    Class that listen every event_in.

    .. method:: public void OnEvent(PlayerEvent e, string[] tags = null)

Model
=====

This is the part where every part of the character is defined.

.. class:: Character

    Class of the definition of a character.

    .. enum:: CharacterTeam

        .. value:: Shadow, Hunter, Neutral

    .. method:: public Character(string characterName, CharacterTeam team, int characterHP, Goal goal, Power power)

        Constructor of the class.

.. class:: Goal

    Class of the definition of a winning condition.

    .. code-block:: csharp

        public delegate void CheckWinningCondition(Player owner);
        public delegate void SetWinningListeners(Player owner);

    Functions used to tests if the winning condition is reached and to add listeners on specific attributes.

    .. method:: public Goal(CheckWinningCondition checkWinning, SetWinningListeners setWinningListeners)

        Constructor of the class.

.. class:: Player

    Class of the definition of a player.

    .. enum:: PlayerNames

        .. value:: Alpha, Bravo, Charlie, Delta, Echo, Foxtrot, Golf, Hotel

    .. code-block:: csharp

        // id du joueur
        public int Id { get; private set; }
        // nom du joueur
        public string Name { get; set; }
        // nombre de blessure
        public Setting<int> Wound { get; private set; } = new Setting<int>(0);
        // carte révélée à tous ou cachée
        public Setting<bool> Revealed { get; private set; } = new Setting<bool>(false);
        // vivant ou mort
        public Setting<bool> Dead { get; private set; } = new Setting<bool>(false);
        // pouvoir déjà utilisé ou non
        public Setting<bool> UsedPower { get; private set; } = new Setting<bool>(false);
        // bonus d'attaque (par défaut = 0)
        public Setting<int> BonusAttack { get; private set; } = new Setting<int>(0);
        // malus d'attaque (par défaut = 0)
        public Setting<int> MalusAttack { get; private set; } = new Setting<int>(0);
        // réduction du nombre de Blessures subites (par défaut = 0)
        public Setting<int> ReductionWounds { get; private set; } = new Setting<int>(0);
        // le joueur possède-t-il la mitrailleuse ?
        public Setting<bool> HasGatling { get; private set; } = new Setting<bool>(false);
        // le joueur possède-t-il le revolver ?
        public Setting<bool> HasRevolver { get; private set; } = new Setting<bool>(false);
        // le joueur possède-t-il le sabre ?
        public Setting<bool> HasSaber { get; private set; } = new Setting<bool>(false);
        // le joueur possède-t-il l'amulette ?
        public Setting<bool> HasAmulet { get; private set; } = new Setting<bool>(false);
        // le joueur possède-t-il la broche ?
        public Setting<bool> HasBroche { get; private set; } = new Setting<bool>(false);
        // le joueur possède-t-il la boussole ?
        public Setting<bool> HasCompass { get; private set; } = new Setting<bool>(false);
        // le joueur possède-t-il le crucifix ?
        public Setting<bool> HasCrucifix { get; private set; } = new Setting<bool>(false);
        // le joueur possède-t-il la lance ?
        public Setting<bool> HasSpear { get; private set; } = new Setting<bool>(false);
        // le joueur possède-t-il la toge ?
        public Setting<bool> HasToge { get; private set; } = new Setting<bool>(false);
        // le joueur est-il sous l'effet de l'ange gardien ?
        public Setting<bool> HasGuardian { get; private set; } = new Setting<bool>(false);
        // le joueur est-il sous l'effet du savoir ancestral ?
        public Setting<bool> HasAncestral { get; private set; } = new Setting<bool>(false);
        // nb d'équipements
        public Setting<int> NbEquipment { get; private set; } = new Setting<int>(0);
        // le joueur a-t-il gagné ?
        public Setting<bool> HasWon { get; private set; } = new Setting<bool>(false);
        // position du joueur
        public Setting<int> Position { get; private set; } = new Setting<int>(-1);

        // personnage du joueur
        public Character Character { get; private set; }
        // liste des cartes possédées par le joueur
        public List<Card> ListCard { get; private set; }
        // le joueur peut-il utiliser son pouvoir ?
        public Setting<bool> CanUsePower { get; private set; } = new Setting<bool>(false);
        // le joueur a-t-il déjà utilisé son pouvoir une fois (utilisé pour les usages uniques)
        public Setting<bool> PowerUsed { get; private set; } = new Setting<bool>(false);
        // Id du joueur qui m'a attaqué en dernier (Loup-garou)
        public Setting<int> OnAttacked { get; private set; } = new Setting<int>(-1);
        // Id du joueur que j'ai attaqué en dernier (Charles)
        public Setting<int> OnAttacking { get; private set; } = new Setting<int>(-1);
        // Nombre de dommage reçu
        public Setting<int> OnDealDamage { get; private set; } = new Setting<int>(0);
        // Nombre de dommage infligé pour la dernière fois en attaquant
        public Setting<int> DamageDealed { get; private set; } = new Setting<int>(-1);

    .. method:: public Player(int id, Character c)

        Constructor of the class.

    .. method:: public virtual int Wounded(int damage, Player attacker, bool isAttack)

        Function that deals damage.

    .. method:: public virtual void Healed(int heal)

        Function that heals.

    .. method:: public void AddCard(Card card)

        Function that add a card in the hands of a player.

    .. method:: public void RemoveCard(int index)

        Function that remove a card from the hand of a player.

    .. method:: public int HasCard(string cardName)

        Function that check if a player has a card.

    .. method:: public List<Player> getTargetablePlayers()

        Function that lists every others players that can be targeted.

.. class:: Power

    .. code-block:: csharp

        public delegate void CharaterPower(Player owner);
        public delegate void CharaterAvailabilityPowerListeners(Player owner);
        public delegate void CharaterAvailabilityPower(Player owner);
    
    Functions used to implemente the power of a character, when he can uses his powers
    and add listeners on specific attributes.

    .. method:: public Power(CharacterPower power, CharacterAvailabilityPowerListeners addListeners, CharacterAvailabilityPower availability)

        Constructor of the class.

View
====

This is the part where the Controller is called to generate every players needed for the game.

.. class:: PlayerView

    Class that initiate a character for every player

    .. code-block:: csharp

        private static GPlayer gPlayer;
        public static int NbPlayer { get; private set; }

    .. method:: public static void Init(int nbPlayers)

        Initiate every player.
    
    .. method:: public static void Clean()
    
        Destructor of every player.

    .. method:: public static Player GetPlayer(int id)

        Get a player by his Id.

    .. method:: public static Player[] GetPlayers()

        Get every players.

    .. method:: public static Player NextPlayer(Player currentPlayer)

        Select the next player.