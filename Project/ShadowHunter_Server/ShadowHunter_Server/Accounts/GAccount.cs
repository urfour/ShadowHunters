using System;
using EventSystem;
using ServerInterface.AuthEvents;
using ServerInterface.RoomEvents;
using ServerInterface.AuthEvents.event_in;
using ServerInterface.AuthEvents.event_out;
using System.Collections.Generic;
using Network.model;

namespace ShadowHunter_Server.Accounts
{
    internal class GAccount : IListener<AuthEvent>
    {

        public static GAccount Instance { get; private set; } = null;

        public Dictionary<Client, Account> Accounts { get; private set; } = new Dictionary<Client, Account>();

        // Régit le comportement du serveur en fonction de l'évènement
        // d'authentification reçu
        // Entrée : - Évenement héritant de AuthEvent
        //          - Liste des tags
        public void OnEvent(AuthEvent e, string[] tags = null)
        {
            //throw new NotImplementedException();

            if (e is LogInEvent logIn)
            {
                Console.Write("Login");
                if (Authentify(logIn) == true)
                {
                    // TODO: associer le client à son compte
                    logIn.Account.IsLogged = true;
                    Accounts.Add(logIn.GetSender(),logIn.Account);
                    logIn.GetSender().Send(new AssingAccountEvent(
                        logIn.Account));

                }
                else
                {
                    logIn.GetSender().Send(new AuthInvalidEvent());
                }
            }

            else if (e is LogOutEvent logOut){
                Console.Write("Logout");
                // TODO : supprimer 
                Accounts.Remove(logOut.GetSender());
               
            }

        }



        // Vérifie si les identifiants envoyés par l'utilisateur correspondent
        // à un utilisateur déjà enregistré
        // Entrée : évènement LogInEvent contenant un objet Account 
        //          et un Password
        // Sortie : true si les identifiants correspondent à un utilisateur
        //      existant, false sinon
        private bool Authentify(LogInEvent credentials)
        {
            /* TODO : vérifier les identifiants dans la BDD */
            return false;
        }


        internal GAccount()
        {
            if (Instance != null) Logger.Warning("GAccount replaced");
            Instance = this;
        }
    }
}
