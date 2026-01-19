using System;
using System.IO;

namespace Business.Objects
{
    public class Log
    {
        string log;
        string path;
        string fileName;
        string fullPath;
        public Log(string path)
        {
            log = string.Empty;
            this.path = path;
            this.fileName = DateTime.Now.ToString("dd_MM_yyyy_HH_mm_ss") + "-ReportesFacturacion.txt";
            this.fullPath = path + fileName;
        }

        public void WriteEntry(string text)
        {
            log = DateTime.Now.ToString("dd/MM/yyy HH:mm:ss") + " -- " + text + Environment.NewLine;
            Save(log);
        }

        public void Save(string log)
        {
            File.AppendAllText(fullPath, log);
            log = string.Empty;
        }
    }
}
