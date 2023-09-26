using ElmahCore;

namespace ControleFinanceiro.Domain.Core.Interfaces
{
    public interface IElmahRepository
    {
        string LogError(Error error);
    }
}