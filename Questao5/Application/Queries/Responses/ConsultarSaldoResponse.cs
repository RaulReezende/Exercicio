using Questao5.Domain.Models;

namespace Questao5.Application.Queries.Responses;


public class ConsultarSaldoResponse
{
    public SaldoContaResponse SaldoConta { get; set; }
    public string MensagemErro { get; set; }
    public string TipoErro { get; set; }
}
