using ControleFinanceiro.Domain.CartoesCredito;
using ControleFinanceiro.Domain.CartoesCredito.Interfaces.Repositories;
using ControleFinanceiro.Domain.CartoesCredito.Queries;
using ControleFinanceiro.Domain.CartoesCredito.Queries.Results;
using ControleFinanceiro.Domain.CartoesCredito.Results;
using ControleFinanceiro.Domain.Core.Commands;
using ControleFinanceiro.Domain.Core.Data;
using ControleFinanceiro.Domain.Core.Results;
using ControleFinanceiro.Infra.Data.Repositories.DicQueries;
using Dapper;
using System.Data;
using System.Data.SqlClient;
using System.Net;

namespace ControleFinanceiro.Infra.Data.Repositories
{
    public class CartaoCreditoRepository : ICartaoCreditoRepository
    {
        private readonly Settings _settings;

        public CartaoCreditoRepository(Settings settings)
        {
            _settings = settings;
        }

        public async Task<Result<CartaoCredito>> Obter(int cartaoCreditoId)
        {
            using var connection = new SqlConnection(_settings.ConnectionStrings.ControleFinanceiroDB);
            var param = new DynamicParameters();
            param.Add("@Id", cartaoCreditoId, DbType.Int32);

            var result = await connection.QueryFirstOrDefaultAsync<CartaoCredito>(CartaoCreditoRepositoryDicQueries.ObterPorId, param, commandType: CommandType.Text);

            int statusCode = result is null ? HttpStatusCode.NoContent.GetHashCode() : HttpStatusCode.OK.GetHashCode();

            return new Result<CartaoCredito>(statusCode, result);
        }

        public Result<CartaoCreditoQueryResult> ObterResult(ConsultarCartaoCreditoQuery query)
        {
            using var connection = new SqlConnection(_settings.ConnectionStrings.ControleFinanceiroDB);
            var param = new DynamicParameters();
            param.Add("@REGISTROPORPAGINA", query.RegistrosPorPagina);
            param.Add("@PAGINAATUAL", query.PaginaAtual);

            param.Add("@Id", query.Id, DbType.Int32);
            param.Add("@Nome", query.Nome, DbType.String);
            param.Add("@DiaFechamento", query.DiaFechamento, DbType.Int32);
            param.Add("@DiaVencimento", query.DiaVencimento, DbType.Int32);

            using var multi = connection.QueryMultiple(CartaoCreditoRepositoryDicQueries.ObterPaginado, param, commandType: CommandType.Text);

            var totalRegistros = multi.Read<int>().First();

            var itens = multi.Read<CartaoCreditoQueryResult>().ToList();

            int statusCode = itens.Count <= 0 ? HttpStatusCode.UnprocessableEntity.GetHashCode() : HttpStatusCode.PartialContent.GetHashCode();

            return new Result<CartaoCreditoQueryResult>(statusCode, query.PaginaAtual, query.RegistrosPorPagina, totalRegistros, itens);
        }

        public async Task<Result<CartaoCreditoQueryResult>> ObterResult(int cartaoCreditoId)
        {
            using var connection = new SqlConnection(_settings.ConnectionStrings.ControleFinanceiroDB);
            var param = new DynamicParameters();
            param.Add("@Id", cartaoCreditoId, DbType.Int32);

            var result = await connection.QueryFirstOrDefaultAsync<CartaoCreditoQueryResult>(CartaoCreditoRepositoryDicQueries.ObterPorId, param, commandType: CommandType.Text);

            int statusCode = result is null ? HttpStatusCode.NoContent.GetHashCode() : HttpStatusCode.OK.GetHashCode();

            return new Result<CartaoCreditoQueryResult>(statusCode, result);
        }

        public async Task<Result<CartaoCreditoQueryResult>> ObterUnique(string nome)
        {
            using var connection = new SqlConnection(_settings.ConnectionStrings.ControleFinanceiroDB);
            var param = new DynamicParameters();
            param.Add("@Nome", nome, DbType.String);

            var result = await connection.QueryFirstOrDefaultAsync<CartaoCreditoQueryResult>(CartaoCreditoRepositoryDicQueries.ObterPorId, param, commandType: CommandType.Text);

            int statusCode = result is null ? HttpStatusCode.NoContent.GetHashCode() : HttpStatusCode.OK.GetHashCode();

            return new Result<CartaoCreditoQueryResult>(statusCode, result);
        }

        public async Task<Result<IncluirCartaoCreditoCommandResult>> Incluir(CartaoCredito cartaoCredito)
        {
            using var connection = new SqlConnection(_settings.ConnectionStrings.ControleFinanceiroDB);
            var param = new DynamicParameters();
            param.Add("@Nome", cartaoCredito.Nome, DbType.String);
            param.Add("@DiaFechamento", cartaoCredito.DiaFechamento, DbType.Int32);
            param.Add("@DiaVencimento", cartaoCredito.DiaVencimento, DbType.Int32);

            var cartaoCreditoId = await connection.QuerySingleAsync<int>(CartaoCreditoRepositoryDicQueries.Inserir, param, commandType: CommandType.Text);

            return new Result<IncluirCartaoCreditoCommandResult>(HttpStatusCode.Created.GetHashCode(), new IncluirCartaoCreditoCommandResult(cartaoCreditoId, cartaoCredito.Nome, cartaoCredito.DiaFechamento, cartaoCredito.DiaVencimento));
        }

        public async Task<CommandResult> Atualizar(CartaoCredito cartaoCredito)
        {
            using var connection = new SqlConnection(_settings.ConnectionStrings.ControleFinanceiroDB);
            var param = new DynamicParameters();
            param.Add("@Id", cartaoCredito.Id, DbType.Int32);
            param.Add("@Nome", cartaoCredito.Nome, DbType.String);
            param.Add("@DiaFechamento", cartaoCredito.DiaFechamento, DbType.Int32);
            param.Add("@DiaVencimento", cartaoCredito.DiaVencimento, DbType.Int32);

            await connection.ExecuteAsync(CartaoCreditoRepositoryDicQueries.Alterar, param, commandType: CommandType.Text);

            return new CommandResult(HttpStatusCode.NoContent.GetHashCode());
        }

        public async Task<CommandResult> Excluir(int cartaoCreditoId)
        {
            using var connection = new SqlConnection(_settings.ConnectionStrings.ControleFinanceiroDB);
            var param = new DynamicParameters();
            param.Add("@Id", cartaoCreditoId, DbType.Int32);

            await connection.ExecuteAsync(CartaoCreditoRepositoryDicQueries.Excluir, param, commandType: CommandType.Text);

            return new CommandResult(HttpStatusCode.NoContent.GetHashCode());
        }
    }
}