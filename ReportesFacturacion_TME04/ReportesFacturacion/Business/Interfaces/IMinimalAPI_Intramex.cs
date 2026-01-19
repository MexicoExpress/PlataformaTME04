using Entities.DataTransferObject;
using System.Collections.Generic;

namespace Business.Interfaces
{
    public interface IMinimalAPI_Intramex
    {
        List<ReporteFacturacionFinanzas> ObtieneFacturas(string urlService, string FechaInicio, string FechaFin);
        T_EmailEnvio InsertaEmail(string urlService, T_EmailEnvio email);
        T_EmailAdjunto InsertaEmailAdjunto(string urlService, T_EmailAdjunto emailAdjunto);
    }
}
