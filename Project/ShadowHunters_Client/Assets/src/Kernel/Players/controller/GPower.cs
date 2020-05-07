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
using Assets.Noyau.Cards.view;
using Log;

namespace Assets.Noyau.Players.controller
{
    /// <summary>
    /// Classe qui va instancier tout les pouvoirs des personnages.
    /// </summary>
    class GPower
    {
        // HUNTERS

        /// <summary>
        /// Fonction qui va instancier le pouvoir de Georges.
        /// </summary>
        /// <param name="power">Fonction de l'effet du pouvoir du personnage</param>
        /// <param name="addListeners">Fonction qui ajoute des Listeners uniquement sur les Setting concernés</param>
        /// <param name="availability">Fonction qui test quand le pouvoir est utilisable</param>
        public static Power George = new Power
            (
            power: (owner) =>
            {
                KernelLog.Instance.UsePower(owner);
                if (GameManager.LocalPlayer.Value == owner)
                {
                    EventView.Manager.Emit(new SelectUsableCardPickedEvent(CardView.GCard.GeorgesPower.Id, false, owner.Id));
                }
                owner.PowerUsed.Value = true;
            },
            addListeners: (owner) =>
            {
                // initialisation des listeners qui appeleront availability
                GameManager.StartOfTurn.AddListener((sender) => { owner.Character.power.availability(owner); });
                owner.Revealed.AddListener((sender) => { owner.Character.power.availability(owner); });
                owner.PowerUsed.AddListener((sender) => { owner.Character.power.availability(owner); });
            },
            availability: (owner) =>
            {
                // fonction qui test si le pouvoir peut être utilisé
                bool available = GameManager.PlayerTurn.Value == owner && GameManager.StartOfTurn.Value && owner.Revealed.Value && !owner.PowerUsed.Value;
                if (owner.CanUsePower.Value != available)
                {
                    owner.CanUsePower.Value = available;
                }
            }
            );

        /// <summary>
        /// Fonction qui va instancier le pouvoir de Franklin.
        /// </summary>
        /// <param name="power">Fonction de l'effet du pouvoir du personnage</param>
        /// <param name="addListeners">Fonction qui ajoute des Listeners uniquement sur les Setting concernés</param>
        /// <param name="availability">Fonction qui test quand le pouvoir est utilisable</param>
        public static Power Franklin = new Power
            (
            power: (owner) =>
            {
                KernelLog.Instance.UsePower(owner);
                if (GameManager.LocalPlayer.Value == owner)
                {
                    EventView.Manager.Emit(new SelectUsableCardPickedEvent(CardView.GCard.FranklinPower.Id, false, owner.Id));
                }
                owner.PowerUsed.Value = true;
            },
            addListeners: (owner) =>
            {
                // initialisation des listeners qui appeleront availability
                GameManager.StartOfTurn.AddListener((sender) => { owner.Character.power.availability(owner); });
                owner.Revealed.AddListener((sender) => { owner.Character.power.availability(owner); });
                owner.PowerUsed.AddListener((sender) => { owner.Character.power.availability(owner); });
            },
            availability: (owner) =>
            {
                // fonction qui test si le pouvoir peut être utilisé
                bool available = GameManager.PlayerTurn.Value == owner && GameManager.StartOfTurn.Value && owner.Revealed.Value && !owner.PowerUsed.Value;
                if (owner.CanUsePower.Value != available)
                {
                    owner.CanUsePower.Value = available;
                }
            }
            );


        //SHADOWS

        /// <summary>
        /// Fonction qui va instancier le pouvoir du Loup-Garou.
        /// </summary>
        /// <param name="power">Fonction de l'effet du pouvoir du personnage</param>
        /// <param name="addListeners">Fonction qui ajoute des Listeners uniquement sur les Setting concernés</param>
        /// <param name="availability">Fonction qui test quand le pouvoir est utilisable</param>
        public static Power Loup = new Power
            (
            power: (owner) =>
            {
                if (owner.CanUsePower.Value)
                {
                    owner.CanUsePower.Value = false;
                    if (GameManager.LocalPlayer.Value == owner)
                    {
                        EventView.Manager.Emit(new AttackPlayerEvent() { PlayerId = owner.Id, PlayerAttackedId = owner.OnAttackedBy.Value, PowerLoup = true, });
                    }
                }
            },
            addListeners: (owner) =>
            {
                owner.OnAttackedBy.AddListener((sender) => { owner.Character.power.availability(owner); });
                owner.Revealed.AddListener((sender) => { owner.Character.power.availability(owner); });
                GameManager.StartOfTurn.AddListener((sender) => { owner.Character.power.availability(owner); });
                owner.Dead.AddListener((sender) => { owner.Character.power.availability(owner); });
            },
            availability: (owner) =>
            {
                if (owner.Dead.Value)
                    owner.CanUsePower.Value = false;

                if (owner.Revealed.Value && !owner.Dead.Value)
                {
                    if (GameManager.StartOfTurn.Value)
                    {
                        if (owner.OnAttackedBy.Value != -1)
                        {
                            owner.OnAttackedBy.Value = -1;
                        }
                        if (owner.CanUsePower.Value != false)
                        {
                            owner.CanUsePower.Value = false;
                        }
                    }
                    else
                    {
                        if (owner.OnAttackedBy.Value != -1 && owner.CanUsePower.Value == false)
                        {
                            owner.CanUsePower.Value = true;
                        }
                    }
                }
            });

