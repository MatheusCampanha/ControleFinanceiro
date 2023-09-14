using ControleFinanceiro.Domain.Core.Notifications;

namespace ControleFinanceiro.Domain.Core.Queries
{
    public class Query : Notifiable
    {
        public int PaginaAtual { get; set; } = 1;
        public int RegistrosPorPagina { get; set; } = 200;

        public virtual bool IsValid()
        {
            if (PaginaAtual < 1)
                AddNotification("PaginaAtual", "Pagina Atual deve ser maior que zero");

            if (RegistrosPorPagina < 1)
                AddNotification("RegistrosPorPagina", "Registros Por Pagina deve ser maior que 0");

            return Valid;
        }
    }
}