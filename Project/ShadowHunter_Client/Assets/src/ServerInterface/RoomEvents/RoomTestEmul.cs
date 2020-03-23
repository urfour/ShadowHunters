using Assets.Scripts.MainMenuUI.Accounts;
using EventSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerInterface.RoomEvents
{
    class RoomTestEmul : IListener<RoomEvent>
    {

        Dictionary<int, RoomData> Rooms { get; set; } = new Dictionary<int, RoomData>();
        Random rand = new Random();

        public void OnEvent(RoomEvent e, string[] tags = null)
        {
            if (e is CreateRoomEvent cre)
            {
                RoomData r = cre.RoomData;
                int code = rand.Next(0, 100000);
                int whilesafe = 100000;
                while (Rooms.ContainsKey(code) && whilesafe > 0)
                {
                    code = rand.Next(0, 100000);
                    whilesafe--;
                }
                r.Code = code;
                r.CurrentNbPlayer = 1;
                r.Players = new string[r.MaxNbPlayer];
                r.Players[0] = GAccount.Instance.LoggedAccount.Login;
                r.Host = GAccount.Instance.LoggedAccount.Login;
                Rooms.Add(code, r);
                EventView.Manager.Emit(new RoomDataEvent() { RoomData = r });
                EventView.Manager.Emit(new RoomJoinedEvent() { RoomData = r });
            }
        }
    }
}
