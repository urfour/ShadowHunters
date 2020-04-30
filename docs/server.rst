======
Server
======

The server will transfer game events between the clients, and will receive authentification and room managing events from the clients. The server will answer by sending specific events.

.. default-domain:: csharp

Accounts
========

.. namespace:: Accounts

.. class:: GAccount

    .. inherits:: IListener<AuthEvent>

	Class used to listen for incoming AuthEvents, and to manage authentification.

    .. property :: public static GAccounts Instance { get; set; }
	
	Instanciate the account manager.

    .. property :: public Dictionary<Account, Client> ConnectedAccounts { get; set; }
	
	Dictionnary used to associate each connected Account to a Client.

    .. method :: public void OnEvent(AuthEvent e, string[] tags = null)
    
	Method controlling the behavior of the server depending on the received AuthEvent.

    .. method :: public void OnClientDisconnect(Client c)
    
	Remove the account of the disconnected client from the list of all ConnectedAccounts.

    .. method :: private byte Authentify(string login, string password)
    
	Asks the database if the given login/password pair match an existing user. Returns 0 if they match, 1 if the password is invalid, and 2 if they do not match at all.

    .. method :: private bool CreateAccount(SignInEvent sie)
    
	Asks the database to create a new account from the login and password stored in the SignInEvent. Returns true if the account has been create, false otherwise.

    .. method :: public static void Init()
    
	Initialize the Account manager.



EventSystem
===========

IO
==

Log
===

Network
=======

Properties
==========

Rooms
=====

.. namespace:: Rooms

.. class:: GRooms

    .. inherits:: IListener<RoomEvent>

	Class used to listen for incoming RoomEvents, and to manage authentification.

    .. property :: public static GRoom Instance { get; set; }
	
	Instanciates the room manager.

    .. property :: public Dictionary<int, Room> Rooms { get; set; }
	
	Dictionnary used to associate their code to each Room.

    .. method :: public void OnEvent(RoomEvent e, string[] tags = null)
    
	Method controlling the behavior of the server depending on the received RoomEvent.

    .. method :: public static void Init()
    
	Initializes the Account manager.

    .. method :: public void AddClient(Client c)
    
	Adds a client to the Global Room and adds a listener waiting for the disconnection of this user

    .. method :: public void RemoveClient(Client c)
    
	Removes the client from the Global Room.

    .. method :: public void OnClientDisconnect(Client c)
    
	Removes the client and his account from his room.

    .. method :: public void RemovePlayerFromRoom(Client c, RoomData r, bool notifyClient = true, string leaveMessage = null)
    
	Removes a Client from a Room.

ServerInterface
================


AuthEvents
----------

.. namespace:: ServerInterface.AuthEvents

.. class:: AuthEvent

	Class modelizing an account.

    .. property :: public string Login { get; set; }
	
	Login of the Account.
	
    .. property :: public bool IsLogged { get; set; }
	
	Boolean set to true if someone is logged to this Account, or to false otherwise.

.. class:: AuthEvent

    .. inherits:: ServerOnlyEvent
    
	Base authentification event, inherited by all other authentification events.


event_in
~~~~~~~~~


.. class:: AccountDataEvent

    .. inherits:: AuthEvent
    
	Event to give his Account's data to a client.
    
    .. property :: public Account Account { get; set; }
    
	The object containing the data of the account.
    

.. class:: AssingAccountEvent

    .. inherits:: AuthEvent
    
	Event to assign an Account to a client.
    
    .. property :: public Account Account { get; set; }
    
	The object containing the data of the account.
    

.. class:: AuthInvalidEvent

    .. inherits:: AuthEvent
    
	Event to report the failure of a request to a client.
    
event_out
~~~~~~~~~

.. class:: AskAccountEvent

    .. inherits:: AuthEvent
    
	Event to ask the server informations of an account.
    
    .. property :: public string Login { get; set; }
    
	Login of the account from which to get informations.
    

