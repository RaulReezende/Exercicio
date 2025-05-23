using Questao5.Domain.Entities;

namespace Questao5.Application.Interfaces;

public interface IContaCorrenteRepository
{
    Task<ContaCorrente?> GetByIdAsync(string id);
}
