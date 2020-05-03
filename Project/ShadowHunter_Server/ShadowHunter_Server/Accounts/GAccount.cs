using System;
using EventSystem;
using ServerInterface.AuthEvents;
using ServerInterface.RoomEvents;
using System.Collections.Generic;
using Network.model;
using ShadowHunter_Server.Rooms;
using Network.controller;
using System.Threading;
using System.Data.SQLite;


namespace ShadowHunter_Server.Accounts
{
    class GAccount : IListener<AuthEvent>
    {

        public static GAccount Instance { get; private set; } = null;

        public Dictionary<string, Client> ConnectedAccounts { get; private set; } = 
            new Dictionary<string, Client>();
        public Mutex accounts_mutex = new Mutex();

        /*private Dictionary<string, Account> logins { get; set; } =
            new Dictionary<string, Account>();
        private Dictionary<string, string> passwords { get; set; } =
            new Dictionary<string, string>();*/


        // Régit le comportement du serveur en fonction de l'évènement
        // d'authentification reçu
        // Entrée : - Évenement héritant de AuthEvent
        //          - Liste des tags
        public void OnEvent(AuthEvent e, string[] tags = null)
        {
            //UnityEngine.Debug.Log("AuthTestEmul : " + e);
            accounts_mutex.WaitOne();
            if (e is LogInEvent lie)
            {
                if (e.GetSender().Account != null)
                {
                    e.GetSender().Send(new AuthInvalidEvent() { Msg = "message.auth.invalid.already_logged" });
                }
                else {
                    switch (Authentify(lie.Account.Login, lie.Password))
                    {
                        // si l'utilisateur n'existe pas
                        case 2:
                            e.GetSender().Send(new AuthInvalidEvent() { Msg = "message.auth.invalid.login.login_invalid" });
                            break;

                        // si le mot de passe n'est pas bon
                        case 1:
                            e.GetSender().Send(new AuthInvalidEvent() { Msg = "message.auth.invalid.login.password_invalid" });
                            break;

                        // si les identifiants sont valides
                        case 0:
                            if (ConnectedAccounts.ContainsKey(lie.Account.Login))
                            {
                                e.GetSender().Send(new AuthInvalidEvent() { Msg = "message.auth.invalid.account_already_online" });
                            }
                            else
                            {
                                lie.Account.IsLogged = true;
                                ConnectedAccounts.Add(lie.Account.Login, e.GetSender());
                                e.GetSender().Account = lie.Account;
                                e.GetSender().Send(new AssingAccountEvent(lie.Account));
                                GRoom.Instance.AddClient(e.GetSender());
                            }
                            break;
                    }
                }

                

            }
            else if (e is LogOutEvent loe)
            {
                // set IsLogged to false
                if (e.GetSender().Account != null)
                {
                    e.GetSender().Account.IsLogged = false;
                    ConnectedAccounts.Remove(e.GetSender().Account.Login);
                    e.GetSender().Account = null;
                }
                e.GetSender().Send(new AssingAccountEvent(null));
                //GRoom.Instance.RemoveClient(e.GetSender());

            }
            else if (e is SignInEvent sie)
            {
                // on ne peut créer un compte que si le login est disponible
                if (Authentify(sie.Login, sie.Password) != 2)
                {
                    e.GetSender().Send(new AuthInvalidEvent() { Msg = "message.auth.invalid.signin.login_unavailable&" + sie.Login });
                }
                else if (e.GetSender().Account != null)
                {
                    e.GetSender().Send(new AuthInvalidEvent() { Msg = "message.auth.invalid.already_logged" });
                }
                else
                {
                    switch (CreateAccount(sie))
                    {
                        case false:
                            e.GetSender().Send(new AuthInvalidEvent() { Msg = "message.auth.invalid.cant_create_account" });
                            break;

                        case true:

                            Account a = new Account()
                            {
                                Login = sie.Login,
                                IsLogged = true
                            };

                            ConnectedAccounts.Add(a.Login, e.GetSender());
                            e.GetSender().Account = a;
                            e.GetSender().Send(new AssingAccountEvent(a));
                            GRoom.Instance.AddClient(e.GetSender());
                            break;
                    }
                }

                
            }
            else if (e is AskAccountEvent aae)
            {
                if(Authentify(aae.Login, null) == 2 )
                {
                    e.GetSender().Send(new AuthInvalidEvent() { Msg = "message.auth.invalid.asked_account_dont_exists"});
                }
                else
                {
                    Account compte = new Account();
                    compte.Login = aae.Login;
                    compte.IsLogged = false;
                    // on vérifie que le compte n'est pas déjà connecté
                    if (ConnectedAccounts.ContainsKey(compte.Login)) compte.IsLogged = true;
                    e.GetSender().Send(new AccountDataEvent() { Account = compte });
                }
            }
            accounts_mutex.ReleaseMutex();
        }
        

        public void OnClientDisconnect(Client c)
        {
            if (c.Account != null && ConnectedAccounts.ContainsKey(c.Account.Login))
            {
                c.Account.IsLogged = false;
                ConnectedAccounts.Remove(c.Account.Login);
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
            string connectionString = @"DataSource=../../database.db; Version=3;";
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
                    command.Dispose();
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
                    command.Dispose();
                    conn.Close();
                    return 1;
                }
                while (lecteur.Read())
                {
                    if (myUserId == lecteur.GetInt32(0))
                    {
                        lecteur.Close();
                        conn.Close();
                        command.Dispose();
                        return 0;
                    }
                }

                lecteur.Close();
                command.Dispose();
                conn.Close();
                return 1;
            }
        }

        // Demande à la BDD de créer un compte
        // Entrée : un login et un mot de passe
        // Sortie : true si le compte a été créé, false sinon
        private bool CreateAccount(SignInEvent sie)
        {
            string connectionString = @"DataSource=..\..\database.db; Version=3;";
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
                    command.Dispose();
                    conn.Close();
                    Console.WriteLine("Erreur: " + except.Message);
                    return false;
                }

                Console.WriteLine("Compte crée");
                command.Dispose();
                conn.Close();
                return true;
           }
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
            GAccount a = new GAccount();
            EventView.Manager.AddListener(Instance);
            Client.OnConnect.Add(
                (sender) =>
                {
                    ((Client)sender).OnDisconnect.AddListener(
                            (sender2) =>
                            {
                                a.OnClientDisconnect((Client)sender);
                            });
                });
            Console.WriteLine("GAccount OK");
        }

    }
}