namespace ControleFinanceiro.Domain.Core.Data
{
    public class Settings
    {
        public string ApplicationName { get; set; } = default!;
        public string ApplicationBasePath { get; set; } = default!;
        public bool ConsoleLog { get; set; }
        public string Token { get; set; } = default!;
        public string CorrelationDefaultName { get; set; } = default!;
        public string[] PathException { get; set; } = default!;
        public string[] LogRequestHeaders { get; set; } = default!;

        public ConnectionStrings ConnectionStrings { get; set; } = default!;
    }

    public class ConnectionStrings
    {
        public string ControleFinanceiroDB { get; set; } = default!;
    }
}