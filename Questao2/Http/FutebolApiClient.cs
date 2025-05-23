using Questao2.DTO;
using System.Text.Json;
using Questao2.Interfaces;

namespace Questao2.Http;

public class FutebolApiClient : IFutebolApiClient
{
    public HttpClient _client { get; set; }
    private const string BaseUrl = "https://jsonmock.hackerrank.com/api/football_matches";

    public FutebolApiClient(HttpClient Client)
    {
        if (Client == null)
        {
            throw new ArgumentNullException(nameof(Client));
        }
        _client = Client;
    }

    public async Task<ApiResponse> GetMatches(string time, int ano, int qualTime = 1, int page = 1)
    {
        string url = $"{BaseUrl}?year={ano}&team{qualTime}={time}&page={page}";

        var response = await _client.GetAsync(url);
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<ApiResponse>(content);

    }
}
