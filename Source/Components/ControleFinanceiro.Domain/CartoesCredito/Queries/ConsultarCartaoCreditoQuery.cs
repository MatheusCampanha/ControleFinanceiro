using ControleFinanceiro.Domain.Core.Queries;

namespace ControleFinanceiro.Domain.CartoesCredito.Queries
{
    public class ConsultarCartaoCreditoQuery : Query
    {
        public ConsultarCartaoCreditoQuery(int? id, string? nome, int? diaFechamento, int? diaVencimento)
        {
            Id = id;
            Nome = nome;
            DiaFechamento = diaFechamento;
            DiaVencimento = diaVencimento;
        }

        public int? Id { get; set; }
        public string? Nome { get; set; }
        public int? DiaFechamento { get; set; }
        public int? DiaVencimento { get; set; }

        public override bool IsValid()
        {
            if (Id <= 0)
                AddNotification(nameof(Id), "Deve ser maior que zero");

            if (DiaFechamento <= 0)
                AddNotification(nameof(DiaFechamento), "Deve ser maior que 0");
            else if (DiaFechamento > 31)
                AddNotification(nameof(DiaFechamento), "Deve ser menor que 31");

            if (DiaVencimento <= 0)
                AddNotification(nameof(DiaVencimento), "Deve ser maior que 0");
            else if (DiaVencimento > 31)
                AddNotification(nameof(DiaVencimento), "Deve ser menor que 31");

            return base.IsValid();
        }
    }
}