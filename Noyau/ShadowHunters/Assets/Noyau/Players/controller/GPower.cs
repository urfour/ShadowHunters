using Assets.Noyau.Manager.view;
using Assets.Noyau.Players.model;
using Assets.Noyau.Players.view;
using EventSystem;
using Scripts.event_out;
using Scripts.event_in;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Noyau.Players.controller
{
    class GPower
    {

        // HUNTERS
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

        public static Power Franklin = new Power
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
                EventView.Manager.Emit(new SelectAttackTargetEvent() { PossibleTargetId = targatable.ToArray(), PowerFranklin = true, });
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

        // en chantier
        public static Power Emi = new Power
            (
            power: (owner) =>
            {
                //TODO
            },
            addListeners: (owner) =>
            {
                // initialisation des listeners qui appeleront availability

                GameManager.MovementAvailable.AddListener((sender) => { owner.Character.power.availability(owner); });
            },
            availability: (owner) =>
            {
                // fonction qui test si le pouvoir peut être utilisé
                bool available = GameManager.PlayerTurn.Value == owner && GameManager.MovementAvailable.Value && owner.Revealed.Value;
                if (owner.CanUsePower.Value != available)
                {
                    owner.CanUsePower.Value = available;
                }
            }
            );

        //SHADOWS
        public static Power Loup = new Power
            (
            power: (owner) =>
            {
                EventView.Manager.Emit(new SelectAttackTargetEvent() { TargetID = owner.OnAttacked.Value, PowerLoup = true, });
            },
            addListeners: (owner) =>
            {
                owner.OnAttacked.AddListener((sender) => { owner.Character.power.availability(owner); });
                GameManager.StartOfTurn.AddListener((sender) => { owner.Character.power.availability(owner); });
            },
            availability: (owner) =>
            {
                bool start = GameManager.StartOfTurn.Value;
                if (start && owner.CanUsePower.Value)
                {
                    owner.CanUsePower.Value = false;
                    owner.OnAttacked.Value = -1;
                }

                bool available = owner.OnAttacked.Value != -1 && owner.Revealed.Value;
                if (owner.CanUsePower.Value != available)
                {
                    owner.CanUsePower.Value = available;
                }
            }
            );

        public static Power Vampire = new Power
            (
            power: (owner) =>
            {
                //TODO
            },
            addListeners: (owner) =>
            {
                //TODO
            },
            availability: (owner) =>
            {
                //TODO
            }
            );

        public static Power Metamorphe = new Power
            (
            power: (owner) =>
            {
                //TODO
            },
            addListeners: (owner) =>
            {
                //TODO
            },
            availability: (owner) =>
            {
                //TODO
            }
            );

        //NEUTRES
        public static Power Allie = new Power
            (
            power: (owner) =>
            {
                //TODO
            },
            addListeners: (owner) =>
            {
                //TODO
            },
            availability: (owner) =>
            {
                //TODO
            }
            );

        public static Power Bob = new Power
            (
            power: (owner) =>
            {
                //TODO
            },
            addListeners: (owner) =>
            {
                //TODO
            },
            availability: (owner) =>
            {
                //TODO
            }
            );

        public static Power Charles = new Power
            (
            power: (owner) =>
            {
                //TODO
            },
            addListeners: (owner) =>
            {
                //TODO
            },
            availability: (owner) =>
            {
                //TODO
            }
            );

        public static Power Daniel = new Power
            (
            power: (owner) =>
            {
                EventView.Manager.Emit(new RevealCard() { PlayerId = owner.Id , });
            },
            addListeners: (owner) =>
            {
                foreach (Player p in PlayerView.GetPlayers())
                {
                    p.Dead.AddListener((sender) => { owner.Character.goal.checkWinning(owner); });
                }
            },
            availability: (owner) =>
            {
                if (!owner.Dead.Value && !owner.Revealed.Value)
                {
                    owner.Character.power.power(owner);
                }
            }
            );

    }
}

