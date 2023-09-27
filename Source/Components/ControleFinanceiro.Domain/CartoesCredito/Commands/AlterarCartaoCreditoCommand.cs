using ControleFinanceiro.Domain.Core.Commands;
using System.Text.Json.Serialization;

namespace ControleFinanceiro.Domain.CartoesCredito.Commands
{
    public class AlterarCartaoCreditoCommand : Command
    {
        public AlterarCartaoCreditoCommand(string nome, int diaFechamento, int diaVencimento)
        {
            Nome = nome;
            DiaFechamento = diaFechamento;
            DiaVencimento = diaVencimento;
        }

        [JsonIgnore]
        public int Id { get; set; }

        public string Nome { get; private set; }
        public int DiaFechamento { get; private set; }
        public int DiaVencimento { get; private set; }

        public override bool IsValid()
        {
            if (DiaFechamento <= 0)
                AddNotification(nameof(DiaFechamento), "Deve ser maior que 0");
            else if (DiaFechamento > 31)
                AddNotification(nameof(DiaFechamento), "Deve ser menor que 31");

            if (DiaVencimento <= 0)
                AddNotification(nameof(DiaVencimento), "Deve ser maior que 0");
            else if (DiaVencimento > 31)
                AddNotification(nameof(DiaVencimento), "Deve ser menor que 31");

            return true;
        }

        public void DefinirCartaoCreditoId(int id)
        {
            if (id <= 0)
                AddNotification(nameof(id), "Deve ser maior que zero");

            if (Valid)
                Id = id;
        }
    }
}