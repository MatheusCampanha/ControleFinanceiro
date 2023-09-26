namespace ControleFinanceiro.Domain.Core.Logger
{
    public class LogRequestResponse
    {
        public LogRequestResponse(string machineName, DateTime dataEnvio, DateTime dataRecebimento, string endPoint, string method, int statusCodeResponse, string request, string? response, string? errorId, string correlationId, long? tempoDuracao)
        {
            MachineName = machineName;
            DataEnvio = dataEnvio;
            DataRecebimento = dataRecebimento;
            EndPoint = endPoint;
            Method = method;
            StatusCodeResponse = statusCodeResponse;
            Request = request;
            Response = response;
            ErrorId = errorId;
            CorrelationId = correlationId;
            TempoDuracao = tempoDuracao;
        }

        public int LogRequestResponseId { get; private set; }
        public string MachineName { get; private set; } = default!;
        public DateTime DataEnvio { get; private set; }
        public DateTime DataRecebimento { get; private set; }
        public string EndPoint { get; private set; } = default!;
        public string Method { get; private set; } = default!;
        public int StatusCodeResponse { get; private set; }
        public string Request { get; private set; } = default!;
        public string? Response { get; private set; }
        public string? ErrorId { get; private set; }
        public string CorrelationId { get; private set; } = default!;
        public long? TempoDuracao { get; private set; }
    }
}