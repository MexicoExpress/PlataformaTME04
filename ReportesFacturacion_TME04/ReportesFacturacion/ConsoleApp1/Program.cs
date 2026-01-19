using Business.Interfaces;
using Business.Objects;
using Business.Utilities;
using System.Collections.Generic;
using System;
using System.Configuration;
using System.IO;
using System.Linq;
using Entities.DataTransferObject;
using System.Security.Principal;
using SpreadsheetLight;
using System.ServiceProcess;



namespace Rpt_TME
{
    public class Program
    {

        IMinimalAPI_Intramex minimalAPI_Intramex;

        readonly string logPath = ConfigurationManager.AppSettings["LogPath"];
        readonly string daysOfBackup = ConfigurationManager.AppSettings["DaysOfBackup"];
        readonly string urlMinimalAPI_Intramex = ConfigurationManager.AppSettings["MinimalAPI_Intramex"];
        readonly string urlNas = ConfigurationManager.AppSettings["UrlNas"];
    
        readonly string userName = ConfigurationManager.AppSettings["ImpersonationUserName"];
        readonly string password = ConfigurationManager.AppSettings["ImpersonationPassword"];
        readonly string domain = ConfigurationManager.AppSettings["ImpersonationDomain"];


        public Program()
        {
            minimalAPI_Intramex = new MinimalAPI_IntramexImpl();
        }

        public static void Main(string[] args)
        {
            Console.WriteLine("Ejecuta envio de reporte diario");

            Program programRpt = new Program();
            programRpt.EnviaReporteFact();
        }
        private void EnviaReporteFact()
        {
            Log log = new Log(logPath);
            try
            {

                Console.WriteLine("Se obtienen parametros de consulta");
                log.WriteEntry("Se obtienen parametros de consulta");
                DateTime date = DateTime.Now;
                var fechaI = date.AddDays(-1).ToString("yyyy-MM-dd");
                var fechaF = date.AddDays(-1).ToString("yyyy-MM-dd");
                
                
                Console.WriteLine("Consultando facturas");
                    log.WriteEntry("Consultando facturas");



                var facturas = minimalAPI_Intramex.ObtieneFacturas(urlMinimalAPI_Intramex, fechaI, fechaF);

                    if (facturas != null)
                    {
                    Console.WriteLine("Escribiendo excel");
                        log.WriteEntry("Escribiendo excel");


                    SLDocument oSLDocu = new SLDocument();
                        System.Data.DataTable dt = new System.Data.DataTable();


                        //columnas
                        dt.Columns.Add("CLIENTE", typeof(string));
                        dt.Columns.Add("SERIE", typeof(string));
                        dt.Columns.Add("FOLIO", typeof(int));
                        dt.Columns.Add("FECHA", typeof(string));
                        dt.Columns.Add("FECHA_CANCELACION", typeof(string));
                        dt.Columns.Add("MASTER", typeof(string));
                        dt.Columns.Add("HOUSE", typeof(string));
                        dt.Columns.Add("SUBTOTAL MXN", typeof(decimal));
                        dt.Columns.Add("IVA MXN", typeof(decimal));
                        dt.Columns.Add("TOTAL", typeof(decimal));
                        dt.Columns.Add("AGENTE", typeof(string));

                        foreach (var factura in facturas)
                        {
                           
                            dt.Rows.Add(
                                factura.Cliente,
                                factura.Serie,
                                factura.Folio,
                                factura.Fecha,
                                factura.FechaCancelacion,
                                factura.Master,
                                factura.House,
                                factura.SubTotalMXN,
                                factura.ImpuestoMXN,
                                factura.TotalMXN,
                                factura.Agente                              
                                );

                        }

                        var resultCorreo = EnviarCorreo();
                        if (resultCorreo > 0)
                        {
                            string fileName = resultCorreo.ToString() + "-" + Utiles.GeneraCodigo(8) + "_" + DateTime.Now.ToString("yyyy-MM-dd") + ".xlsx";
                    

                            string nasFile = urlNas + fileName;
                            WindowsImpersonationContext wic = Impersonator.LogOn(userName, password, domain);

                            oSLDocu.ImportDataTable(1, 1, dt, true);
                            oSLDocu.SaveAs(nasFile);

                            Console.WriteLine("Excel guardado correctamente");
                            log.WriteEntry("Excel guardado correctamente");

                        var res = EnviarCorreoAdjunto("TME04_REPORTE_DIARIO_FACTURACIÓN_" + DateTime.Now.ToString("yyyy-MM-dd") + ".xlsx", fileName, resultCorreo);
                        }

                    }
                

                log.WriteEntry("Elimina logs");
                List<string> caducFiles = GetCaducFiles();
                DeleteCaducFiles(caducFiles);
                //}
            }
            catch (Exception ex)
            {
                log.WriteEntry("Error detectado");
                log.WriteEntry(ex.ToString());
            }
        }


