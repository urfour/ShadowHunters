======
Server
======

The server will transfer game events between the clients, and will receive authentification and room managing events from the clients. The server will answer by sending specific events.

.. default-domain:: csharp

Accounts
========

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

ServerInterface
================


AuthEvents
----------

.. namespace:: ServerInterface.AuthEvents

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
    
    
Settings
========
