using System;

namespace Entities.DataTransferObject
{
    public class T_EmailEnvio
    {
        public int IdEmailEnvio { get; set; }
        public string Asunto { get; set; }
        public string FromName { get; set; }
        public string FromEmail { get; set; }
        public string ToEmail { get; set; }
        public string CC { get; set; }
        public string BCC { get; set; }
        public string Body { get; set; }
        public DateTime FechaCreacion { get; set; }
        public bool Enviado { get; set; }
        public Nullable<DateTime> FechaEnvio { get; set; }
        public int Reintentos { get; set; }
        public string Error { get; set; }
    }
}
