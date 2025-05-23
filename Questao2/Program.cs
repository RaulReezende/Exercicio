using Questao2.Http;
using Questao2.Services;

public class Program
{
    public static async Task Main(string[] args)
    {
        using HttpClient client = new HttpClient();
        var apiClient = new FutebolApiClient(client);
        var golsCalculator = new GolsCalculator(apiClient);

        string teamName = "Paris Saint-Germain";
        int year = 2013;
        int totalGoals = await golsCalculator.getTotalScoredGoals(teamName, year);

        Console.WriteLine("Team " + teamName + " scored " + totalGoals.ToString() + " goals in " + year);

        teamName = "Chelsea";
        year = 2014;
        totalGoals = await golsCalculator.getTotalScoredGoals(teamName, year);

        Console.WriteLine("Team " + teamName + " scored " + totalGoals.ToString() + " goals in " + year);

        // Output expected:
        // Team Paris Saint - Germain scored 109 goals in 2013
        // Team Chelsea scored 92 goals in 2014
    }
}
