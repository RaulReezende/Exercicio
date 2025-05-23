using Moq;
using Questao5.Application.Commands;
using Questao5.Application.Handlers;
using Questao5.Application.Interfaces;
using Questao5.Application.Queries;
using Questao5.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Questao5.Tests.Handlers;

public class ConsultarSaldoHandlerTests
{
    private readonly Mock<IContaCorrenteRepository> _contaCorrenteRepositoryMock;
    private readonly Mock<IMovimentoRepository> _movimentoRepositoryMock;
    private readonly ConsultarSaldoHandler _handler;

    public ConsultarSaldoHandlerTests()
    {
        _contaCorrenteRepositoryMock = new Mock<IContaCorrenteRepository>();
        _movimentoRepositoryMock = new Mock<IMovimentoRepository>();
        _handler = new ConsultarSaldoHandler(
            _contaCorrenteRepositoryMock.Object,
            _movimentoRepositoryMock.Object
        );
    }

    [Fact]
    public async Task Handle_ContaNaoExiste_ReturnError()
    {
        // Arrange
        _contaCorrenteRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<string>())).ReturnsAsync(() => null);

        var query = new ConsultarSaldoQuery
        {
            IdContaCorrente = "123"
        };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Equal("INVALID_ACCOUNT", result.TipoErro);
        Assert.NotNull(result.MensagemErro);
    }

    [Fact]
    public async Task Handle_ContaInativa_ReturnError()
    {
        // Arrange
        var contaCorrenteContaInativa = new ContaCorrente(
            idContaCorrente: "123",
            numero: 123,
            nome: "Roberta Skinner",
            ativo: false
        );

        _contaCorrenteRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<string>())).ReturnsAsync(contaCorrenteContaInativa);

        var query = new ConsultarSaldoQuery
        {
            IdContaCorrente = "123"
        };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Equal("INACTIVE_ACCOUNT", result.TipoErro);
        Assert.NotNull(result.MensagemErro);
    }

    [Fact]
    public async Task Handle_DadosValido_ReturnSaldo()
    {
        // Arrange
        var contaCorrenteContaInativa = new ContaCorrente(
            idContaCorrente: "123",
            numero: 123,
            nome: "Roberta Skinner",
            ativo: true
        );

        _contaCorrenteRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<string>())).ReturnsAsync(contaCorrenteContaInativa);
        _movimentoRepositoryMock.Setup(x => x.GetSaldoAsync(It.IsAny<string>())).ReturnsAsync(234);

        var query = new ConsultarSaldoQuery
        {
            IdContaCorrente = "123"
        };

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Equal(234, result.SaldoConta.Saldo);
        Assert.Null(result.MensagemErro);
    }
}
