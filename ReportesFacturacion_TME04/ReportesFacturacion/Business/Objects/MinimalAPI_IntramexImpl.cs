using Business.Interfaces;
using Business.Utilities;
using Entities.DataTransferObject;
using System;
using System.Collections.Generic;

namespace Business.Objects
{
    public class MinimalAPI_IntramexImpl : IMinimalAPI_Intramex
    {
        protected readonly string uriObtieneFacturas = "/api/ObtenerFacturacionDiariaFinanzas";
        protected readonly string uriEnviaCorreo = "/api/InsertaEmail";
        protected readonly string uriEnviaCorreoAdjunto = "/api/InsertaEmailAdjunto";

        public List<ReporteFacturacionFinanzas> ObtieneFacturas(string urlService, string FechaInicio, string FechaFin)
        {
            string fullUrl = urlService + uriObtieneFacturas + $"/{FechaInicio}" + $"/{FechaFin}";
            string jsonResponse = RestClient.GetRequest(fullUrl, null);
            if (string.IsNullOrEmpty(jsonResponse))
                return null;
            List<ReporteFacturacionFinanzas> facturas =
                JsonUtil.DeserializeJson<List<ReporteFacturacionFinanzas>>(jsonResponse);
            return facturas;
        }

        public T_EmailEnvio InsertaEmail(string urlService, T_EmailEnvio email)
        {
            string fullUrl = urlService + uriEnviaCorreo;
            string jsonResponse = RestClient.PostRequest(fullUrl, JsonUtil.SerializeJson(email));
            if (string.IsNullOrEmpty(jsonResponse))
                return null;
            T_EmailEnvio response =
                JsonUtil.DeserializeJson<T_EmailEnvio>(jsonResponse);
            return response;
        }

        public T_EmailAdjunto InsertaEmailAdjunto(string urlService, T_EmailAdjunto emailAdjunto)
        {
            string fullUrl = urlService + uriEnviaCorreoAdjunto;
            string jsonResponse = RestClient.PostRequest(fullUrl, JsonUtil.SerializeJson(emailAdjunto));
            if (string.IsNullOrEmpty(jsonResponse))
                return null;
            T_EmailAdjunto response =
                JsonUtil.DeserializeJson<T_EmailAdjunto>(jsonResponse);
            return response;
        }
    }
}
