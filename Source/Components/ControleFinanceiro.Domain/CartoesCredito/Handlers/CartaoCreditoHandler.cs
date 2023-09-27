using ControleFinanceiro.Domain.CartoesCredito.Commands;
using ControleFinanceiro.Domain.CartoesCredito.Interfaces.Handlers;
using ControleFinanceiro.Domain.CartoesCredito.Interfaces.Repositories;
using ControleFinanceiro.Domain.CartoesCredito.Results;
using ControleFinanceiro.Domain.Core.Commands;
using ControleFinanceiro.Domain.Core.Results;
using System.Net;

namespace ControleFinanceiro.Domain.CartoesCredito.Handlers
{
    public class CartaoCreditoHandler : ICartaoCreditoHandler
    {
        private readonly ICartaoCreditoRepository _cartaoCreditoRepository;

        public CartaoCreditoHandler(ICartaoCreditoRepository cartaoCreditoRepository)
        {
            _cartaoCreditoRepository = cartaoCreditoRepository;
        }

        public async Task<Result<IncluirCartaoCreditoCommandResult>> Handle(IncluirCartaoCreditoCommand command)
        {
            var errorResult = new Result<IncluirCartaoCreditoCommandResult>(422);

            if (!command.IsValid())
            {
                errorResult.AddNotifications(command);
                return errorResult;
            }

            var uniqueResult = await _cartaoCreditoRepository.ObterUnique(command.Nome);
            if (uniqueResult.StatusCode == (int)HttpStatusCode.OK)
            {
                errorResult.AddNotification(nameof(command.Nome), "Duplicado");
                return errorResult;
            }

            var cartaoCredito = new CartaoCredito(command.Nome, command.DiaFechamento, command.DiaVencimento);

            if (cartaoCredito.Valid)
                return await _cartaoCreditoRepository.Incluir(cartaoCredito);

            errorResult.AddNotifications(cartaoCredito);
            return errorResult;
        }

        public async Task<CommandResult> Handle(AlterarCartaoCreditoCommand command)
        {
            var errorResult = new CommandResult(422);

            if (!command.IsValid())
            {
                errorResult.AddNotifications(command);
                return errorResult;
            }

            var cartaoCreditoResult = await _cartaoCreditoRepository.Obter(command.Id);
            if (cartaoCreditoResult.StatusCode != (int)HttpStatusCode.OK)
            {
                errorResult.AddNotifications(cartaoCreditoResult);
                return errorResult;
            }

            var uniqueResult = await _cartaoCreditoRepository.ObterUnique(command.Nome);
            var unique = uniqueResult.QueryResult?.Registros?.FirstOrDefault();
            if (uniqueResult.StatusCode == (int)HttpStatusCode.OK && unique is not null && unique.Id != command.Id)
            {
                errorResult.AddNotification(nameof(command.Nome), "Duplicado");
                return errorResult;
            }

            var cartaoCredito = cartaoCreditoResult.QueryResult.Registros.First();

            cartaoCredito.DefinirNome(command.Nome);
            cartaoCredito.DefinirDiaFechamento(command.DiaFechamento);
            cartaoCredito.DefinirDiaVencimento(command.DiaVencimento);

            if (cartaoCredito.Valid)
                return await _cartaoCreditoRepository.Atualizar(cartaoCredito);

            errorResult.AddNotifications(cartaoCredito);
            return errorResult;
        }

        public async Task<CommandResult> Handle(int cartaoCreditoId)
        {
            var errorResult = new CommandResult(422);

            if (cartaoCreditoId <= 0)
            {
                errorResult.AddNotification(nameof(cartaoCreditoId), "Deve ser maior que zero");
                return errorResult;
            }

            var cartaoCreditoResult = await _cartaoCreditoRepository.ObterResult(cartaoCreditoId);
            if (cartaoCreditoResult.StatusCode != (int)HttpStatusCode.OK)
            {
                errorResult.AddNotifications(cartaoCreditoResult);
                return errorResult;
            }

            return await _cartaoCreditoRepository.Excluir(cartaoCreditoId);
        }
    }
}