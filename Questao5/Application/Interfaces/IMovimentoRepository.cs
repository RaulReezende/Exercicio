
using Questao5.Domain.Entities;

namespace Questao5.Application.Interfaces;

public interface IMovimentoRepository
{
    Task AddAsync(Movimento movimento);
    Task<IEnumerable<Movimento>> GetByIdContaCorrenteAsync(string idContaCorrente);
    Task<decimal> GetSaldoAsync(string idContaCorrente);
}
