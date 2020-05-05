using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kernel.Settings
{
    public partial class SettingManager
    {
        public Setting<int> Port { get; set; } = new Setting<int>(30050);
        public Setting<int> BufferSize { get; set; } = new Setting<int>(4096);
        public static Setting<Encoding> Encoder { get; set; } = new Setting<Encoding>(Encoding.Unicode);
    }
}
