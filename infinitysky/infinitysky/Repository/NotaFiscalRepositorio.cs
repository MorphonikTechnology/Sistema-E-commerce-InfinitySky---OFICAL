namespace infinitysky.Repository
{
    using infinitysky.Models;
    using MySql.Data.MySqlClient;
    using Microsoft.Extensions.Configuration;

    public class NotaFiscalRepositorio : INotaFiscalRepositorio
    {
        private readonly string _conexaoMySQL;

        public NotaFiscalRepositorio(IConfiguration configuration)
        {
            // Pega a connection string do appsettings.json
            _conexaoMySQL = configuration.GetConnectionString("ConexaoMySQL");
        }

        public void AdicionarNotaFiscal(NotaFiscal notaFiscal)
        {
            using var conexao = new MySqlConnection(_conexaoMySQL);
            conexao.Open();

            var query = @"
            INSERT INTO NF_tbl (valor_nf, data_emissao, hora_emissao, id_pagamento)
            VALUES (@ValorNF, @DataEmissao, @HoraEmissao, @IdPagamento)";

            using var cmd = new MySqlCommand(query, conexao);

            cmd.Parameters.AddWithValue("@ValorNF", notaFiscal.ValorNF);
            cmd.Parameters.AddWithValue("@DataEmissao", notaFiscal.DataEmissao.ToString("yyyy-MM-dd")); // Garantir formato correto para DATE
            cmd.Parameters.AddWithValue("@HoraEmissao", notaFiscal.HoraEmissao.ToString(@"hh\:mm\:ss")); // Garantir formato correto para TIME
            cmd.Parameters.AddWithValue("@IdPagamento", notaFiscal.IdPagamento);

            cmd.ExecuteNonQuery();
        }
    }
}
