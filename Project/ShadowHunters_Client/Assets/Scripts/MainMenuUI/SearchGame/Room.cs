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
    public class Room : ListenableObject
    {
        public Setting<int> Code = new Setting<int>(0);
        public Setting<string> Name = new Setting<string>("");
        public Setting<int> CurrentNbPlayer = new Setting<int>(0);
        public Setting<int> MaxNbPlayer = new Setting<int>(0);
        public Setting<bool> IsPrivate = new Setting<bool>(false);
        public Setting<bool> WithExtension = new Setting<bool>(false);
        public Setting<string[]> Players = new Setting<string[]>(null);

        public Setting<bool> IsActive = new Setting<bool>(false);

        public RoomData RawData { get; private set; } = null;

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
            IsPrivate.Value = data.IsPrivate;
            WithExtension.Value = data.WithExtension;
            IsActive.Value = true;
            Players.Value = data.Players;
            RawData = data;
            Notify();
        }
    }
}
