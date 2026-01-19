namespace Entities.DataTransferObject
{
    public class ReporteFacturacionFinanzas
    {
        public string Cliente { get; set; }
        public string Serie { get; set; }
        public int Folio { get; set; }
        public string Fecha { get; set; }
        public string FechaCancelacion { get; set; }
        public string Master { get; set; }
        public string House { get; set; }
        public decimal SubTotalMXN { get; set; }
        public decimal ImpuestoMXN { get; set; }
        public decimal TotalMXN { get; set; }
        public string Agente { get; set; }
       
    }
}
