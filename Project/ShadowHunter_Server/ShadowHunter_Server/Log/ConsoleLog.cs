using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Log
{
    class ConsoleLog : ILogger
    {
        int nbIndente = 0;
        string indent = "";
        bool displayErrorStackTrace;
        Mutex mut = new Mutex();

        public ConsoleLog(bool displayErrorStackTrace)
        {
            this.displayErrorStackTrace = displayErrorStackTrace;
        }

        public void Comment(string msg)
        {
            mut.WaitOne();
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
            mut.ReleaseMutex();
        }

        public void Error(string msg)
        {
            mut.WaitOne();
            Console.ForegroundColor = ConsoleColor.Red;
            msg.Replace("\n", "\n" + indent);
            Console.WriteLine(indent + msg);
            Console.ResetColor();
            mut.ReleaseMutex();
        }

        public void Error(Exception msg)
        {
            Console.WriteLine("\n");
            Error(msg.GetType().FullName);
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
            mut.WaitOne();
            Console.ForegroundColor = ConsoleColor.White;
            msg.Replace("\n", "\n" + indent);
            Console.WriteLine(indent + msg);
            Console.ResetColor();
            mut.ReleaseMutex();
        }

        public void Warning(string msg)
        {
            mut.WaitOne();
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            msg.Replace("\n", "\n" + indent);
            Console.WriteLine(indent + msg);
            Console.ResetColor();
            mut.ReleaseMutex();
        }
    }
}
