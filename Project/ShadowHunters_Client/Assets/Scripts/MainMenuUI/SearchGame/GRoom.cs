﻿using EventSystem;
using ServerInterface.AuthEvents;
using ServerInterface.RoomEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.MainMenuUI.SearchGame
{
    class GRoom : ListenableObject, IListener<RoomEvent>
    {
        public static GRoom Instance { get; private set; } = null;

        public Dictionary<int, Room> Rooms { get; private set; } = new Dictionary<int, Room>();
        public Room JoinedRoom { get; private set; } = new Room();

        public void OnEvent(RoomEvent e, string[] tags = null)
        {
            if (e is RoomDataEvent rde)
            {
                if (rde.RoomData.IsSuppressed)
                {
                    if (Rooms.ContainsKey(rde.RoomData.Code)) Rooms.Remove(rde.RoomData.Code);
                }
                else
                {
                    if (Rooms.ContainsKey(rde.RoomData.Code))
                    {
                        Rooms[rde.RoomData.Code].ModifData(rde.RoomData);
                    }
                    else
                    {
                        Rooms.Add(rde.RoomData.Code, new Room(rde.RoomData));
                    }
                }
                if (JoinedRoom.Code.Value == rde.RoomData.Code)
                {
                    if (rde.RoomData.IsLaunched && !JoinedRoom.RawData.IsLaunched)
                    {
                        // lancement de la partie
                        JoinedRoom.ModifData(rde.RoomData);
                        SceneManagerComponent.InitBeforeScene(JoinedRoom);
                        SceneManagerComponent.LoadScene();
                    }
                    else
                    {
                        JoinedRoom.ModifData(rde.RoomData);
                    }
                }
            }
            else if (e is RoomJoinedEvent rje)
            {
                // TODO gestion serveur
                RoomData r = rje.RoomData;
                JoinedRoom.ModifData(r);
                if (Rooms.ContainsKey(r.Code))
                {
                    Rooms[r.Code].ModifData(r);
                }
                else
                {
                    Rooms.Add(r.Code, new Room(r));
                }
            }
            else if (e is RoomLeavedEvent rle)
            {
                JoinedRoom.ModifData(new RoomData() { Code=0 });
            }

            Notify();
        }

        public static void Init()
        {
            if (Instance != null)
            {
                Logger.Warning("[GROOM] initialization en trop");
            }
            else
            {
                new GRoom();
                EventView.Manager.AddListener(Instance, true);
            }
        }
        
        private GRoom()
        {
            if (Instance != null) Logger.Warning("GRoom replaced");
            Instance = this;
        }
    }
}
