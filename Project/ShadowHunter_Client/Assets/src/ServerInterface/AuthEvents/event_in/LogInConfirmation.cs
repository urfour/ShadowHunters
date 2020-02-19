using Network.events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerInterface.AuthEvents.event_in
{
    class LogInConfirmation : AuthEvent
    {
        public bool IsSuccess { get; set; }

        public LogInConfirmation(bool isSuccess, string msg = null) : base(msg)
        {
            this.IsSuccess = isSuccess;
        }

        public LogInConfirmation()
        {

        }
    }
}
