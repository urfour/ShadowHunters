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

.. class:: AttackEvent

    Event called when the player press the attack button.

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

.. class:: BobPowerEvent

   Callback of the SelectBobPowerEvent.

   .. property:: public bool UsePower{ get; set; }

      If set to true the player use his power, else nothing happens.

.. class:: DrawCardEvent

   Event called when the player draws a card.

   .. property:: public CardType SelectedCardType { get; set; }

      Type of the selected card.

.. class:: EndTurnEvent

   Event sent when the player press the button to end the turn to choose the next player.

.. class:: ForestSelectTargetEvent

   Event used to choose a player from a list to either heal him or wound him.

   .. property:: public bool Hurt { get; set; }

      If set to true, the selected player will be wounded, or else he will be healed.

.. class:: GiveCardEvent

   Callback of the SelectGiveCardEvent.

   .. property:: public int PlayerGivedId { get; set; }

      Id of the player which will get a card.

   .. property:: public int CardId { get; set; }

      Id of the selected card to give.

.. class:: GiveOrWoundEvent

   Callback of the SelectGiveOrWoundEvent.

   .. property:: public bool Give { get; set; }

      If set to true, the player will have to give a card, or else he will be wounded.

.. class:: LightCardEffectEvent

   Callback of the SelectLightCardTargetEvent.

   .. property:: public int PlayerChoosenId { get; set; }

      Id of the player targeted by the card.

   .. property:: public Card LightCard { get; set; }

      Card that has been drawn.

.. class:: MoveOn

   Event which moves the player inside an area.

   .. property:: public int Location { get; set; }

      Id of the area where the player will move.

.. class:: NewTurnEvent

   Event which starts the turn of a player.

.. class:: PowerNotUsedEvent

   Event that is triggered when a player choose not to use his power.

.. class:: PowerUsedEvent

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

.. class:: SelectedDiceEvent

   Callback of the SelectDiceThrowEvent.

   .. property:: public int D6Dice { get; set; }

      Roll of the 6-faces die.

   .. property:: public int D4Dice { get; set; }

      Roll of the 4-faces die.

.. class:: StealCardEvent

   Callback of SelectStealCardEvent and SelectStealCardFromPlayerEvent.

   .. property:: public int PlayerStealedId { get; set; }

      Id of the player who will be stolen a card.

   .. property:: public int CardId { get; set; }

      Id of the stolen card.

.. class:: TakingWoundsEffectEvent

   Callback of the SelectPlayerTakingWoundsEffectEvent.

   .. property:: public int PlayerAttackingId { get; set; }

      Id of the player dealing the wounds.

   .. property:: public bool IsPuppet { get; set; }

      If set to true, the effect applied will be the one of the Demonic Puppet.

   .. property:: public int NbWoundsTaken { get; set; }

      Number of wounds taken.

   .. property:: public int NbWoundsSelfHealed { get; set; }

      Number of wounds healed.

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

.. class:: VisionCardEffectEvent

   Callback of the SelectVisionPowerEvent.

   .. property:: public int TargetId { get; set; }

      Id of the player targeted by the card.

   .. property:: public Card VisionCard { get; set; }

      Card which effect is to be used.

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

.. class:: SelectDiceThrow

   Event when the player have the Compass

   .. property:: public int D6Dice1 { get; set; }

      Value of the first throw of the die 6

   .. property:: public int D4Dice1 { get; set; }
   
      Value of the first throw of the die 4

   .. property:: public int D6Dice2 { get; set; }
   
      Value of the second throw of the die 6

   .. property:: public int D4Dice2 { get; set; }

      Value of the second throw of the die 4

.. class:: SelectedNextPlayer

   Event that is triggered at the end of a turn and choose the next player.

.. class:: SelectForestPowerEvent

   Event sent when a player comes in the Haunted Forest.

.. class:: SelectGiveCardEvent

   Event that allow to choose a target to give a card.

   .. property:: public int[] PossibleTargetId { get; set; }

      Array of Id's possible targets.

.. class:: SelectGiveOrWoundEvent

   Event that allows the player to choose to give an equipment or to take a wound.

.. class:: SelectLightCardTargetEvent

   Event that allow to choose on who the effect of the card is used

   .. property:: public int[] PossibleTargetId { get; set; }

      Array of Id's possible targets.

   .. property:: public Card LightCard { get; set; }

      The card.

.. class:: SelectMovement

   Event that gives available locations to the player.

   .. property::  public int[] LocationAvailable { get; set; }

      Array of possible locations.

.. class:: SelectPlayerTakingWoundsEvent

   Event that allow to choose a target in a list of players.

   .. property:: public int[] PossibleTargetId { get; set; }

      Array of Id's possible targets.

   .. property:: public int TargetID { get; set; } = -1;

      Id of a specific target.

   .. property:: public bool IsPuppet { get; set; }

      Boolean if the card that trigger the event is the Puppet.

   .. property:: public int NbWoundsTaken { get; set; }

      Number of wounds taken.

   .. property:: public int NbWoundsSelfHealed { get; set; }

      Number of wounds that are healed.

.. class:: SelectRevealOrNotEvent

   Event that allow the player to choose to reveal his character.

   .. property:: public Card EffectCard { get; set; } = null;

      The card that forces the player to reveal himself.

   .. property:: public bool PowerDaniel { get; set; } = false;

      Boolean if the power of Daniel is used.

   .. property:: public bool PowerLoup { get; set; } = false;

      Boolean if the power of Werewolf is used.

.. class:: SelectStealCardEvent

   Event that allow to choose a target in a list of players.

   .. property:: public (int equipment, int owner)[] PossiblePlayerTargetId { get; set; }

      Array of tuples composed of the Ids of the equipment and the possible targeted players.

.. class:: SelectStealCardFromPlayerEvent

   Event that allow to choose which equipment to steal to a selected target.

   .. property:: public int PlayerStealedId { get; set; }

      Id of the selected target.

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
