using MediatR;
using Questao5.Application.Interfaces;
using Questao5.Application.Queries;
using Questao5.Application.Queries.Responses;
using Questao5.Domain.Models;

namespace Questao5.Application.Handlers;

public class ConsultarSaldoHandler : IRequestHandler<ConsultarSaldoQuery, ConsultarSaldoResponse>
{
    private readonly IContaCorrenteRepository _contaRepository;
    private readonly IMovimentoRepository _movimentoRepository;

    public ConsultarSaldoHandler(
        IContaCorrenteRepository contaRepository,
        IMovimentoRepository movimentoRepository)
    {
        _contaRepository = contaRepository;
        _movimentoRepository = movimentoRepository;
    }

    public async Task<ConsultarSaldoResponse> Handle(ConsultarSaldoQuery query, CancellationToken cancellationToken)
    {
        var conta = await _contaRepository.GetByIdAsync(query.IdContaCorrente);
        if (conta == null)
            return new ConsultarSaldoResponse
            {
                MensagemErro = "Conta corrente não encontrada.",
                TipoErro = "INVALID_ACCOUNT"
            };

        if (!conta.Ativo)
            return new ConsultarSaldoResponse
            {
                MensagemErro = "Conta corrente inativa.",
                TipoErro = "INACTIVE_ACCOUNT"
            };

        var saldo = await _movimentoRepository.GetSaldoAsync(query.IdContaCorrente);

        return new ConsultarSaldoResponse
        {
            SaldoConta = new SaldoContaResponse
            {
                NumeroConta = conta.Numero,
                NomeTitular = conta.Nome,
                DataConsulta = DateTime.Now,
                Saldo = saldo
            }
        };
    }
}
