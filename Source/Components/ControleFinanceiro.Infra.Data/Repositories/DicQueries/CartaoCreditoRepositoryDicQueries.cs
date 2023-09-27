namespace ControleFinanceiro.Infra.Data.Repositories.DicQueries
{
    public static class CartaoCreditoRepositoryDicQueries
    {
        #region Obter

        public const string ObterPorId = @"
            SELECT
                 c.[Id]
                ,c.[Nome]
                ,c.[DiaFechamento]
                ,c.[DiaVencimento]
            FROM
                [dbo].[CartaoCredito] AS c WITH(NOLOCK)
            WHERE
                c.[Id] = @Id";

        public const string ObterUnique = @"
            SELECT
                 c.[Id]
                ,c.[Nome]
                ,c.[DiaFechamento]
                ,c.[DiaVencimento]
            FROM
                [dbo].[CartaoCredito] AS c WITH(NOLOCK)
            WHERE
                c.[Nome] = @Nome";

        public const string ObterPaginado = @"
            SELECT
                 ROW_NUMBER() OVER (ORDER BY c.ContaId) AS RowNum
                ,c.[Id]
                ,c.[Nome]
                ,c.[DiaFechamento]
                ,c.[DiaVencimento]
            INTO
                #cartaoCredito
            FROM
                [dbo].[CartaoCredito] AS c WITH(NOLOCK)
            WHERE
                c.[Id] = ISNULL(@Id, c.[Id])
            AND c.[Nome] LIKE ('%' + ISNULL(REPLACE(@Nome, ' ', '%'), c.[Nome]) + '%')
            AND c.[DiaFechamento] = ISNULL(@DiaFechamento, c.[DiaFechamento])
            AND c.[DiaVencimento] = ISNULL(@DiaVencimento, c.[DiaVencimento])

            SELECT
                Count(1) as TotalRegistros,
                CEILING((Count(1) / CAST(@REGISTROPORPAGINA AS DECIMAL(10,2)))) as TotalPaginas
            FROM
                #cartaoCredito

            SELECT
                 c.[Id]
                ,c.[Nome]
                ,c.[DiaFechamento]
                ,c.[DiaVencimento]
            FROM
                #cartaoCredito AS c
            WHERE
                RowNum > ((@PAGINAATUAL - 1) * @REGISTROPORPAGINA)

            DROP TABLE #cartaoCredito";

        #endregion Obter

        #region Inserir

        public const string Inserir = @"
            INSERT INTO [dbo].[CartaoCredito]
                (Nome
                ,DiaFechamento
                ,DiaVencimento)
            OUTPUT INSERTED.Id
            VALUES
                (@Nome
                ,@DiaFechamento
                ,@DiaVencimento)";

        #endregion Inserir

        #region Alterar

        public const string Alterar = @"
            UPDATE [dbo].[CartaoCredito]
                 SET [Nome] = @Nome
                    ,[DiaFechamento] = @DiaFechamento
                    ,[DiaVencimento] = @DiaVencimento
            WHERE
                [Id] = @Id";

        #endregion Alterar

        #region Excluir

        public const string Excluir = @"
            DELETE FROM [dbo].[CartaoCredito]
            WHERE [Id] = @Id";

        #endregion Excluir
    }
}