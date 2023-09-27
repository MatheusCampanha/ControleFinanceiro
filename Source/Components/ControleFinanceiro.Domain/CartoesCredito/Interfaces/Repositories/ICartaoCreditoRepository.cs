using ControleFinanceiro.Domain.CartoesCredito.Queries;
using ControleFinanceiro.Domain.CartoesCredito.Queries.Results;
using ControleFinanceiro.Domain.CartoesCredito.Results;
using ControleFinanceiro.Domain.Core.Commands;
using ControleFinanceiro.Domain.Core.Results;

namespace ControleFinanceiro.Domain.CartoesCredito.Interfaces.Repositories
{
    public interface ICartaoCreditoRepository
    {
        Task<Result<CartaoCredito>> Obter(int cartaoCreditoId);

        Result<CartaoCreditoQueryResult> ObterResult(ConsultarCartaoCreditoQuery query);

        Task<Result<CartaoCreditoQueryResult>> ObterResult(int cartaoCreditoId);
        Task<Result<CartaoCreditoQueryResult>> ObterUnique(string nome);

        Task<Result<IncluirCartaoCreditoCommandResult>> Incluir(CartaoCredito cartaoCredito);

        Task<CommandResult> Atualizar(CartaoCredito cartaoCredito);

        Task<CommandResult> Excluir(int cartaoCreditoId);
    }
}