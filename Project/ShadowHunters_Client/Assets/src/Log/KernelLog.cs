using Assets.Noyau.Cards.view;
using Assets.Noyau.Manager.view;
using Assets.Noyau.Players.view;
using EventSystem;
using Lang;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Log
{
    public enum KernelLogType
    {
        NOTHINGHAPPEN,
        STARTTURN,
        MOVEON,
        DRAWCARD,
        GIVEEQUIPMENT,
        STEALEQUIPMENT,
        GIVEVISION,
        DEALWOUNDS,
        HEALWOUNDS,
        ATTACK,
        ATTACKFAILED,
        REVEAL,
        USEPOWER,
        DIE,
        REPLAY,
        DEFINE
    }
    public class KernelLog : ListenableObject
    {
        public static KernelLog Instance { get; private set; }
        public List<(KernelLogType type, string msg)> Messages { get; private set; } = new List<(KernelLogType, string)>();

        public KernelLog()
        {
            Instance = this;
        }

        public void NothingHappen()
        {
            Messages.Add((KernelLogType.NOTHINGHAPPEN, "kernel.log.nothinghappen"));
        }
        public void StartTurn()
        {
            Messages.Add((KernelLogType.STARTTURN, "kernel.log.startturn.args.playername&" + GameManager.PlayerTurn.Value.Name));
            Notify();
        }
        public void MoveOn()
        {
            Messages.Add((KernelLogType.MOVEON, "kernel.log.moveon.args.position&" + GameManager.PlayerTurn.Value.Name + "&" 
                         + Language.Translate("board.position." + GameManager.Board[GameManager.PlayerTurn.Value.Position.Value].ToString().ToLower())));
            Notify();
        }
        public void DrawCard(Player player, int cardId, bool isHidden)
        {
            if (isHidden)
            {
                Messages.Add((KernelLogType.DRAWCARD, "kernel.log.drawcard.args.playername&" + player.Name));
            }
            else
            {
                Messages.Add((KernelLogType.DRAWCARD, "kernel.log.drawcard.args.playername_cardlabel&" + player.Name + "&" + Language.Translate(CardView.GetCard(cardId).cardLabel)));
            }
            Notify();
        }
        public void GiveEquipement(Player playerGiver, Player playerGiven, int cardId)
        {
            Messages.Add((KernelLogType.GIVEEQUIPMENT, "kernel.log.giveequipement.args.playergiver_playergiven_playergiven_cardgiven&" +
                         playerGiver.Name + "&" + playerGiven.Name + "&" + Language.Translate(CardView.GetCard(cardId).cardLabel)));
            Notify();
        }
        public void StealEquipement(Player playerThief, Player playerStolen, int cardId)
        {
            Messages.Add((KernelLogType.STEALEQUIPMENT, "kernel.log.stealequipement.args.playerthief_playerstolen_cardgiven&" +
                         playerThief.Name + "&" + playerStolen.Name + "&" + Language.Translate(CardView.GetCard(cardId).cardLabel)));
            Notify();
        }
        public void GiveVision(Player playerSender, Player playerReceiver)
        {
            Messages.Add((KernelLogType.GIVEVISION, "kernel.log.givevision.args.playersender_playerreceiver&" + playerSender.Name + "&" + playerReceiver.Name));
            Notify();
        }
        public void DealWounds(Player player, int wounds)
        {
            Messages.Add((KernelLogType.DEALWOUNDS, "kernel.log.dealwounds.args.playername_wounds&" + player.Name + "&" + wounds));
            Notify();
        }
        public void HealWounds(Player player, int wounds)
        {
            Messages.Add((KernelLogType.HEALWOUNDS, "kernel.log.healwounds.args.playername_wounds&" + player.Name + "&" + wounds));
            Notify();
        }
        public void Attack(Player attackerPlayer, Player attackedPlayer, int wounds)
        {
            Messages.Add((KernelLogType.ATTACK, "kernel.log.attack.args.attackername_attackedname_wounds&" + attackerPlayer.Name + "&" + attackedPlayer.Name + "&" + wounds));
            Notify();
        }
        public void AttackFailed(Player attackerPlayer, Player attackedPlayer)
        {
            Messages.Add((KernelLogType.ATTACKFAILED, "kernel.log.attackfailed.args.attackername_attackedname&" + attackerPlayer.Name + "&" + attackedPlayer.Name));
            Notify();
        }
        public void Reveal(Player player)
        {
            Messages.Add((KernelLogType.REVEAL, "kernel.log.reveal.args.playername&" + player.Name + "&" + Language.Translate(player.Character.characterName)));
            Notify();
        }
        public void UsePower(Player player)
        {
            Messages.Add((KernelLogType.USEPOWER, "kernel.log.usepower.args.playername&" + player.Name));
            Notify();
        }
        public void Die(Player player)
        {
            if (!player.Revealed.Value)
            {
                Messages.Add((KernelLogType.DIE, "kernel.log.die.args.playername_charactername&" + player.Name + "&" + Language.Translate(player.Character.characterName)));
            }
            else
            {
                Messages.Add((KernelLogType.DIE, "kernel.log.die.args.playername&" + player.Name));
            }
            Notify();
        }
        public void Replay(Player player)
        {
            Messages.Add((KernelLogType.REPLAY, "kernel.log.replay.args.playername&" + player.Name));
            Notify();
        }

        public void DefineWounds(Player player, int wounds)
        {
            Messages.Add((KernelLogType.DEFINE, "kernel.log.define.args.playername_wounds&" + player.Name + "&" + wounds));
            Notify();
        }
    }
}