using MediatR;
using Questao5.Application.Commands;
using Questao5.Application.Commands.Responses;
using Questao5.Application.Interfaces;
using Questao5.Domain.Entities;
using Questao5.Domain.Enumerators;
using System.Text.Json;

namespace Questao5.Application.Handlers;

public class MovimentarContaHandler : IRequestHandler<MovimentarContaCommand, MovimentarContaResponse>
{
    private readonly IContaCorrenteRepository _contaRepository;
    private readonly IMovimentoRepository _movimentoRepository;
    private readonly IIdempotenciaRepository _idempotenciaRepository;

    public MovimentarContaHandler(
        IContaCorrenteRepository contaRepository,
        IMovimentoRepository movimentoRepository,
        IIdempotenciaRepository idempotenciaRepository
    )
    {
        _contaRepository = contaRepository;
        _movimentoRepository = movimentoRepository;
        _idempotenciaRepository = idempotenciaRepository;
    }

    public async Task<MovimentarContaResponse> Handle(MovimentarContaCommand command, CancellationToken cancellationToken)
    {
        var conta = await _contaRepository.GetByIdAsync(command.IdContaCorrente);
        if (conta == null)
            return new MovimentarContaResponse
            {
                MensagemErro = "Conta corrente não encontrada.",
                TipoErro = "INVALID_ACCOUNT"
            };

        if (!conta.Ativo)
            return new MovimentarContaResponse
            {
                MensagemErro = "Conta corrente inativa.",
                TipoErro = "INACTIVE_ACCOUNT"
            };

        if (command.Valor <= 0)
            return new MovimentarContaResponse
            {
                MensagemErro = "Valor deve ser positivo.",
                TipoErro = "INVALID_VALUE"
            };

        if (command.TipoMovimento != ((char)TipoMovimentoEnum.Credito) && command.TipoMovimento != ((char)TipoMovimentoEnum.Debito))
            return new MovimentarContaResponse
            {
                MensagemErro = "Tipo de movimento inválido. Use 'C' para crédito ou 'D' para débito.",
                TipoErro = "INVALID_TYPE"
            };

        var idempotencia = await _idempotenciaRepository.GetByChaveAsync(command.IdRequisicao);

        if (idempotencia != null)
            return JsonSerializer.Deserialize<MovimentarContaResponse>(idempotencia.Resultado);

        var movimento = new Movimento(
                Guid.NewGuid().ToString(),
                command.IdContaCorrente,
                DateTime.Now,
                (char)command.TipoMovimento,
                command.Valor
        );

        await _movimentoRepository.AddAsync(movimento);

        var response = new MovimentarContaResponse { IdMovimento = movimento.IdMovimento };

        var idempotenciaNova = new Idempotencia(
               command.IdRequisicao,
               JsonSerializer.Serialize(command),
               JsonSerializer.Serialize(response));

        await _idempotenciaRepository.AddAsync(idempotenciaNova);

        return response;
    }       

}
