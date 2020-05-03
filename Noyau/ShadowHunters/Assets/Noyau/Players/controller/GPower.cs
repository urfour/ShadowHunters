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
                // lorsque le pouvoir est lancé
                List<int> targatable = new List<int>();
                foreach (Player p in PlayerView.GetPlayers())
                {
                    if (!p.Dead.Value && p != owner)
                    {
                        targatable.Add(p.Id);
                    }
                }
                EventView.Manager.Emit(new SelectAttackTargetEvent() { PlayerId = owner.Id, PossibleTargetId = targatable.ToArray(), PowerGeorges = true, });
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
                // lorsque le pouvoir est lancé
                List<int> targatable = new List<int>();
                foreach (Player p in PlayerView.GetPlayers())
                {
                    if (!p.Dead.Value && p != owner)
                    {
                        targatable.Add(p.Id);
                    }
                }
                EventView.Manager.Emit(new SelectAttackTargetEvent() { PlayerId = owner.Id, PossibleTargetId = targatable.ToArray(), PowerFranklin = true, });
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
        public static Power Emi = new Power
            (
            power: (owner) =>
            {
                // Si Emi se TP elle ne peut pas en plus se déplacer normalement
                GameManager.MovementAvailable.Value = false;

                int position = owner.Position.Value;
                int choice1 = (position + 1) % 6;
                int choice2 = (position - 1) % 6 ;

                /*Position[] availableDestination = new Position[]
                {
                    GameManager.Board.ElementAt(choice1).Value,
                    GameManager.Board.ElementAt(choice2).Value
                };*/

                int[] availableDestination = new int[]
                {
                    choice1,
                    choice2
                };

                EventView.Manager.Emit(new SelectMovement() { LocationAvailable = availableDestination, PlayerId = owner.Id });

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
                EventView.Manager.Emit(new SelectAttackTargetEvent() { PlayerId = owner.Id, TargetID = owner.OnAttacked.Value, PowerLoup = true, });
                // empêche le spam du pouvoir
                owner.CanUsePower.Value = false;
            },
            addListeners: (owner) =>
            {
                owner.OnAttacked.AddListener((sender) => { owner.Character.power.availability(owner); });
                GameManager.StartOfTurn.AddListener((sender) => { owner.Character.power.availability(owner); });
            },
            availability: (owner) =>
            {
                // Si le joueur attaquant met fin à son tour, dès le début du suivant on coupe le pouvoir
                bool start = GameManager.StartOfTurn.Value;
                if (start)
                {
                    if (owner.CanUsePower.Value)
                        owner.CanUsePower.Value = false;
                }
                else // Si la raison de l'appel vient de OnAttacked (donc pas start)
                {
                    // Si le loup n'est pas révélé, on lui propose de se révéler
                    if (!owner.Revealed.Value)
                    {
                        EventView.Manager.Emit(new SelectRevealOrNotEvent() { PlayerId = owner.Id, PowerLoup = true });
                    }
                    // Si le loup est révélé, il peut utiliser son pouvoir
                    else if (!owner.CanUsePower.Value)
                    {
                        owner.CanUsePower.Value = true;
                    }
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
                EventView.Manager.Emit(new SelectPlayerTakingWoundsEvent() { PlayerId = owner.Id, TargetID = owner.Id, NbWoundsTaken = 2, });
                EventView.Manager.Emit(new SelectAttackTargetEvent() { PlayerId = owner.Id, TargetID = owner.OnAttacking.Value, });
                // empêche le spam du pouvoir
                owner.CanUsePower.Value = false;
            },
            addListeners: (owner) =>
            {
                owner.OnAttacking.AddListener((sender) => { owner.Character.power.availability(owner); });
                GameManager.StartOfTurn.AddListener((sender) => { owner.Character.power.availability(owner); });
            },
            availability: (owner) =>
            {
                //Après la fin de son tour, le pouvoir n'est plus dispo
                bool start = GameManager.StartOfTurn.Value;
                if (start && owner.CanUsePower.Value)
                {
                    owner.CanUsePower.Value = false;
                }

                // Si la raison de l'appel vient de OnAttacking (donc pas start) et que Charles est révélé, on active le pouvoir
                bool available = !start && owner.Revealed.Value;
                if (!owner.CanUsePower.Value && available)
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
                EventView.Manager.Emit(new SelectRevealOrNotEvent() { PlayerId = owner.Id ,  PowerDaniel = true});
            },
            addListeners: (owner) =>
            {
                foreach (Player p in PlayerView.GetPlayers())
                {
                    if (p.Id != owner.Id)
                        p.Dead.AddListener((sender) => { owner.Character.goal.checkWinning(owner); });
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