        public int EnviarCorreo()
        {
            Log log = new Log(logPath);
            try
            {
                Console.WriteLine("Enviando correo... ");
                log.WriteEntry("Enviando correo... ");

                string email = ConfigurationManager.AppSettings["Email"];

                DateTime date = DateTime.Now;
                var fecha = date.AddDays(-1).ToString("dd/MM/yyyy");

                string asuntoCorreo = "TME04 - REPORTE DIARIO DE FACTURACION DEL " + fecha;
                string fromName = "Departamento de Sistemas";
                string body = GeneraBody();
                string toEmail = email;

                T_EmailEnvio emailEnvio = new T_EmailEnvio()
                {
                    Asunto = asuntoCorreo,
                    FromName = fromName,
                    FromEmail = "sistemas@tme04.com",
                    ToEmail = toEmail,
                    CC = string.Empty,
                    BCC = string.Empty,
                    Body = body,
                    FechaCreacion = DateTime.Now
                };

                var emailResponse = minimalAPI_Intramex.InsertaEmail(urlMinimalAPI_Intramex, emailEnvio);


                return emailResponse.IdEmailEnvio;
            }
            catch (Exception ex)
            {
                log.WriteEntry("Error detectado");
                log.WriteEntry(ex.ToString());
                return 0;
            }
        }

        public int EnviarCorreoAdjunto(string nombreAdjunto, string nombrefisico, int idEmailEnvio)
        {
            Log log = new Log(logPath);
            try
            {
                T_EmailAdjunto emailAdjunto = new T_EmailAdjunto()
                {
                    IdEmailEnvio = idEmailEnvio,
                    NombreAdjunto = nombreAdjunto,
                    NombreFisico = nombrefisico,
                    FechaCreacion = DateTime.Now
                };

                Console.WriteLine("Enviando correo adjunto");
                log.WriteEntry("Enviando correo adjunto");

                var adjunto = minimalAPI_Intramex.InsertaEmailAdjunto(urlMinimalAPI_Intramex, emailAdjunto);

                return 1;
            }
            catch (Exception ex)
            {
                log.WriteEntry("Error detectado");
                log.WriteEntry(ex.ToString());
                return 0;
            }
        }

        public string GeneraBody()
        {
            string html =
            $"<p style='color:black'>Se envía el reporte diario de facturación correspondiente a TME04.</p><p style='color: black'>Saludos cordiales.</p><p style='color:gray; font-size:small;'><br/><br/><b style='color: #8A0829'>NOTA IMPORTANTE: </b> Favor de no responder debido a que la cuenta no esta habilitada para recibir respuestas.</p>";

            return html;
        }

        public bool CheckLogFolder(string LogPath)
        {
            if (!Directory.Exists(LogPath))
            {
                Directory.CreateDirectory(LogPath);
                return true;
            }
            else
            {
                return DeleteCaducFiles(GetCaducFiles());
            }


        }
        public List<string> GetCaducFiles()
        {
            Log log = new Log(logPath);
            string[] txtLogFiles;
            List<string> caducFiles = new List<string>();
            try
            {
                log.WriteEntry("Consultando archivos logs");
                txtLogFiles = Directory.GetFiles(logPath);
                DateTime limitDate = DateTime.Now.AddDays(-Convert.ToInt32(daysOfBackup));
                caducFiles =
                    txtLogFiles.Where
                    (
                        cbf => DateTime.ParseExact(cbf.Replace(logPath, string.Empty).Substring(0, 10), "dd_MM_yyyy", null) < limitDate
                    ).ToList();
            }
            catch (Exception ex)
            {
                log.WriteEntry("Error detectado");
                log.WriteEntry(ex.ToString());
            }
            return caducFiles;
        }
        public bool DeleteCaducFiles(List<string> caducFiles)
        {
            Log log = new Log(logPath);
            try
            {
                log.WriteEntry("Eliminando archivos log");
                foreach (string caducFile in caducFiles)
                {
                    DeleteFile(caducFile);
                }
            }
            catch (Exception ex)
            {
                log.WriteEntry("Error detectado");
                log.WriteEntry(ex.ToString());
            }
            return true;
        }
        public bool DeleteFile(string caducFile)
        {
            File.Delete(caducFile);
            return true;
        }
    }
}
