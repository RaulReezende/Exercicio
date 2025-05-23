using MediatR;
using Questao5.Application.Commands.Responses;
using Questao5.Domain.Enumerators;

namespace Questao5.Application.Commands;

public class MovimentarContaCommand : IRequest<MovimentarContaResponse>
{
    public string IdRequisicao { get; set; }
    public string IdContaCorrente { get; set; }
    public decimal Valor { get; set; }
    public char TipoMovimento { get; set; } // C (Crédito) ou D (Débito)
}
