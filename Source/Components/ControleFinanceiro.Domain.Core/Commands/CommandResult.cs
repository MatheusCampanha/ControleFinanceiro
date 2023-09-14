using ControleFinanceiro.Domain.Core.Notifications;
using System.Text.Json.Serialization;

namespace ControleFinanceiro.Domain.Core.Commands
{
    public class CommandResult : Notifiable
    {
        public CommandResult(int statusCode)
        {
            StatusCode = statusCode;
        }

        public CommandResult(int statusCode, Notifiable item)
        {
            StatusCode = statusCode;
            AddNotifications(item);
        }

        [JsonIgnore]
        public int StatusCode { get; set; }
    }

    public class CommandResult<T> : Notifiable
    {
        public CommandResult(int statusCode, int paginaAtual, int registrosPorPagina, long totalRegistros, ICollection<T> registros)
        {
            StatusCode = statusCode;
            PaginaAtual = paginaAtual;
            RegistrosPorPagina = registrosPorPagina;
            TotalRegistros = totalRegistros;
            Registros = registros;
        }

        [JsonIgnore]
        public int StatusCode { get; set; }

        public int PaginaAtual { get; set; }
        public int RegistrosPorPagina { get; set; }
        public long TotalRegistros { get; set; }
        public ICollection<T> Registros { get; set; }
    }
}