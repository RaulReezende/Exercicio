using Questao5.Domain.Entities;

namespace Questao5.Application.Interfaces;

public interface IIdempotenciaRepository
{
    Task<Idempotencia> GetByChaveAsync(string chaveIdempotencia);
    Task AddAsync(Idempotencia idempotencia);
}
