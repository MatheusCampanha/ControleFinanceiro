namespace ControleFinanceiro.Domain.CartoesCredito.Results
{
    public class IncluirCartaoCreditoCommandResult
    {
        public IncluirCartaoCreditoCommandResult(int id, string nome, int diaFechamento, int diaVencimento)
        {
            Id = id;
            Nome = nome;
            DiaFechamento = diaFechamento;
            DiaVencimento = diaVencimento;
        }

        public int Id { get; private set; }
        public string Nome { get; private set; }
        public int DiaFechamento { get; private set; }
        public int DiaVencimento { get; private set; }
    }
}