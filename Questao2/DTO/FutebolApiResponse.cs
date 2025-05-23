using System.Text.Json.Serialization;


namespace Questao2.DTO;

public class ApiResponse
{
    [JsonPropertyName("page")]
    public int Page { get; set; }

    [JsonPropertyName("per_page")]
    public int PerPage { get; set; }

    [JsonPropertyName("total")]
    public int Total { get; set; }

    [JsonPropertyName("total_pages")]
    public int TotalPages { get; set; }

    [JsonPropertyName("data")]
    public List<Match> Data { get; set; }
}

public class Match
{
    [JsonPropertyName("team1")]
    public string Team1 { get; set; }

    [JsonPropertyName("team2")]
    public string Team2 { get; set; }

    [JsonPropertyName("team1goals")]
    public string Team1Goals { get; set; }

    [JsonPropertyName("team2goals")]
    public string Team2Goals { get; set; }

    [JsonPropertyName("year")]
    public int Year { get; set; }
}
