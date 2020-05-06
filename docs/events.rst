======
Events
======

There are two types of events used for the kernel:

    * the events that are waited by the kernel to occur (called events_in)
    * the events that are sent by the kernel (called events_out).

In this page, every events that are used will be listed.

.. default-domain:: csharp

Main event
==========

.. namespace:: Assets.Noyau

.. class:: PlayerEvent

    .. inherits:: Event

       It is the main event, herited by all the other, used to interpret actions from the player.

    .. property:: public int PlayerId { get; set; }

       The id of the player that is concerned by the event.

events_in
=========

.. namespace:: Assets.Noyau.event_in

.. class:: AskMovement

    Movement event at the beginning of the turn.

.. class:: AttackPlayerEvent

    Callback of the SelectAttackTargetEvent.

    .. property:: public bool PowerFranklin { get; set; }

       Boolean used to know if Franklin is the player's character that sent the event.

    .. property:: public bool PowerGeorges { get; set; }

       Boolean used to know if Georges is the player's character that sent the event.

    .. property:: public bool PowerLoup { get; set; }
    
       Boolean used to know if Werewolf is the player's character that sent the event.

    .. property:: public bool PowerCharles { get; set; }

       Boolean used to know if Charles is the player's character that sent the event.

.. class:: DrawCardEvent

   Event called when the player draws a card.

   .. property:: public CardType SelectedCardType { get; set; }

      Type of the selected card.

.. class:: EndTurnEvent

   Event sent when the player press the button to end the turn to choose the next player.

.. class:: MoveOn

   Event which moves the player inside an area.

   .. property:: public int Location { get; set; }

      Id of the area where the player will move.

.. class:: PowerUseEvent

   Event that is triggered when a player choose to use his power.

.. class:: RevealCardEvent

   Event triggered when a player choose to reveal his card.

.. class:: RevealOrNotEvent

   Callback of the SelectRevealOrNotEvent.

   .. property:: public Card EffectCard { get; set; }

      Card which effect is used.

   .. property:: public bool HasRevealed { get; set; }

      Boolean representing the choice of the player to reveal or not.

   .. property:: public bool PowerLoup { get; set; }

      Boolean to know if the player is Werewolf or not.
      Roll of the 4-faces die.

.. class:: UsableCardUseEvent

   Event sent when a single use card is used.

   .. property:: public int Cardid { get; set; }

      Id of the card used.

   .. property:: public int EffectSelected { get; set; }

      Index of the effect to trigger.

   .. property:: public int PlayerSelected { get; set; }

      Id of the player targeted by the card.

   .. method:: public UsableCardUseEvent(int card_id, int effect_selected, int player_selected)

      Constructor used to serialize the event.

   .. method:: public UsableCardUseEvent()

      Constructor called when creating the event.


events_out
==========

.. class:: DrawEquipmentCardEvent

   Event that transmit the info that an equipment card has been drawn.

   .. property:: public int CardId { get; set; }

      Id of the card.

   .. method:: public DrawEquipmentCardEvent (int id)

      Constructor used to serialize the event.
   
   .. method:: public DrawEquipmentCardEvent ()

      Constructor called when creating the event.

.. class:: SelectAttackTargetEvent

   Event that allow to choose a target in a list of player

   .. property:: public int[] PossibleTargetId { get; set; }

      Array of Id's possible targets.

   .. property:: public int TargetID { get; set; }

      Id of a specific target.

   .. property:: public bool PowerFranklin { get; set; }

      Boolean if the power of the character Franklin is used.

   .. property:: public bool PowerGeorges { get; set; }

      Boolean if the power of the character Georges is used.

   .. property:: public bool PowerLoup { get; set; }

      Boolean if the power of the character Werewolf is used.

   .. property:: public bool PowerCharles { get; set; }

      Boolean if the power of the character Charles is used.

.. class:: SelectBobPowerEvent

   Callback of BobPowerEvent

.. class:: SelectMovement

   Event that gives available locations to the player.

   .. property::  public int[] LocationAvailable { get; set; }

      Array of possible locations.

.. class:: SelectRevealOrNotEvent

   Event that allow the player to choose to reveal his character.

   .. property:: public Card EffectCard { get; set; }

      Initialised at null, the card that forces the player to reveal himself.

   .. property:: public bool PowerDaniel { get; set; }

      Initialised at false, boolean if the power of Daniel is used.

   .. property:: public bool PowerLoup { get; set; }

      Initialised at false ,boolean if the power of Werewolf is used.

.. class:: SelectUsableCardPickedEvent

   Event that announce a usable card had been drawned.

   .. property:: public int CardId { get; set; }

      Id of the card.

   .. property:: public bool IsHidden { get; set; }

      Boolean if  the card is hidden (in the case of a vision card, only 2 players can see).

   .. method:: public SelectUsableCardPickedEvent(int cardId, bool isHidden, int playerId)

      Constructor used to serialize the event.

   .. method:: public SelectUsableCardPickedEvent()

      Constructor called when creating the event.

.. class:: SelectVisionTargetEvent

   Event to select a target for the vision card picked

   .. property:: public int cardId { get; set; }

      Id of the card to give.

.. class:: ShowCharacterCardEvent

   Event for the vision card Prediction

   .. property:: public string CardLabel { get; set; }

      Label of the character name to give.