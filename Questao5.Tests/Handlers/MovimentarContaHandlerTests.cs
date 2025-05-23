using FluentAssertions.Equivalency;
using MediatR;
using Moq;
using Questao5.Application.Commands;
using Questao5.Application.Handlers;
using Questao5.Application.Interfaces;
using Questao5.Domain.Entities;
using Questao5.Domain.Models;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using Xunit;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Questao5.Tests.Handlers;

public class MovimentarContaCommandHandlerTests
{
    private readonly Mock<IContaCorrenteRepository> _contaCorrenteRepositoryMock;
    private readonly Mock<IMovimentoRepository> _movimentoRepositoryMock;
    private readonly Mock<IIdempotenciaRepository> _idempotenciaRepositoryMock;
    private readonly MovimentarContaHandler _handler;

    public MovimentarContaCommandHandlerTests()
    {
        _contaCorrenteRepositoryMock = new Mock<IContaCorrenteRepository>();
        _movimentoRepositoryMock = new Mock<IMovimentoRepository>();
        _idempotenciaRepositoryMock = new Mock<IIdempotenciaRepository>();
        _handler = new MovimentarContaHandler(
            _contaCorrenteRepositoryMock.Object,
            _movimentoRepositoryMock.Object,
            _idempotenciaRepositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ContaNaoExiste_RetornaErro()
    {
        // Arrange
        _contaCorrenteRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<string>())).ReturnsAsync(() => null);

        var command = new MovimentarContaCommand
        {
            IdRequisicao = Guid.NewGuid().ToString(),
            IdContaCorrente = "123",
            Valor = 100,
            TipoMovimento = 'C'
        };

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal("INVALID_ACCOUNT", result.TipoErro);
        Assert.NotNull(result.MensagemErro);
    }

    [Fact]
    public async Task Handle_ContaInativa_RetornaErro()
    {
        // Arrange
        var contaCorrenteContaInativa = new ContaCorrente(
            idContaCorrente: "123",
            numero: 123,
            nome: "Roberta Skinner",
            ativo: false
        );

        _contaCorrenteRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<string>())).ReturnsAsync(contaCorrenteContaInativa);

        var command = new MovimentarContaCommand
        {
            IdRequisicao = Guid.NewGuid().ToString(),
            IdContaCorrente = "123",
            Valor = 100,
            TipoMovimento = 'C'
        };

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal("INACTIVE_ACCOUNT", result.TipoErro);
        Assert.NotNull(result.MensagemErro);
    }

    [Fact]
    public async Task Handle_ValorInvalido_RetornaErro()
    {
        // Arrange
        var contaCorrenteConta = new ContaCorrente(
            idContaCorrente: "123",
            numero: 123,
            nome: "Roberta Skinner",
            ativo: true
        );

        _contaCorrenteRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<string>())).ReturnsAsync(contaCorrenteConta);

        var command = new MovimentarContaCommand
        {
            IdRequisicao = Guid.NewGuid().ToString(),
            IdContaCorrente = "123",
            Valor = -100,
            TipoMovimento = 'C'
        };

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal("INVALID_VALUE", result.TipoErro);
        Assert.NotNull(result.MensagemErro);
    }

    [Fact]
    public async Task Handle_TipoMovimentoInvalido_RetornaErro()
    {
        // Arrange
        var contaCorrenteConta = new ContaCorrente(
           idContaCorrente: "123",
           numero: 123,
           nome: "Roberta Skinner",
           ativo: true
       );
        _contaCorrenteRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<string>())).ReturnsAsync(contaCorrenteConta);

        var command = new MovimentarContaCommand
        {
            IdRequisicao = Guid.NewGuid().ToString(),
            IdContaCorrente = "123",
            Valor = 100,
            TipoMovimento = 'X'
        };

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal("INVALID_TYPE", result.TipoErro);
        Assert.NotNull(result.MensagemErro);
    }

    [Fact]
    public async Task Handle_DadosValidos_CriaMovimento()
    {
        // Arrange
        var contaCorrenteConta = new ContaCorrente(
           idContaCorrente: "123",
           numero: 123,
           nome: "Roberta Skinner",
           ativo: true
       );

        _contaCorrenteRepositoryMock.Setup(x => x.GetByIdAsync(It.IsAny<string>())).ReturnsAsync(contaCorrenteConta);
        _idempotenciaRepositoryMock.Setup(x => x.GetByChaveAsync(It.IsAny<string>())).ReturnsAsync((Idempotencia)null);

        var command = new MovimentarContaCommand
        {
            IdRequisicao = Guid.NewGuid().ToString(),
            IdContaCorrente = "123",
            Valor = 100,
            TipoMovimento = 'C'
        };

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(result.IdMovimento);
        _movimentoRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Movimento>()), Times.Once);
        _idempotenciaRepositoryMock.Verify(x => x.AddAsync(It.IsAny<Idempotencia>()), Times.Once);
    }
}