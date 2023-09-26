namespace ControleFinanceiro.Infra.Data.Repositories.DicQueries
{
    public static class LogRequestResponseRepositoryDicQueries
    {
        public const string Inserir = @"
            INSERT INTO LogRequestResponse
                (DataEnvio
                ,DataRecebimento
                ,EndPoint
                ,Method
                ,StatusCodeResponse
                ,Request
                ,Response
                ,TempoDuracao
                ,MachineName
                ,ErrorId
                ,CorrelationId)
            VALUES
                (@DataEnvio
                ,@DataRecebimento
                ,@EndPoint
                ,@Method
                ,@StatusCodeResponse
                ,@Request
                ,@Response
                ,@TempoDuracao
                ,@MachineName
                ,@ErrorId
                ,@CorrelationId)";
    }
}