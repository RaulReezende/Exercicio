using Dapper;
using Microsoft.Data.Sqlite;
using Questao5.Application.Interfaces;
using Questao5.Domain.Entities;
using System.Data;

namespace Questao5.Infrastructure.Repositories
{
    public class MovimentoRepository : IMovimentoRepository
    {
        private readonly IDbConnection _dbConnection;

        public MovimentoRepository(IDbConnection dbConnection)
        {
            _dbConnection = dbConnection;
        }

        public async Task AddAsync(Movimento movimento)
        {
            using var connection = new SqliteConnection(_dbConnection.ConnectionString);
            await connection.ExecuteAsync(
                "INSERT INTO movimento (idmovimento, idcontacorrente, datamovimento, tipomovimento, valor) " +
                "VALUES (@IdMovimento, @IdContaCorrente, @DataMovimento, @TipoMovimento, @Valor)", movimento
            );
        }

        public async Task<IEnumerable<Movimento>> GetByIdContaCorrenteAsync(string idContaCorrente)
        {
            using var connection = new SqliteConnection(_dbConnection.ConnectionString);
            return await connection.QueryAsync<Movimento>(
                "SELECT idmovimento AS IdMovimento, idcontacorrente AS IdContaCorrente, datamovimento AS DataMovimento, tipomovimento AS TipoMovimento, valor AS Valor FROM movimento WHERE idcontacorrente = @Id", new { Id = idContaCorrente });
        }

        public async Task<decimal> GetSaldoAsync(string idContaCorrente)
        {
            using var connection = new SqliteConnection(_dbConnection.ConnectionString);
            var creditos = await connection.ExecuteScalarAsync<decimal>(
                "SELECT COALESCE(SUM(valor), 0) FROM movimento " +
                "WHERE idcontacorrente = @Id AND tipomovimento = 'C'", new { Id = idContaCorrente });

            var debitos = await connection.ExecuteScalarAsync<decimal>(
                "SELECT COALESCE(SUM(valor), 0) FROM movimento " +
                "WHERE idcontacorrente = @Id AND tipomovimento = 'D'", new { Id = idContaCorrente });

            return creditos - debitos;
        }
    }
}
