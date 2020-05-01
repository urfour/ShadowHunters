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
                EventView.Manager.Emit(new SelectUsableCardPickedEvent(CardView.GCard.GeorgesPower.Id, false, owner.Id));
                owner.PowerUsed.Value = true;
            },
            addListeners: (owner) =>
            {
                // initialisation des listeners qui appeleront availability
                GameManager.StartOfTurn.AddListener((sender) => { owner.Character.power.availability(owner); });
                owner.Revealed.AddListener((sender) => { owner.Character.power.availability(owner); });
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
                EventView.Manager.Emit(new SelectUsableCardPickedEvent(CardView.GCard.FranklinPower.Id, false, owner.Id));
                owner.PowerUsed.Value = true;
            },
            addListeners: (owner) =>
            {
                // initialisation des listeners qui appeleront availability
                GameManager.StartOfTurn.AddListener((sender) => { owner.Character.power.availability(owner); });
                owner.Revealed.AddListener((sender) => { owner.Character.power.availability(owner); });
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
        /// Fonction qui va instancier le pouvoir d'Emi.
        /// </summary>
        /// <param name="power">Fonction de l'effet du pouvoir du personnage</param>
        /// <param name="addListeners">Fonction qui ajoute des Listeners uniquement sur les Setting concernés</param>
        /// <param name="availability">Fonction qui test quand le pouvoir est utilisable</param>
        /*public static Power Emi = new Power
            (
            power: (owner) =>
            {
                // Si Emi se TP elle ne peut pas en plus se déplacer normalement
                GameManager.MovementAvailable.Value = false;

                List<int> availableDestination = new List<int>();

                if (owner.Position.Value % 2 == 0)
                    availableDestination.Add(owner.Position.Value + 1);
                else
                    availableDestination.Add(owner.Position.Value - 1);

                EventView.Manager.Emit(new SelectMovement() { LocationAvailable = availableDestination.ToArray(), PlayerId = owner.Id });

            },
            addListeners: (owner) =>
            {
                // initialisation des listeners qui appeleront availability

                GameManager.MovementAvailable.AddListener((sender) => { owner.Character.power.availability(owner); });
                owner.Revealed.AddListener((sender) => { owner.Character.power.availability(owner); });
            },
            availability: (owner) =>
            {
                // fonction qui test si le pouvoir peut être utilisé
                bool available = GameManager.PlayerTurn.Value == owner
                                && GameManager.MovementAvailable.Value
                                && owner.Revealed.Value
                                && owner.Position.Value != -1;

                if (owner.CanUsePower.Value != available)
                {
                    owner.CanUsePower.Value = available;
                }
            }
            );*/

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
                EventView.Manager.Emit(new SelectAttackTargetEvent() { PlayerId = owner.Id, TargetID = owner.OnAttackedAttacker.Value, PowerLoup = true, });
                // empêche le spam du pouvoir
                owner.CanUsePower.Value = false;
            },
            addListeners: (owner) =>
            {
                owner.OnAttacked.AddListener((sender) => { owner.Character.power.availability(owner); });
                owner.Revealed.AddListener((sender) => { owner.Character.power.availability(owner); });
                GameManager.StartOfTurn.AddListener((sender) => { owner.Character.power.availability(owner); });
            },
            availability: (owner) =>
            {
                // Si le joueur attaquant met fin à son tour, dès le début du suivant on coupe le pouvoir
                if (GameManager.StartOfTurn.Value)
                {
                    if (owner.CanUsePower.Value)
                        owner.CanUsePower.Value = false;

                    if (owner.OnAttacked.Value)
                        owner.OnAttacked.Value = false;
                }
                // Si la raison de l'appel vient de OnAttacked (donc pas start)
                else if (owner.OnAttacked.Value && owner.Revealed.Value)
                {
                    owner.CanUsePower.Value = true;
                    owner.OnAttacked.Value = false;
                }
            }
            );

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

/* pas besoin de le faire car déjà géré par les cartes visions
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
*/
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
                EventView.Manager.Emit(new SelectBobPowerEvent() { PlayerId = owner.Id});
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
                EventView.Manager.Emit(new SelectAttackTargetEvent() { PlayerId = owner.Id, TargetID = owner.OnAttackingPlayer.Value, });
                // empêche le spam du pouvoir
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

