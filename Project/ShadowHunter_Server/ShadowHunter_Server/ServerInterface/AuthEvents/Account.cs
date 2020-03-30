using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerInterface.AuthEvents
{
    [Serializable]
    public class Account
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public bool IsLogged { get; set; }

    }
}
