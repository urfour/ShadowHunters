using EventSystem;
using Kernel.Settings;
using ServerInterface.RoomEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.MainMenuUI.SearchGame
{
    class Room
    {
        public Setting<int> Code = new Setting<int>();
        public Setting<string> Name = new Setting<string>();
        public Setting<int> CurrentNbPlayer = new Setting<int>();
        public Setting<int> MaxNbPlayer = new Setting<int>();
        public Setting<bool> HasPassword = new Setting<bool>();

        public Setting<bool> IsActive = new Setting<bool>(false);

        public Room(RoomData data)
        {
            ModifData(data);
        }

        public Room()
        {

        }

        public void ModifData(RoomData data)
        {
            Code.Value = data.Code;
            Name.Value = data.Name;
            MaxNbPlayer.Value = data.MaxNbPlayer;
            CurrentNbPlayer.Value = data.CurrentNbPlayer;
            HasPassword.Value = data.HasPassword;
            IsActive.Value = true;
        }
    }
}
