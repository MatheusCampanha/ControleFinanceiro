using ControleFinanceiro.Domain.Core.Notifications;

namespace ControleFinanceiro.Domain.Core.Commands
{
    public abstract class Command : Notifiable
    {
        public abstract bool IsValid();
    }
}