using IsoFetcher.Services.Interfaces;
using System.Text.RegularExpressions;

namespace IsoFetcher.Services
{
    public class WorldBankIsoRequestClientService : IWorldBankIsoRequestClientService
    {
        static readonly HttpClient client = new();

        public bool IsIsoCodeValid(string isoString)
        {
            return Regex.IsMatch(isoString, @"^[a-zA-Z]+$") && (isoString.Length == 2 || isoString.Length == 3);
        }

        public async Task<string?> SendIsoRequest(string isoString)
        {
            var response = await client.GetAsync($"http://api.worldbank.org/v2/country/{isoString}?format=json");
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();
            return responseBody;
        }
    }
}
