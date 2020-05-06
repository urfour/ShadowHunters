using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Log
{
    interface ILogger
    {
        void Info(string msg);
        void Comment(string msg);
        void Warning(string msg);
        void Error(string msg);
        void Error(Exception msg);
        void Indent(int amount = 1);
    }
}
