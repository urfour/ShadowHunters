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

      Boolean to know if the player is Wirewolf or not.

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