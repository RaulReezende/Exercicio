using MediatR;
using Microsoft.AspNetCore.Mvc;
using Questao5.Application.Commands;
using Questao5.Application.Queries;

namespace Questao5.Controllers;

[ApiController]
[Route("api/contacorrente")]
public class ContaCorrenteController : ControllerBase
{
    private readonly IMediator _mediator;

    public ContaCorrenteController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("movimentar")]
    public async Task<IActionResult> Movimentar([FromBody] MovimentarContaCommand command)
    {
        var result = await _mediator.Send(command);

        if (!string.IsNullOrEmpty(result.MensagemErro))
        {
            return BadRequest(new { result.MensagemErro, result.TipoErro });
        }

        return Ok(new { result.IdMovimento });
    }

    [HttpGet("saldo/{idContaCorrente}")]
    public async Task<IActionResult> ConsultarSaldo(string idContaCorrente)
    {
        var query = new ConsultarSaldoQuery { IdContaCorrente = idContaCorrente };
        var result = await _mediator.Send(query);

        if (!string.IsNullOrEmpty(result.MensagemErro))
        {
            return BadRequest(new { result.MensagemErro, result.TipoErro });
        }

        return Ok(result.SaldoConta);
    }
}
