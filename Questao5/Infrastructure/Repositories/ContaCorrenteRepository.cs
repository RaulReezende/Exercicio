using Dapper;
using Microsoft.Data.Sqlite;
using Questao5.Application.Interfaces;
using Questao5.Domain.Entities;
using System.Data;

namespace Questao5.Infrastructure.Repositories;

public class ContaCorrenteRepository : IContaCorrenteRepository
{
    private readonly IDbConnection _dbConnection;

    public ContaCorrenteRepository(IDbConnection dbConnection)
    {
        _dbConnection = dbConnection;
    }

    public async Task<ContaCorrente?> GetByIdAsync(string id)
    {
        using var connection = new SqliteConnection(_dbConnection.ConnectionString);

        return await connection.QueryFirstOrDefaultAsync<ContaCorrente>(
            "SELECT idcontacorrente AS IdContaCorrente, numero AS Numero, nome AS Nome, ativo AS Ativo FROM contacorrente WHERE idcontacorrente = @Id", new { Id = id });
    }


}
