using Questao2.DTO;

namespace Questao2.Interfaces;

public interface IFutebolApiClient
{
    public Task<ApiResponse> GetMatches(string time, int ano, int qualTime, int page);
}
