namespace ControleFinanceiro.Domain.Core.Interfaces
{
    public interface ICorrelationContext
    {
        Guid CorrelationId { get; set; }
    }
}