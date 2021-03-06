﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerInterface.RoomEvents
{
    [Serializable]
    public class RoomData
    {
        public RoomData(int code, string name, int maxNbPlayers, int currentNbPlayers, bool isSuppressed, bool isPrivate, bool withExtension, bool isLaunched = false)
        {
            Code = code;
            Name = name;
            MaxNbPlayer = maxNbPlayers;
            CurrentNbPlayer = currentNbPlayers;
            IsSuppressed = isSuppressed;
            IsPrivate = isPrivate;
            WithExtension = withExtension;
            IsLaunched = isLaunched;
        }

        public RoomData()
        {

        }

        // defined by server
        public int Code { get; set; } = 0;
        public int CurrentNbPlayer { get; set; } = 0;
        public bool IsSuppressed { get; set; } = false;
        public bool IsLaunched { get; set; } = false;
        public string[] Players { get; set; } = null;
        public bool[] ReadyPlayers { get; set; } = null;

        // defined by host
        public string Name { get; set; } = "";
        public int MaxNbPlayer { get; set; } = 0;
        public bool IsPrivate { get; set; } = false;
        public bool WithExtension { get; set; } = false;
        public string Password { get; set; } = "";
    }
}
