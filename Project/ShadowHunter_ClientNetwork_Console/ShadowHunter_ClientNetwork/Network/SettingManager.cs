using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kernel.Settings
{
    public partial class SettingManager
    {
        public Setting<string> Server_IP { get; set; } = new Setting<string>("127.0.0.1");
        public Setting<int> Server_Port { get; set; } = new Setting<int>(30050);
        public Setting<int> BufferSize { get; set; } = new Setting<int>(4096);
        public static Setting<Encoding> Encoder { get; set; } = new Setting<Encoding>(Encoding.Unicode);
    }
}
