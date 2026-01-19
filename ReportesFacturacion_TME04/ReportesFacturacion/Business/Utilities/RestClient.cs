using System.Net.Http.Headers;
using System.Net;
using System.Text;
using System.Collections.Generic;
using System.Net.Http;
using System;
using System.Threading.Tasks;

namespace Business.Utilities
{
    public class RestClient
    {
        public static string PostRequest(string url, string jsonRequest)
        {
            ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            string jsonResult = string.Empty;
            StringContent httpContent = new StringContent(jsonRequest, Encoding.UTF8, "application/json");
            try
            {
                httpClient.PostAsync(url, httpContent).ContinueWith((Action<Task<HttpResponseMessage>>)(response =>
                {
                    HttpResponseMessage result = response.Result;
                    Task<string> task = result.Content.ReadAsStringAsync();
                    task.Wait();
                    if (!result.IsSuccessStatusCode)
                        return;
                    jsonResult = task.Result;
                })).Wait();
                return jsonResult;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        public static string GetRequest(string url, Dictionary<string, string> parametersValuePairs)
        {
            ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
            ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => true;
            string uriParameters = string.Empty;
            if (parametersValuePairs != null)
                uriParameters = getUriParametersByDictionary(parametersValuePairs);
            HttpClient httpClient = new HttpClient();
            httpClient.Timeout = TimeSpan.FromMinutes(5);
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            string jsonResult = string.Empty;
            try
            {
                httpClient.GetAsync(url + uriParameters).ContinueWith((Action<Task<HttpResponseMessage>>)(response =>
                {
                    HttpResponseMessage result = response.Result;
                    Task<string> task = result.Content.ReadAsStringAsync();
                    task.Wait();
                    if (!result.IsSuccessStatusCode)
                        return;
                    jsonResult = task.Result;
                })).Wait();
                return jsonResult;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        private static string getUriParametersByDictionary(Dictionary<string, string> parameters)
        {
            StringBuilder uriParameters = new StringBuilder();
            uriParameters.Append("?");
            int count = 1;
            foreach (KeyValuePair<string, string> rowKeyValue in parameters)
            {
                uriParameters.Append(rowKeyValue.Key);
                uriParameters.Append("=");
                uriParameters.Append(rowKeyValue.Value);
                if (count < parameters.Count)
                    uriParameters.Append("&");
                count++;
            }
            return uriParameters.ToString();
        }

        public static string GetRequest(string url, Dictionary<string, string> parametersValuePairs, string token)
        {
            ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
            string uriParameters = getUriParametersByDictionary(parametersValuePairs);
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            httpClient.DefaultRequestHeaders.Add("X-AUTH-TOKEN", token);
            string jsonResult = string.Empty;
            try
            {
                httpClient.GetAsync(url + uriParameters).ContinueWith((Action<Task<HttpResponseMessage>>)(response =>
                {
                    HttpResponseMessage result = response.Result;
                    Task<string> task = result.Content.ReadAsStringAsync();
                    task.Wait();
                    if (!result.IsSuccessStatusCode)
                        return;
                    jsonResult = task.Result;
                })).Wait();
                return jsonResult;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }
    }
}

