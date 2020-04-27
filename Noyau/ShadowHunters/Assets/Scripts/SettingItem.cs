using EventSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scripts.Settings
{
    [Serializable]
    public class SettingItem : ListenableObject
    {
        protected object value;

        public virtual object ItemValue
        {
            get
            {
                return value;
            }
        }

        public SettingItem(object val)
        {
            this.value = val;
        }

        public SettingItem()
        {

        }
    }
}
