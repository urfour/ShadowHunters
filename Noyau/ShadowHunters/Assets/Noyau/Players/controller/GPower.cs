using Assets.Noyau.Manager.view;
using Assets.Noyau.Players.model;
using Assets.Noyau.Players.view;
using EventSystem;
using Scripts.event_out;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Noyau.Players.controller
{
    class GPower
    {

        // exemple de George à faire pour tous les autres
        public static Power George = new Power
            (
            power: (owner) =>
            {
                // lorsque le pouvoir est lancé
                List<int> targatable = new List<int>();
                foreach (Player p in PlayerView.GetPlayers())
                {
                    if (!p.Dead.Value && p != owner)
                    {
                        targatable.Add(p.Id);
                    }
                }
                EventView.Manager.Emit(new SelectAttackTargetEvent() { PossibleTargetId = targatable.ToArray(), PowerGeorges = true, });
            },
            addListeners: (owner) =>
            {
                // initialisation des listeners qui appeleront availability
                GameManager.PlayerTurn.AddListener((sender) => { owner.Character.power.availability(owner); });
                GameManager.StartOfTurn.AddListener((sender) => { owner.Character.power.availability(owner); });
                owner.Revealed.AddListener((sender) => { owner.Character.power.availability(owner); });
            },
            availability: (owner) =>
            {
            // fonction qui test si le pouvoir peut être utilisé
                bool available = GameManager.PlayerTurn.Value == owner && GameManager.StartOfTurn.Value && owner.Revealed.Value;
                if (owner.CanUsePower.Value != available)
                {
                    owner.CanUsePower.Value = available;
                }
            }
            );
    }
}
