using EventSystem;
using Kernel.Settings;
using Network.model;
using ServerInterface.RoomEvents;
using ShadowHunter_Server.Accounts;
using ShadowHunter_Server.Rooms;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ShadowHunter_Client
{
    class Program
    {
        public static Server Server { get; private set; }

        static void Main(string[] args)
        {
            EventView.Load();
            SettingManager.Load();
            Server = new Server();
            GRoom.Init();
            GAccount.Init();

            string cmd = Console.ReadLine();
            while (cmd != "stop")
            {
                string[] cmd_args = cmd.Split(' ');
                if (cmd_args.Length > 0)
                {
                    switch (cmd_args[0])
                    {
                        case "accounts":
                            {
                                if (GAccount.Instance != null)
                                {
                                    Logger.Mutex.WaitOne();
                                    Logger.Comment("[START] : Logged Accounts : ");
                                    Logger.Indent(1);
                                    foreach (var pair in GAccount.Instance.ConnectedAccounts)
                                    {
                                        if (pair.Value.Room != null && pair.Value.Room.Data != null)
                                        {
                                            Logger.Comment(pair.Key + " : " + pair.Value.Room.Data.Name);
                                        }
                                        else
                                        {
                                            Logger.Comment(pair.Key + " : not in room");
                                        }
                                    }
                                    Logger.Indent(-1);
                                    Logger.Comment("[End]   : Logged Accounts : ");
                                    Logger.Mutex.ReleaseMutex();
                                }
                                break;
                            }
                        case "rooms":
                            {
                                if (GRoom.Instance != null)
                                {
                                    Logger.Mutex.WaitOne();
                                    Logger.Comment("[START] : Rooms : ");
                                    Logger.Indent(1);
                                    foreach (var pair in GRoom.Instance.Rooms)
                                    {
                                        if (pair.Value.Data != null)
                                        {
                                            Logger.Comment(pair.Key + " : " + pair.Value.Data.Name);
                                        }
                                        else
                                        {
                                            Logger.Warning(pair.Key + " : null data");
                                        }
                                    }
                                    Logger.Indent(-1);
                                    Logger.Comment("[End]   : Rooms : ");
                                    Logger.Mutex.ReleaseMutex();
                                }
                                break;
                            }
                        case "room":
                            {
                                if (cmd_args.Length == 2)
                                {
                                    int code;
                                    if (int.TryParse(cmd_args[1], out code))
                                    {
                                        if (GRoom.Instance.Rooms.ContainsKey(code))
                                        {
                                            XmlSerializer serializer = new XmlSerializer(typeof(RoomData));
                                            StringBuilder b = new StringBuilder();
                                            serializer.Serialize(new StringWriter(b), GRoom.Instance.Rooms[code].Data);
                                            Logger.Comment(b.ToString());
                                        }
                                        else
                                        {
                                            Logger.Warning("USAGE : room <existing room code>");
                                        }
                                    }
                                    else
                                    {
                                        Logger.Warning("USAGE : room <valid room code>");
                                    }
                                }
                                else
                                {
                                    Logger.Warning("USAGE : room <room code>");
                                }
                                break;
                            }
                    }
                }
                cmd = Console.ReadLine();
            }
            
            Server.Stop();
            Console.ReadLine();

            SettingManager.Save();
        }
    }
}
