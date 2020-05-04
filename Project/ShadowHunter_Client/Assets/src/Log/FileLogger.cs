using IO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Log
{
    class FileLogger : ILogger
    {
        FileStream logFile;


        public FileLogger()
        {
            string filename = DateTime.Now.ToString("yyyy_MM_dd_HH_mm_ss") + ".txt";
            logFile = IOSystem.CreateFile("Logs/" + filename, true);
        }

        public void Comment(string msg)
        {
            byte[] buffer = Encoding.Unicode.GetBytes("[COMMENT] " + msg + "\n\n");
            logFile.Write(buffer, 0, buffer.Length);
        }

        public void Error(string msg)
        {
            byte[] buffer = Encoding.Unicode.GetBytes("[ERROR] " + msg + "\n\n");
            logFile.Write(buffer, 0, buffer.Length);
        }

        public void Error(Exception msg)
        {
            byte[] buffer = Encoding.Unicode.GetBytes("##########################\n[ERROR]\n##########################\n" + msg + " \n\t" + msg.Message + " \n" + msg.TargetSite + "\n##########################\n\n");
            logFile.Write(buffer, 0, buffer.Length);
        }

        public void Indent(int amount = 1)
        {
            return;
        }

        public void Info(string msg)
        {
            byte[] buffer = Encoding.Unicode.GetBytes("[INFO] " + msg + "\n\n");
            logFile.Write(buffer, 0, buffer.Length);
        }

        public void Warning(string msg)
        {
            byte[] buffer = Encoding.Unicode.GetBytes("[WARNING] " + msg + "\n\n");
            logFile.Write(buffer, 0, buffer.Length);
        }
    }
}
