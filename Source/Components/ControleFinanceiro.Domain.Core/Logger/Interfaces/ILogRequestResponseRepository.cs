namespace ControleFinanceiro.Domain.Core.Logger.Interfaces
{
    public interface ILogRequestResponseRepository
    {
        Task Inserir(LogRequestResponse entidade);
    }
}