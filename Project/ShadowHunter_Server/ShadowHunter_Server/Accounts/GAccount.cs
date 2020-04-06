using System;
using EventSystem;
using ServerInterface.AuthEvents;
using ServerInterface.RoomEvents;
using ServerInterface.AuthEvents.event_in;
using ServerInterface.AuthEvents.event_out;
using System.Collections.Generic;
using Network.model;
//using System.Data.SQLite;

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
            Console.WriteLine("Evènement d'authentification reçu.");
            //throw new NotImplementedException();

            if (e is LogInEvent logIn)
            {
                Console.WriteLine("Demande de connexion.");
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
                Console.WriteLine("Logout");
                Accounts.TryGetValue(logOut.GetSender(), out Account accountToLogOut);
                accountToLogOut.IsLogged = false;
                Accounts.Remove(logOut.GetSender());
            }

            // si l'utilisateur veut s'enregistrer, on vérifie que son compte
            // n'existe pas, puis on le créé le cas échéant, et enfin on
            // connecte l'utilisateur
            else if (e is SignInEvent sie)
            {
                Console.WriteLine("Demande de création d'un compte.");
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
                    Console.WriteLine("Création du compte...");
                    if (CreateAccount(sie) == false)
                    {
                        sie.GetSender().Send(new AuthInvalidEvent() { Msg = "auth.account_creation_error;" });
                    }
                }
            }
        }
        // Vérifie si les identifiants envoyés par l'utilisateur correspondent
        // à un utilisateur déjà enregistré
        // Entrée : un login et un mot de passe
        // Sortie : 0 si les identifiants correspondent à un utilisateur
        //              existant,
        //          1 si le mot de passe est invalide,
        //          2 si l'utilisateur
        //              n'existe pas
        private byte Authentify(string login, string password)
        {
            /*string connectionString = @"DataSource=..\..\database.db; Version=3;";
            int myUserId = -1;

            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            using (SQLiteCommand command = new SQLiteCommand(conn))
            {
                conn.Open();
                command.CommandText = "select id from joueur where login_id = '" + login + "'";
                SQLiteDataReader lecteur = command.ExecuteReader();
                if (!lecteur.HasRows)
                {
                    lecteur.Close();
                    conn.Close();
                    return 2;
                }
                while (lecteur.Read())
                    myUserId = lecteur.GetInt32(0);
                lecteur.Close();
                

                command.CommandText = "select id from joueur where pwd = '" + password + "'";
                lecteur = command.ExecuteReader();
                if (!lecteur.HasRows)
                {
                    lecteur.Close();
                    conn.Close();
                    return 1;
                }
                while (lecteur.Read())
                {
                    if (myUserId == lecteur.GetInt32(0))
                    {
                        conn.Close();
                        return 0;
                    }
                }

                lecteur.Close();
                conn.Close();*/
                return 1;
           /* }*/
        }

        // Demande à la BDD de créer un compte
        // Entrée : un login et un mot de passe
        // Sortie : true si le compte a été créé, false sinon
        private bool CreateAccount(SignInEvent sie)
        {
            /*string connectionString = @"DataSource=..\..\database.db; Version=3;";
            string login = sie.Login;
            string password = sie.Password;

            using (SQLiteConnection conn = new SQLiteConnection(connectionString))
            using (SQLiteCommand command = new SQLiteCommand(conn))
            {
                conn.Open();
                command.CommandText = "insert into joueur (login_id, pwd) values ('" + login + "','" + password + "')";
                try
                {
                    command.ExecuteNonQuery();
                }
                catch (SQLiteException except)
                {
                    conn.Close();
                    Console.WriteLine("Erreur: " + except.Message);
                    return false;
                }

                Console.WriteLine("Compte crée");
                conn.Close();*/
                return true;
           /*}*/
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
            EventView.Manager.AddListener(Instance, true);
            Console.WriteLine("GAccount OK");
        }

    }
}