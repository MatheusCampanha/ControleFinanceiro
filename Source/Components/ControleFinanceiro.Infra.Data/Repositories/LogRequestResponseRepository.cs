using ControleFinanceiro.Domain.Core.Data;
using ControleFinanceiro.Domain.Core.Logger;
using ControleFinanceiro.Domain.Core.Logger.Interfaces;
using ControleFinanceiro.Infra.Data.Repositories.DicQueries;
using Dapper;
using System.Data;
using System.Data.SQLite;

namespace ControleFinanceiro.Infra.Data.Repositories
{
    public class LogRequestResponseRepository : ILogRequestResponseRepository
    {
        private readonly Settings _settings;

        public LogRequestResponseRepository(Settings settings)
        {
            _settings = settings;
        }

        public async Task Inserir(LogRequestResponse entidade)
        {
            using var connection = new SQLiteConnection(_settings.ConnectionStrings.ControleFinanceiroDB);
            var param = new DynamicParameters();
            param.Add("@MachineName", entidade.MachineName);
            param.Add("@DataEnvio", entidade.DataEnvio);
            param.Add("@DataRecebimento", entidade.DataRecebimento);
            param.Add("@EndPoint", entidade.EndPoint);
            param.Add("@Method", entidade.Method);
            param.Add("@StatusCodeResponse", entidade.StatusCodeResponse);
            param.Add("@Request", entidade.Request);
            param.Add("@Response", entidade.Response);
            param.Add("@ErrorId", entidade.ErrorId);
            param.Add("@CorrelationId", entidade.CorrelationId);
            param.Add("@TempoDuracao", entidade.TempoDuracao);

            await connection.ExecuteAsync(LogRequestResponseRepositoryDicQueries.Inserir, param, commandType: CommandType.Text);
        }
    }
}