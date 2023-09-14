using ControleFinanceiro.Domain.Core.Notifications;
using ControleFinanceiro.Domain.Core.Queries;
using System.Text.Json.Serialization;

namespace ControleFinanceiro.Domain.Core.Results
{
    public class Result<T> : Notifiable where T : class
    {
        public Result(int statusCode)
        {
            StatusCode = statusCode;
        }

        public Result(int statusCode, T? registro)
        {
            StatusCode = statusCode;

            if (registro == null)
            {
                StatusCode = 422;
                AddNotification(_dominio, $"Dados de {_dominio} não encontrados");
            }
            else
            {
                QueryResult = new QueryResult<T>(registro);
            }
        }

        public Result(int statusCode, int paginaAtual, int registrosPorPagina, int totalRegistros, ICollection<T> registros)
        {
            StatusCode = statusCode;

            QueryResult = new QueryResult<T>(paginaAtual, registrosPorPagina, totalRegistros, registros);

            if (registros.Count == 0)
            {
                StatusCode = 422;
                AddNotification(_dominio, $"Dados de {_dominio} não encontrados");
            }
        }

        [JsonIgnore]
        public readonly string _dominio = typeof(T).Name.ToString().Replace("QueryResult", "");

        [JsonIgnore]
        public int StatusCode { get; private set; }

        public QueryResult<T> QueryResult { get; private set; } = default!;
    }
}