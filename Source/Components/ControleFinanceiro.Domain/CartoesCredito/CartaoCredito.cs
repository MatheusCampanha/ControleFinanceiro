using ControleFinanceiro.Domain.Core.Entities;

namespace ControleFinanceiro.Domain.CartoesCredito
{
    public class CartaoCredito : Entity
    {
        public CartaoCredito(int id, string nome, int diaFechamento, int diaVencimento)
        {
            DefinirId(id);
            DefinirNome(nome);
            DefinirDiaFechamento(diaFechamento);
            DefinirDiaVencimento(diaVencimento);
        }

        public CartaoCredito(string nome, int diaFechamento, int diaVencimento)
        {
            DefinirNome(nome);
            DefinirDiaFechamento(diaFechamento);
            DefinirDiaVencimento(diaVencimento);
        }

        public int Id { get; private set; }
        public string Nome { get; private set; } = default!;
        public int DiaFechamento { get; private set; }
        public int DiaVencimento { get; private set; }

        public void DefinirId(int id)
        {
            if (id <= 0)
                AddNotification(nameof(id), "Deve ser maior que zero");

            if (Valid)
                Id = id;
        }

        public void DefinirNome(string nome)
        {
            if (Valid)
                Nome = nome;
        }

        public void DefinirDiaFechamento(int diaFechamento)
        {
            if (diaFechamento <= 0)
                AddNotification(nameof(diaFechamento), "Deve ser maior que 0");
            else if (diaFechamento > 31)
                AddNotification(nameof(diaFechamento), "Deve ser menor que 31");

            if (Valid)
                DiaFechamento = diaFechamento;
        }

        public void DefinirDiaVencimento(int diaVencimento)
        {
            if (diaVencimento <= 0)
                AddNotification(nameof(diaVencimento), "Deve ser maior que 0");
            else if (diaVencimento > 31)
                AddNotification(nameof(diaVencimento), "Deve ser menor que 31");

            if (Valid)
                DiaVencimento = diaVencimento;
        }
    }
}