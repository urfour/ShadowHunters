=====
Cards
=====

Implemented in the software design pattern MVC (Model View Controller).

.. default-domain:: csharp

Controller
==========

This is where the different classes in the Model folder are instanciated.

.. class:: GCard

    .. code-block:: csharp

        public List<Card> cards = new List<Card>();

        public List<Card> visionDeck;
        public List<Card> lightDeck;
        public List<Card> darknessDeck;

        public UsableCard Foret;
        public UsableCard Sanctuaire;
    
    .. method:: public GCard()

        Constructor of the class that instanciate every cards of the game.

    .. method:: public EquipmentCard CreateEquipmentCard(string cardLabel, CardType cardType, string description, EquipmentCondition condition, EquipmentAddEffect addeffect, EquipmentRemoveEffect rmeffect)

        Function that create an equipment card.

    .. method:: public UsableCard CreateUsableCard(string cardLabel, CardType cardType, string description, bool canDismiss, params CardEffect[] cardEffect)

    Function that create an usable card.

    .. method:: public UsableCard CreateUsableCard(string cardLabel, CardType cardType, string description, bool canDismiss, bool hiddenChoices, params CardEffect[] cardEffect)

    Overload of the previous.

    .. method:: public UsableCard CreateVisionCard(string cardLabel, CardType cardType, string description, bool canDismiss, params CardEffect[] cardEffect)

    Function that create a vision card.

Model
=====

This is where every type of card is defined.

.. class:: Card

    .. enum:: CardType
        
        .. value:: Location, Vision, Light, Darkness

    .. method:: public Card(string cardLabel, CardType cardType, string description, int id)

        Constructor of the class.

.. class:: CardEffect

    .. code-block:: csharp

        public delegate void Effect(Player target, Player owner, UsableCard card);
        public delegate bool PlayerTargetable(Player target, Player owner);

    Function used to create the effect of a card and which player can be targeted by its effects.

    .. method:: public CardEffect(string description, Effect effect, PlayerTargetable targetableCondition)

        Constructor of the class.

.. class:: EquipmentCard

    .. code-block:: csharp

        public delegate void Equipe(Player player, EquipmentCard card);
        public delegate void Unequipe(Player player, EquipmentCard card);
        public delegate bool EquipmentCondition(Player player);
        public delegate void EquipmentAddEffect(Player player, Card card);
        public delegate void EquipmentRemoveEffect(Player player, Card card);

    Function that equipe, add effect or unequipe, remove effect of an equipment card on a player
    and how the equipment can be used and by who.

    .. method:: public EquipmentCard(string cardLabel, CardType cardType, string description, int id, Equipe equipe, Unequipe unequipe, EquipmentCondition condition, EquipmentAddEffect addeffect, EquipmentRemoveEffect rmeffect)
        
        .. code-block:: csharp 
            
            : base(cardLabel, cardType, description, id)

        Constructor of the class.

.. class:: UsableCard

    .. method:: public UsableCard(string cardLabel, CardType cardType, string description, int id, bool canDismiss, params CardEffect[] cardEffect)
        
        .. code-block:: csharp
        
            : base(cardLabel, cardType, description, id)

        Constructor of the class.

    .. method:: public UsableCard(string cardLabel, CardType cardType, string description, int id, bool canDismiss, bool hiddenChoices, params CardEffect[] cardEffect)
        
        .. code-block:: csharp
            
            : base(cardLabel, cardType, description, id)

        Overload of the previous.

View
====

This is the part where the Controller is called to generate every cards needed for the game.

.. class:: CardView

    Class that initiate every cards in the game.

    .. method:: public static void Init()

        Initiate every cards.
    
    .. method:: public static void Clean()
    
        Destructor of every cards.
    
    .. method:: public static Card PickVision()

        Function that pick a vision card randomly.

    .. method:: public static Card PickLight()

        Function that pick a light card randomly.

    .. method:: public static Card PickDarkness()

        Function that pick a darkness card randomly.

    .. method:: public static Card GetCard(int idCard)

        Function that get a card by her Id.
