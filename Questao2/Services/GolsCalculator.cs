using Questao2.DTO;
using Questao2.Interfaces;

namespace Questao2.Services;

public class GolsCalculator : IGolsCalculator
{
    private readonly IFutebolApiClient _futebolApi;
    private const int TimeMandante = 1;
    private const int TimeVisitante = 2;
    public GolsCalculator(IFutebolApiClient apiClient)
    {
        _futebolApi = apiClient ?? throw new ArgumentNullException(nameof(apiClient));
    }

    public async Task<int> getTotalScoredGoals(string time, int ano)
    {
        int totalGols = 0;
        int currentPage = 1;
        int totalPages = 1;
        int qualTime = TimeMandante;

        while (currentPage <= totalPages)
        {


            ApiResponse response = await _futebolApi.GetMatches(time, ano, qualTime, currentPage);

            totalPages = response.TotalPages;

            totalGols += response.Data.Sum(match =>
                match.Team1.Equals(time, StringComparison.OrdinalIgnoreCase)
                ? int.Parse(match.Team1Goals)
                : int.Parse(match.Team2Goals));

            currentPage++;

            if (currentPage > totalPages && qualTime == 1)
            {
                totalPages = 1;
                currentPage = 1;
                qualTime = TimeVisitante;
            }
        }

        return totalGols;
    }
}
