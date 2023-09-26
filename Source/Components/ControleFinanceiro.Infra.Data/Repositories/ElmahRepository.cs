using ControleFinanceiro.Domain.Core.Data;
using ControleFinanceiro.Domain.Core.Interfaces;
using ElmahCore;
using ElmahCore.Sql;

namespace ControleFinanceiro.Infra.Data.Repositories
{
    public class ElmahRepository : IElmahRepository
    {
        private readonly ErrorLog _errorLog;

        public ElmahRepository(Settings settings)
        {
            _errorLog = new SqlErrorLog(settings.ConnectionStrings.ControleFinanceiroDB)
            {
                ApplicationName = settings.ApplicationName
            };
        }

        public string LogError(Error error)
        {
            try
            {
                return _errorLog.Log(error);
            }
            catch (Exception ex)
            {
                throw new SystemException(ex.Message);
            }
        }
    }
}