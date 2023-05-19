using Newtonsoft.Json;
using System.Net;
using System.Text;

namespace RareCrew.Infrastructure
{
    public class RestClient : IRestClient
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;
       
        
        public RestClient(IConfiguration config)
        {
            _config = config;
            _httpClient = new HttpClient
            {
                Timeout = TimeSpan.FromSeconds(Convert.ToDouble(_config["RareCrew:apiTimeOut"]))
            };
            _httpClient.DefaultRequestHeaders.Accept.Clear();
        }

        public List<T>? Get<T>(string methodUrl, IEnumerable<KeyValuePair<string, object>> parameters, Dictionary<string, string> headers, string apiUrl)
        {           
            try
            {
                SetHeaders(headers);
                if (apiUrl != null)
                {
                    _httpClient.BaseAddress = new Uri(apiUrl);
                }
                if (parameters != null && parameters.Any())
                {
                    methodUrl += AsQueryString(parameters);
                }
                var response = _httpClient.GetAsync(methodUrl).Result;

                string? result = response.Content.ReadAsStringAsync().Result;

                List<T>? list = JsonConvert.DeserializeObject<List<T>>(result) ?? null;
               
                if (response.IsSuccessStatusCode)
                {
                    return list;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        private void SetHeaders(Dictionary<string, string> headers)
        {
            _httpClient.DefaultRequestHeaders.Clear();
            if (headers != null && headers.Count > 0)
            {
                foreach (var header in headers)
                {
                    _httpClient.DefaultRequestHeaders.Add(header.Key, header.Value);
                }
            }
        }
        private static string AsQueryString(IEnumerable<KeyValuePair<string, object>> parameters)
        {
            if (!parameters.Any())
                return string.Empty;

            var builder = new StringBuilder("?");

            var separator = "";

            foreach (var kvp in parameters)
            {
                if (kvp.Value != null)
                {
                    var type = kvp.Value.GetType();
                    if (type != null && type == typeof(DateTime))
                    {
                        DateTime date = DateTime.SpecifyKind((DateTime)kvp.Value, DateTimeKind.Utc);
                        string? isoDate = date.ToString("o");
                        builder.AppendFormat("{0}{1}={2}", separator, WebUtility.UrlEncode(kvp.Key), WebUtility.UrlEncode(isoDate ?? string.Empty));
                    }
                    else
                    {
                        builder.AppendFormat("{0}{1}={2}", separator, WebUtility.UrlEncode(kvp.Key), WebUtility.UrlEncode(kvp.Value == null ? string.Empty : kvp.Value.ToString()));
                    }
                }
                else
                {
                    builder.AppendFormat("{0}{1}={2}", separator, WebUtility.UrlEncode(kvp.Key), WebUtility.UrlEncode(kvp.Value == null ? string.Empty : kvp.Value.ToString()));
                }

                separator = "&";
            }

            return builder.ToString();
        }
    }
}
