using Dapper;
using Microsoft.Data.Sqlite;
using Questao5.Application.Interfaces;
using Questao5.Domain.Entities;
using System.Data;
using System.Data.Common;

namespace Questao5.Infrastructure.Repositories;

public class IdempotenciaRepository : IIdempotenciaRepository
{
    private readonly IDbConnection _dbConnection;

    public IdempotenciaRepository(IDbConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }

    public async Task<Idempotencia> GetByChaveAsync(string chaveIdempotencia)
    {
        using var connection = new SqliteConnection(_dbConnection.ConnectionString);
        return await connection.QueryFirstOrDefaultAsync<Idempotencia>(
            "SELECT chave_idempotencia AS ChaveIdempotencia, requisicao AS Requisicao, resultado AS Resultado FROM idempotencia WHERE chave_idempotencia = @Chave", new { Chave = chaveIdempotencia }
        );
    }

    public async Task AddAsync(Idempotencia idempotencia)
    {
        using var connection = new SqliteConnection(_dbConnection.ConnectionString);
        await connection.ExecuteAsync(
            "INSERT INTO idempotencia (chave_idempotencia, requisicao, resultado) " +
            "VALUES (@ChaveIdempotencia, @Requisicao, @Resultado)",
            idempotencia);
    }
}
