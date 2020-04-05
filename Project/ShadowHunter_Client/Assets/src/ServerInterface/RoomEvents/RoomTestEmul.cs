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
                int code = rand.Next(10000, 100000);
                int whilesafe = 100000;
                while (Rooms.ContainsKey(code) && whilesafe > 0)
                {
                    code = rand.Next(10000, 100000);
                    whilesafe--;
                }
                r.Code = code;
                r.CurrentNbPlayer = 1;
                r.Players = new string[r.MaxNbPlayer];
                r.ReadyPlayers = new bool[r.MaxNbPlayer];
                r.Players[0] = GAccount.Instance.LoggedAccount.Login;
                //r.Host = GAccount.Instance.LoggedAccount.Login;
                Rooms.Add(code, r);
                EventView.Manager.Emit(new RoomDataEvent() { RoomData = r });
                EventView.Manager.Emit(new RoomJoinedEvent() { RoomData = r });
            }
            else if (e is JoinRoomEvent jre)
            {
                RoomData r = Rooms[jre.RoomData.Code];
                if (r.CurrentNbPlayer < r.MaxNbPlayer)
                {
                    r.Players[r.CurrentNbPlayer] = GAccount.Instance.LoggedAccount.Login;
                    r.CurrentNbPlayer++;
                    EventView.Manager.Emit(new RoomDataEvent() { RoomData = r });
                    EventView.Manager.Emit(new RoomJoinedEvent() { RoomData = r });
                }
                else
                {
                    // unsuccess
                }
            }
            else if (e is LeaveRoomEvent lre)
            {
                RoomData r = Rooms[lre.RoomData.Code];
                bool found = false;
                r.CurrentNbPlayer--;
                if (r.CurrentNbPlayer == 0)
                {
                    r.IsSuppressed = true;
                }
                else
                {
                    for (int i = 0; i < r.CurrentNbPlayer; i++)
                    {
                        if (found)
                        {
                            r.Players[i] = r.Players[i + 1];
                            r.ReadyPlayers[i] = r.ReadyPlayers[i + 1];
                        }
                        else
                        {
                            if (r.Players[i] == GAccount.Instance.LoggedAccount.Login)
                            {
                                found = true;
                                r.Players[i] = r.Players[i + 1];
                                r.ReadyPlayers[i] = r.ReadyPlayers[i + 1];
                            }
                        }
                    }
                    r.ReadyPlayers[r.CurrentNbPlayer] = false;
                    r.Players[r.CurrentNbPlayer] = null;
                }
                EventView.Manager.Emit(new RoomDataEvent() { RoomData = r });
                EventView.Manager.Emit(new RoomLeavedEvent() { RoomData = r });
            }
            else if (e is ReadyEvent re)
            {
                RoomData r = re.RoomData;
                for (int i = 0; i < r.CurrentNbPlayer; i++)
                {
                    if (r.Players[i] == GAccount.Instance.LoggedAccount.Login)
                    {
                        r.ReadyPlayers[i] = !r.ReadyPlayers[i];
                        break;
                    }
                }
                EventView.Manager.Emit(new RoomDataEvent() { RoomData = r });
            }
            else if (e is StartGameEvent sge)
            {
                RoomData r = sge.RoomData;
                bool ready = true;
                for (int i = 0; i < r.MaxNbPlayer; i++)
                {
                    if (!r.ReadyPlayers[i])
                    {
                        ready = false;
                        break;
                    }
                }
                if (ready)
                {
                    r.IsLaunched = true;
                    EventView.Manager.Emit(new RoomDataEvent() { RoomData = r });
                }
            }
        }
    }
}
