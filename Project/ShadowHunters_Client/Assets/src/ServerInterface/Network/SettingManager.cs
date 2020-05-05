using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kernel.Settings
{
    public partial class SettingManager
    {
        public Setting<string> ServerIP { get; set; } = new Setting<string>("51.91.157.30");
        public Setting<int> ServerPort { get; set; } = new Setting<int>(30050);
        public Setting<int> BufferSize { get; set; } = new Setting<int>(4096);

        public Setting<string> RegisteredLogin { get; set; } = new Setting<string>(null);
        public Setting<string> RegisteredPassword { get; set; } = new Setting<string>(null);

        public static Setting<Encoding> Encoder { get; set; } = new Setting<Encoding>(Encoding.Unicode);
    }
}
