using EventSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kernel.Settings
{
    [Serializable]
    public class Setting<T> : SettingItem
    {
        public T Value
        {
            get
            {
                return (T)value;
            }
            set
            {
                this.value = value;
                Notify();
            }
        }

        public Setting(T val)
        {
            this.value = val;
        }

        public Setting()
        {

        }
    }
}
