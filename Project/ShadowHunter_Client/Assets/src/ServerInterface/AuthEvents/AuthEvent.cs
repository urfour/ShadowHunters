﻿using Network.events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServerInterface.AuthEvents
{
    class AuthEvent : ServerOnlyEvent
    {
        public AuthEvent()
        {

        }

        public AuthEvent(string msg) : base(msg)
        {

        }
    }
}
