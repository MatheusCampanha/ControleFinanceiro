using ControleFinanceiro.Domain.CartoesCredito.Commands;
using ControleFinanceiro.Domain.CartoesCredito.Results;
using ControleFinanceiro.Domain.Core.Commands;
using ControleFinanceiro.Domain.Core.Results;

namespace ControleFinanceiro.Domain.CartoesCredito.Interfaces.Handlers
{
    public interface ICartaoCreditoHandler
    {
        Task<Result<IncluirCartaoCreditoCommandResult>> Handle(IncluirCartaoCreditoCommand command);

        Task<CommandResult> Handle(AlterarCartaoCreditoCommand command);

        Task<CommandResult> Handle(int cartaoCreditoId);
    }
}