.. class:: LogInEvent

    .. inherits:: AuthEvent
    
	Event to ask the server to log the client in.
    
    .. property :: public Account Account { get; set; }
    
	The object containing the data of the account.
    
    .. property :: public string Password { get; set; }
    
	The password of the account to which to log in.
    

.. class:: LogOutEvent

    .. inherits:: AuthEvent
    
	Event to ask the server to log out the sending client.

.. class:: LogInEvent

    .. inherits:: AuthEvent
    
	Event to ask the server to create an account, and log the client in with the created account.
    
    .. property :: public Account Account { get; set; }
    
	The object containing the data of the account.
    
    .. property :: public string Password { get; set; }
    
	The password of the account to which to log in.   


RoomEvents
----------
.. namespace:: ServerInterface.RoomEvents

.. class:: RoomData

	This class countains all the informations about a Room.
	
    .. property :: public int Code { get; set; }
	
	An integer used to identify a Room. This field will be filled by the server: what the client will write here will not be used.
	
    .. property :: public int CurrentNbPlayer { get; set; }
	
	The number of players currently in the room. This field will be filled by the server: what the client will write here will not be used.
	
    .. property :: public bool IsSuppressed { get; set; }
	
	Boolean set to true if this Room is currently active, false otherwise. This field will be filled by the server: what the client will write here will not be used.
	
    .. property :: public bool IsLaunched { get; set; }
	
	Boolean set to true if a game is currently being played in this Room. This field will be filled by the server: what the client will write here will not be used.
	
    .. property :: public string[] Players { get; set; }

       A list of the logins of all the players in the Room. This field will be filled by the server: what the client will write here will not be used.
	
    .. property :: public bool[] ReadyPlayers { get; set; }
	
	A list of booleans associated with the Players property; if Players[1] is ready to play, then ReadyPlayers[1] will be set to true, false otherwise. This field will be filled by the server: what the client will write here will not be used.
	
    .. property :: public string Name { get; set; }
	
	The name of the Room.
	
    .. property :: public int MaxNbPlayer { get; set; }
	
	The maximum number of players a Room can accept.
	
    .. property :: public bool HasPassword { get; set; }
	
	A boolean set to true if the Room has a password, false otherwise.
	
    .. property :: public string Password { get; set; }
	
	The password of the Room.
	
	
.. namespace:: ServerInterface.RoomEvents

.. class:: RoomEvent

    .. inherits:: ServerOnlyEvent

	Basic event, from which inherits all events related to the rooms.
	
    .. property :: public RoomData RoomData { get; set; }
	
	The informations of this Room.
event_in
~~~~~~~~~

.. class:: RoomDataEvent

    .. inherits:: RoomEvent
    
	Event to inform all clients of the state of every Room.
    
.. class:: RoomFailureEvent

    .. inherits:: RoomEvent
    
	Event to notify a client of the failure of his request. 
    
.. class:: RoomJoinedEvent

    .. inherits:: RoomEvent

	Event to inform a client that he has been added to a Room.

.. class:: RoomLeavedEvent

    .. inherits:: RoomEvent

	Event to inform a client that he has been removed from a Room.

event_out
~~~~~~~~~    

.. class:: CreateRoomEvent

    .. inherits:: RoomEvent
    
	Event to ask the server to create a Room, using RoomData.
    
.. class:: JoinRoomEvent

    .. inherits:: RoomEvent
    
	Event ask the server to join a Room. 
    
.. class:: LeaveRoomEvent

    .. inherits:: RoomEvent
    
	Event ask the server to leave a Room.
    
.. class:: ModifyRoomEvent

    .. inherits:: RoomEvent

	Event to ask the server to modify a Room, using the new RoomData.

.. class:: ReadyEvent

    .. inherits:: RoomEvent

	Event to notify the server that the sending client is ready to play.
	
Settings
========
