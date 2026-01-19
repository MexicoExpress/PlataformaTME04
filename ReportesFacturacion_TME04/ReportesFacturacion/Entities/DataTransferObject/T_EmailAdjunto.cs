using System;

namespace Entities.DataTransferObject
{
    public class T_EmailAdjunto
    {
        public int IdEmailAdjunto { get; set; }
        public int IdEmailEnvio { get; set; }
        public string NombreAdjunto { get; set; }
        public string NombreFisico { get; set; }
        public DateTime FechaCreacion { get; set; }
    }
}
