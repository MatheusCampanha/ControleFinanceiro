using ControleFinanceiro.Domain.Core.Notifications;
using System.Text.Json.Serialization;

namespace ControleFinanceiro.Domain.Core.Queries
{
    public class QueryResult<T> : Notifiable where T : class
    {
        [JsonIgnore]
        public readonly string _dominio = typeof(T).Name.ToString().Replace("QueryResult", "");

        protected QueryResult()
        {
            Registros = new List<T>();
        }

        public QueryResult(T registro)
        {
            PaginaAtual = 1;
            Registros = new List<T> { registro };

            RegistrosPorPagina = Registros.Count;
            TotalRegistros = Registros.Count;
        }

        public QueryResult(int paginaAtual, int registrosPorPagina, int totalRegistros, ICollection<T> registros)
        {
            PaginaAtual = paginaAtual;
            RegistrosPorPagina = registrosPorPagina;
            TotalRegistros = totalRegistros;
            Registros = registros;
        }

        public QueryResult(int statusCode, T registro)
        {
            StatusCode = statusCode;
            Registros = new List<T> { registro };
            PaginaAtual = 1;

            if (registro == null)
            {
                StatusCode = 422;
                AddNotification(_dominio, $"Dados de {_dominio} não encontrados");
            }
        }

        public QueryResult(int statusCode, int paginaAtual, int registrosPorPagina, int totalRegistros, ICollection<T> registros)
        {
            StatusCode = statusCode;
            PaginaAtual = paginaAtual;
            RegistrosPorPagina = registrosPorPagina;
            TotalRegistros = totalRegistros;
            Registros = registros;

            if (registros.Count == 0)
            {
                StatusCode = 422;
                AddNotification(_dominio, $"Dados de {_dominio} não encontrados");
            }
        }

        [JsonIgnore]
        public int StatusCode { get; private set; }

        public int PaginaAtual { get; private set; }
        public int RegistrosPorPagina { get; private set; }
        public long TotalRegistros { get; private set; }
        public ICollection<T> Registros { get; private set; }
    }
}