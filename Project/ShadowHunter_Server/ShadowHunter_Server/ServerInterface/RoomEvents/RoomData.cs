using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerInterface.RoomEvents
{
    [Serializable]
    public class RoomData
    {
        public RoomData(int code, string name, int maxNbPlayers, int currentNbPlayers, bool isSuppressed, bool hasPassword, bool isLaunched = false)
        {
            Code = code;
            Name = name;
            MaxNbPlayer = maxNbPlayers;
            CurrentNbPlayer = currentNbPlayers;
            IsSuppressed = isSuppressed;
            HasPassword = hasPassword;
            this.IsLaunched = isLaunched;

        }

        // defined by server
        public int Code { get; set; }
        public int CurrentNbPlayer { get; set; } = 0;
        public bool IsSuppressed { get; set; } = false;
        public bool IsLaunched { get; set; } = false;
        public string[] Players { get; set; } = null;
        public string Host { get; set; } = null;

        // defined by host
        public string Name { get; set; }
        public int MaxNbPlayer { get; set; }
        public bool HasPassword { get; set; }
        public string Password { get; set; }

        public RoomData()
        {

        }
    }
}
