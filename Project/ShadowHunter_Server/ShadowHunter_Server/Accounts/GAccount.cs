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
    class GAccount : IListener<AuthEvent>
    {

        public static GAccount Instance { get; private set; } = null;

        public Dictionary<Client, Account> Accounts { get; private set; } = 
            new Dictionary<Client, Account>();


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
                byte loginStatus = Authentify(logIn.Account.Login, 
                    logIn.Password);
                switch (loginStatus)
                {
                    // identifiants corrects
                    case 0:
                        logIn.Account.IsLogged = true;
                        Accounts.Add(logIn.GetSender(), logIn.Account);
                        logIn.GetSender().Send(new AssingAccountEvent(
                            logIn.Account, "credentials_ok"));
                        break;

                    // mauvais mot de passe
                    case 1:
                        logIn.GetSender().Send(new AuthInvalidEvent()
                            { Msg = "auth.wrong_password;" });
                        break;
                    
                    // login inexistant
                    case 2:
                        logIn.GetSender().Send(new AuthInvalidEvent()
                            { Msg = "auth.inexistent_login; "
                                 + logIn.Account.Login });
                        break;

                    default:
                        logIn.GetSender().Send(new AuthInvalidEvent());
                        break;
                }
            }

            // si le client demande à se déconnecter, on le retire
            // du dictionnaire contenant les comptes actifs
            else if (e is LogOutEvent logOut)
            {
                Console.Write("Logout");
                Accounts.TryGetValue(logOut.GetSender(), out Account accountToLogOut);
                accountToLogOut.IsLogged = false;
                Accounts.Remove(logOut.GetSender());
            }

            // si l'utilisateur veut s'enregistrer, on vérifie que son compte
            // n'existe pas, puis on le créé le cas échéant, et enfin on
            // connecte l'utilisateur
            else if (e is SignInEvent sie)
            {
                // on vérifie que l'évènement contient bien les informations nécessaires 
                if (sie.Login == null || sie.Password == null)
                {
                    sie.GetSender().Send(new AuthInvalidEvent() { Msg = "auth.null_event;" });
                }

                // si le compte à créer existe déjà -> erreur
                else if (Authentify(sie.Login, sie.Password) != 2)
                {
                    sie.GetSender().Send(new AuthInvalidEvent() { Msg = "auth.login_unavailable;" + sie.Login });
                }

                else
                {
                    Account a = new Account()
                    {
                        Login = sie.Login,
                        IsLogged = true
                    };
                    Accounts.Add(sie.GetSender(), a);
                    if (CreateAccount(sie) == false)
                    {
                        sie.GetSender().Send(new AuthInvalidEvent() { Msg = "auth.account_creation_error;" });
                    }
                }
            }
        }
        // Vérifie si les identifiants envoyés par l'utilisateur correspondent
        // à un utilisateur déjà enregistré
        // Entrée : évènement LogInEvent contenant un objet Account 
        //          et un Password
        // Sortie : 0 si les identifiants correspondent à un utilisateur
        //              existant,
        //          1 si le mot de passe est invalide,
        //          2 si l'utilisateur
        //              n'existe pas
        private byte Authentify(string login, string password)
        {
            /* TODO : vérifier les identifiants dans la BDD */
            return 2;
        }

        // Demande à la BDD de créer un compte
        // Entrée : un SignInEvent contenant un login et un mot de passe
        // Sortie : true si le compte a été créé, false sinon
        private bool CreateAccount(SignInEvent sie)
        {
            // TODO: créer un compte dans la BDD
            return false;
        }



        private GAccount()
        {
            if (Instance != null)
            {

            }
            else
            {
                Instance = this;
            }
        }

        public static void Init()
        {
            new GAccount();
            Console.Write("GAccount OK");
            EventView.Manager.AddListener(Instance, true);
        }

    }
}