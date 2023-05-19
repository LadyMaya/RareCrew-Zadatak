using RareCrew.Models;

namespace RareCrew.Infrastructure
{
    public interface IRestClient
    {
        List<T>? Get<T>(string methodUrl, IEnumerable<KeyValuePair<string, object>> parameters, Dictionary<string, string> headers, string apiUrl);
    }
}
