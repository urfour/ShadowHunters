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
        STARTTURN,
        MOVEON,
        DRAWCARD,
        GIVEVISION,
        DEALWOUNDS,
        HEALWOUNDS,
        ATTACK,
        REVEAL,
        USEPOWER,
        DIE
    }
    public class KernelLog : ListenableObject
    {
        public static KernelLog Instance { get; private set; }
        public List<(KernelLogType type, string msg)> Messages { get; private set; } = new List<(KernelLogType, string)>();

        public KernelLog()
        {
            Instance = this;
        }

        void StartTurn()
        {
            Messages.Add((KernelLogType.STARTTURN, "kernel.log.startturn&" + GameManager.PlayerTurn.Value.Name));
            Notify();
        }
        void MoveOn(Position position)
        {
            Messages.Add((KernelLogType.MOVEON, "kernel.log.moveon&" + GameManager.PlayerTurn.Value.Name + "&" + GameManager.Board[GameManager.PlayerTurn.Value.Position.Value]));
            Notify();
        }
        void DrawCard(int playerId, int cardId, bool isHidden)
        {
            if (isHidden)
            {
                Messages.Add((KernelLogType.DRAWCARD, "kernel.log.drawcardhidden&" + PlayerView.GetPlayer(playerId).Name));
            }
            else
            {
                Messages.Add((KernelLogType.DRAWCARD, "kernel.log.drawcard&" + PlayerView.GetPlayer(playerId).Name + "&" + Language.Translate(CardView.GetCard(cardId).cardLabel)));
            }
            Notify();
        }
        void GiveVision(int playerSenderId, int playerReceiverId)
        {
            Messages.Add((KernelLogType.GIVEVISION, "kernel.log.givevision&" + PlayerView.GetPlayer(playerSenderId).Name + "&" + PlayerView.GetPlayer(playerReceiverId).Name));
            Notify();
        }
        void DealWounds(int playerId, int wounds)
        {
            Messages.Add((KernelLogType.DEALWOUNDS, "kernel.log.dealwounds&" + PlayerView.GetPlayer(playerId).Name + "&" + wounds));
            Notify();
        }
        void HealWounds(int playerId, int wounds)
        {
            Messages.Add((KernelLogType.HEALWOUNDS, "kernel.log.healwounds&" + PlayerView.GetPlayer(playerId).Name + "&" + wounds));
            Notify();
        }
        void Attack(int attackerPlayerId, int attackedPlayerId, int wounds)
        {
            Messages.Add((KernelLogType.ATTACK, "kernel.log.attack&" + PlayerView.GetPlayer(attackerPlayerId).Name + "&" + PlayerView.GetPlayer(attackedPlayerId).Name + wounds));
            Notify();
        }
        void Reveal(int playerId)
        {
            Messages.Add((KernelLogType.REVEAL, "kernel.log.reveal&" + PlayerView.GetPlayer(playerId).Name + "&" + PlayerView.GetPlayer(playerId).Character.characterName));
            Notify();
        }
        void UsePower(int playerId)
        {
            Messages.Add((KernelLogType.USEPOWER, "kernel.log.usepower&" + PlayerView.GetPlayer(playerId).Name));
            Notify();
        }
        void Die(int playerId)
        {
            if (!PlayerView.GetPlayer(playerId).Revealed.Value)
            {
                Messages.Add((KernelLogType.DIE, "kernel.log.diereveal&" + PlayerView.GetPlayer(playerId).Name + "&" + PlayerView.GetPlayer(playerId).Character.characterName));
            }
            else
            {
                Messages.Add((KernelLogType.DIE, "kernel.log.die&" + PlayerView.GetPlayer(playerId).Name));
            }
            Notify();
        }
    }
}