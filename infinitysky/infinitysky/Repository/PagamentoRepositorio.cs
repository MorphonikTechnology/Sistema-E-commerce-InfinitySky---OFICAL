using infinitysky.Models;
using Microsoft.EntityFrameworkCore;
using MySql.Data.MySqlClient;
using System.Numerics;

namespace infinitysky.Repository
{
    public class PagamentoRepositorio : IPagamentoRepositorio
    {
        // Declarando a variável para a string de conexão
        private readonly string? _conexaoMySQL;

        // Construtor para inicializar a conexão
        public PagamentoRepositorio(IConfiguration conf) => _conexaoMySQL = conf.GetConnectionString("ConexaoMySQL");

        // Método para adicionar um novo pagamento
        public void Adicionar(Pagamento pagamento)
        {
            using var conexao = new MySqlConnection(_conexaoMySQL);
            conexao.Open();

            var query = @"INSERT INTO Pagamento_tbl (forma_pagamento, status_pagamentos, hora_pagamento, valor_pagamento, id_carrinho)
                          VALUES (@FormaPagamento, @StatusPagamentos, @HoraPagamento, @ValorPagamento, @IdCarrinho)";

            using var comando = new MySqlCommand(query, conexao);
            comando.Parameters.AddWithValue("@FormaPagamento", pagamento.FormaPagamento);
            comando.Parameters.AddWithValue("@StatusPagamentos", pagamento.StatusPagamentos);
            comando.Parameters.AddWithValue("@HoraPagamento", pagamento.HoraPagamento);
            comando.Parameters.AddWithValue("@ValorPagamento", pagamento.ValorPagamento);
            comando.Parameters.AddWithValue("@IdCarrinho", pagamento.IdCarrinho);

            comando.ExecuteNonQuery();
        }

        // Método para atualizar o status de um pagamento
        public void Atualizar(Pagamento pagamento)
        {
            using var conexao = new MySqlConnection(_conexaoMySQL);
            conexao.Open();

            var query = @"UPDATE Pagamento_tbl
                          SET status_pagamentos = @StatusPagamentos, 
                              hora_pagamento = @HoraPagamento,
                              forma_pagamento = @FormaPagamento
                          WHERE id_pagamento = @IdPagamento";

            using var comando = new MySqlCommand(query, conexao);
            comando.Parameters.AddWithValue("@StatusPagamentos", pagamento.StatusPagamentos);
            comando.Parameters.AddWithValue("@HoraPagamento", pagamento.HoraPagamento);
            comando.Parameters.AddWithValue("@FormaPagamento", pagamento.FormaPagamento);
            comando.Parameters.AddWithValue("@IdPagamento", pagamento.IdPagamento);

            comando.ExecuteNonQuery();
        }

        // Método para obter um pagamento por ID
        public Pagamento ObterPorId(int idPagamento)
        {
            using var conexao = new MySqlConnection(_conexaoMySQL);
            conexao.Open();

            var query = @"SELECT * FROM Pagamento_tbl WHERE id_pagamento = @IdPagamento";

            using var comando = new MySqlCommand(query, conexao);
            comando.Parameters.AddWithValue("@IdPagamento", idPagamento);

            using var reader = comando.ExecuteReader();

            if (reader.Read())
            {
                return new Pagamento
                {
                    IdPagamento = Convert.ToInt32(reader["id_pagamento"]),
                    FormaPagamento = reader["forma_pagamento"].ToString(),
                    StatusPagamentos = reader["status_pagamentos"].ToString(),
                    HoraPagamento = TimeSpan.Parse(reader["hora_pagamento"].ToString()),
                    ValorPagamento = Convert.ToDecimal(reader["valor_pagamento"]),
                    IdCarrinho = Convert.ToInt32(reader["id_carrinho"])
                };
            }

            return null;
        }

