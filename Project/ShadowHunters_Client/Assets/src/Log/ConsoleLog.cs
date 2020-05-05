using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Log
{
    class ConsoleLog : ILogger
    {
        int nbIndente = 0;
        string indent = "";
        bool displayErrorStackTrace;

        public ConsoleLog(bool displayErrorStackTrace)
        {
            this.displayErrorStackTrace = displayErrorStackTrace;
        }

        public void Comment(string msg)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            if (msg.Contains('\n'))
            {
                Console.WriteLine(indent + ("/*\n" + msg + "\n*/").Replace("\n", "\n" + indent));
            }
            else
            {
                Console.WriteLine(indent + "// " + msg);
            }
            Console.ResetColor();
        }

        public void Error(string msg)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            msg.Replace("\n", "\n" + indent);
            Console.WriteLine(indent + msg);
            Console.ResetColor();
        }

        public void Error(Exception msg)
        {
            Console.WriteLine("\n");
            Error(msg.Message);
            if (displayErrorStackTrace)
            {
                Indent();
                Error("Target: " + msg.TargetSite + "\nAt : ");
                Error(msg.StackTrace);
                Indent(-1);
            }
            Console.WriteLine("\n");
        }

        public void Indent(int amount = 1)
        {
            this.nbIndente += amount;
            this.indent = "";
            for (int i = 0; i < nbIndente; i++)
            {
                this.indent += "\t";
            }
        }

        public void Info(string msg)
        {
            Console.ForegroundColor = ConsoleColor.White;
            msg.Replace("\n", "\n" + indent);
            Console.WriteLine(indent + msg);
            Console.ResetColor();
        }

        public void Warning(string msg)
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            msg.Replace("\n", "\n" + indent);
            Console.WriteLine(indent + msg);
            Console.ResetColor();
        }
    }
}
