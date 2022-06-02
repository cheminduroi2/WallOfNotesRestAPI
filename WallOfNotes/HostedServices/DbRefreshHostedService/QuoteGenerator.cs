using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace WallOfNotes.HostedServices.DbRefreshHostedService
{
    public static class QuoteGenerator
    {
        private static readonly HttpClient s_client = new HttpClient();
        
        public static async Task<List<string>> fetchMessages(int NumMessages)
        {
            var streamTask = s_client.GetStreamAsync($"https://api.quotable.io/quotes?limit={NumMessages}&maxLength=70");
            return (await JsonSerializer
                .DeserializeAsync<Result>(await streamTask))
                .Quotes
                .Select(quote => quote.Message).ToList();
        }
    }

    public class Result
    {
        [JsonPropertyName("results")]
        public List<Quote> Quotes { get; set; }
    }

    public class Quote
    {
        [JsonPropertyName("content")]
        public string Message { get; set; }
    }
}