        // Método para listar pagamentos de um cliente baseado no ID do carrinho
        public IEnumerable<Pagamento> ObterPorCliente(int idCliente)
        {
            Console.WriteLine($"Buscando pagamentos para o cliente ID: {idCliente}");

            using var conexao = new MySqlConnection(_conexaoMySQL);
            conexao.Open();

            var query = @"SELECT p.* 
                  FROM Pagamento_tbl p
                  INNER JOIN Carrinho_tbl c ON p.id_carrinho = c.id_carrinho
                  WHERE c.id_cliente = @IdCliente";

            using var comando = new MySqlCommand(query, conexao);
            comando.Parameters.AddWithValue("@IdCliente", idCliente);

            using var reader = comando.ExecuteReader();

            var pagamentos = new List<Pagamento>();

            while (reader.Read())
            {
                pagamentos.Add(new Pagamento
                {
                    IdPagamento = Convert.ToInt32(reader["id_pagamento"]),
                    FormaPagamento = reader["forma_pagamento"].ToString(),
                    StatusPagamentos = reader["status_pagamentos"].ToString(),
                    HoraPagamento = TimeSpan.Parse(reader["hora_pagamento"].ToString()),
                    ValorPagamento = Convert.ToDecimal(reader["valor_pagamento"]),
                    IdCarrinho = Convert.ToInt32(reader["id_carrinho"])
                });
            }

            Console.WriteLine($"Pagamentos encontrados: {pagamentos.Count}");

            return pagamentos;
        }

        public Pagamento ObterPagamentoPorCarrinhoId(int carrinhoId)
        {
            using var conexao = new MySqlConnection(_conexaoMySQL);
            conexao.Open();

            var query = @"SELECT id_pagamento, forma_pagamento, status_pagamentos, hora_pagamento, valor_pagamento, id_carrinho
                  FROM Pagamento_tbl
                  WHERE id_carrinho = @IdCarrinho";

            using var comando = new MySqlCommand(query, conexao);
            comando.Parameters.AddWithValue("@IdCarrinho", carrinhoId);

            using var reader = comando.ExecuteReader();

            if (reader.Read())
            {
                return new Pagamento
                {
                    IdPagamento = reader.GetInt32("id_pagamento"),
                    FormaPagamento = reader.GetString("forma_pagamento"),
                    StatusPagamentos = reader.GetString("status_pagamentos"),
                    HoraPagamento = reader.GetTimeSpan("hora_pagamento"),
                    ValorPagamento = reader.GetDecimal("valor_pagamento"),
                    IdCarrinho = reader.GetInt32("id_carrinho")
                };
            }

            return null;
        }

        public IEnumerable<Pagamento> ObterPagamentosPorClienteId(int clienteId)
        {
            using var conexao = new MySqlConnection(_conexaoMySQL);
            conexao.Open();

            // Consulta que obtém pagamentos associados ao cliente por meio da tabela Carrinho_tbl
            var query = @"
        SELECT p.id_pagamento, 
               p.forma_pagamento, 
               p.status_pagamentos, 
               p.hora_pagamento, 
               p.valor_pagamento, 
               p.id_carrinho
        FROM Pagamento_tbl p
        INNER JOIN Carrinho_tbl c ON p.id_carrinho = c.id_carrinho
        WHERE c.id_cliente = @ClienteId";

            using var comando = new MySqlCommand(query, conexao);
            comando.Parameters.AddWithValue("@ClienteId", clienteId);

            using var reader = comando.ExecuteReader();
            var pagamentos = new List<Pagamento>();

            while (reader.Read())
            {
                pagamentos.Add(new Pagamento
                {
                    IdPagamento = reader.GetInt32("id_pagamento"),
                    FormaPagamento = reader.GetString("forma_pagamento"),
                    StatusPagamentos = reader.GetString("status_pagamentos"),
                    HoraPagamento = reader.GetTimeSpan("hora_pagamento"),
                    ValorPagamento = reader.GetDecimal("valor_pagamento"),
                    IdCarrinho = reader.GetInt32("id_carrinho")
                });
            }

            return pagamentos;
        }

       

    }
}
