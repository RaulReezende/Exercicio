using MediatR;
using Questao5.Application.Queries.Responses;
using Questao5.Domain.Models;

namespace Questao5.Application.Queries;

public class ConsultarSaldoQuery : IRequest<ConsultarSaldoResponse>
{
    public string IdContaCorrente { get; set; }
}