        /// <summary>
        /// Fonction qui va instancier le pouvoir du Vampire.
        /// </summary>
        /// <param name="power">Fonction de l'effet du pouvoir du personnage</param>
        /// <param name="addListeners">Fonction qui ajoute des Listeners uniquement sur les Setting concernés</param>
        /// <param name="availability">Fonction qui test quand le pouvoir est utilisable</param>
        public static Power Vampire = new Power
            (
            power: (owner) =>
            {
                owner.Healed(2);
            },
            addListeners: (owner) =>
            {
                owner.DamageDealed.AddListener((sender) => { owner.Character.power.availability(owner); });
            },
            availability: (owner) =>
            {
                if(owner.Revealed.Value && owner.DamageDealed.Value > 0)
                {
                    owner.Character.power.power(owner);
                }
            }
            );

        //NEUTRES

        /// <summary>
        /// Fonction qui va instancier le pouvoir d'Allie.
        /// </summary>
        /// <param name="power">Fonction de l'effet du pouvoir du personnage</param>
        /// <param name="addListeners">Fonction qui ajoute des Listeners uniquement sur les Setting concernés</param>
        /// <param name="availability">Fonction qui test quand le pouvoir est utilisable</param>
        public static Power Allie = new Power
            (
            power: (owner) =>
            {
                KernelLog.Instance.UsePower(owner);
                //Elle ne pourra plus jamais l'utiliser
                owner.CanUsePower.Value = false;

                owner.Healed(owner.Wound.Value);
            },
            addListeners: (owner) =>
            {
                owner.Revealed.AddListener((sender) => { owner.Character.power.availability(owner); });
            },
            availability: (owner) =>
            {
                // A partir du moment où elle se révèle, elle peut utiliser son pouvoir n'importe quand (mais pas forcément immédiatement)
                if(owner.Revealed.Value)
                {
                    owner.CanUsePower.Value = true;
                }
            }
            );

        /// <summary>
        /// Fonction qui va instancier le pouvoir de Bob.
        /// </summary>
        /// <param name="power">Fonction de l'effet du pouvoir du personnage</param>
        /// <param name="addListeners">Fonction qui ajoute des Listeners uniquement sur les Setting concernés</param>
        /// <param name="availability">Fonction qui test quand le pouvoir est utilisable</param>
        public static Power Bob = new Power
            (
            power: (owner) =>
            {
                if (GameManager.LocalPlayer.Value == owner)
                {
                    EventView.Manager.Emit(new SelectUsableCardPickedEvent(CardView.GCard.BobPower.Id, false, owner.Id));
                }
            },
            addListeners: (owner) =>
            {
                owner.DamageDealed.AddListener((sender) => { owner.Character.power.availability(owner); });
            },
            availability: (owner) =>
            {
                if(owner.Revealed.Value && owner.DamageDealed.Value >= 2)
                {
                    owner.Character.power.power(owner);
                }
            }
            );

        /// <summary>
        /// Fonction qui va instancier le pouvoir de Charles.
        /// </summary>
        /// <param name="power">Fonction de l'effet du pouvoir du personnage</param>
        /// <param name="addListeners">Fonction qui ajoute des Listeners uniquement sur les Setting concernés</param>
        /// <param name="availability">Fonction qui test quand le pouvoir est utilisable</param>
        public static Power Charles = new Power
            (
            power: (owner) =>
            {
                owner.Wounded(2, owner, false);
                if (GameManager.LocalPlayer.Value == owner)
                {
                    EventView.Manager.Emit(new AttackPlayerEvent() { PlayerId = owner.Id, PlayerAttackedId = owner.OnAttackingPlayer.Value, PowerCharles = true, });
                }
                // empêche le spam du pouvoir
                owner.OnAttacking.Value = false;
                owner.CanUsePower.Value = false;
            },
            addListeners: (owner) =>
            {
                owner.OnAttacking.AddListener((sender) => { owner.Character.power.availability(owner); });
                owner.Revealed.AddListener((sender) => { owner.Character.power.availability(owner); });
                GameManager.StartOfTurn.AddListener((sender) => { owner.Character.power.availability(owner); });
            },
            availability: (owner) =>
            {
                //Après la fin de son tour, le pouvoir n'est plus dispo
                if (GameManager.StartOfTurn.Value)
                {
                    owner.OnAttacking.Value = false;
                    owner.CanUsePower.Value = false;
                }

                // Si la raison de l'appel vient de OnAttacking (donc pas start) et que Charles est révélé, on active le pouvoir
                else if (owner.OnAttacking.Value && owner.Revealed.Value)
                {
                    owner.CanUsePower.Value = true;
                }
            }
            );

        /// <summary>
        /// Fonction qui va instancier le pouvoir de Daniel.
        /// </summary>
        /// <param name="power">Fonction de l'effet du pouvoir du personnage</param>
        /// <param name="addListeners">Fonction qui ajoute des Listeners uniquement sur les Setting concernés</param>
        /// <param name="availability">Fonction qui test quand le pouvoir est utilisable</param>
        public static Power Daniel = new Power
            (
            power: (owner) =>
            {
                owner.Revealed.Value = true;
            },
            addListeners: (owner) =>
            {
                foreach (Player p in PlayerView.GetPlayers())
                {
                    if (p.Id != owner.Id)
                        p.Dead.AddListener((sender) => { owner.Character.power.availability(owner); });
                }
            },
            availability: (owner) =>
            {
                // A ce moment Daniel est forcément vivant car sinon il aurait gagné
                if (!owner.Revealed.Value)
                {
                    owner.Character.power.power(owner);
                }
            }
            );
    }
}

