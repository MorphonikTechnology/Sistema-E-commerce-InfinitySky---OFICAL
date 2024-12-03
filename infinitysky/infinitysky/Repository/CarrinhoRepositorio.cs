using infinitysky.Models;
using MySql.Data.MySqlClient;

namespace infinitysky.Repository
{
    public class CarrinhoRepositorio : ICarrinhoRepositorio
    {

        private readonly string? _conexaoMySQL;

        public CarrinhoRepositorio(IConfiguration conf) => _conexaoMySQL = conf.GetConnectionString("ConexaoMySQL");

        // Método para adicionar item ao carrinho -> fazendo a inserção na nossa tabela, 
        public void AdicionarItem(CarrinhoViewModel carrinho)
        {
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();

                // o itens_carrinho será a quantidade de planos de um respectivo ID 
                var cmd = new MySqlCommand(
                    "INSERT INTO Carrinho_tbl (itens_carrinho, valor_total_carrinho, id_plano, id_cliente) VALUES (@itens, @valor, @idPlano, @idCliente)", conexao);

                cmd.Parameters.Add("@itens", MySqlDbType.Int32).Value = carrinho.ItensCarrinho;
                cmd.Parameters.Add("@valor", MySqlDbType.Decimal).Value = carrinho.ValorTotalCarrinho;
                cmd.Parameters.Add("@idPlano", MySqlDbType.Int32).Value = carrinho.IdPlano;
                cmd.Parameters.Add("@idCliente", MySqlDbType.Int32).Value = carrinho.IdCliente;

                cmd.ExecuteNonQuery();
            }
        }


        public void RemoverItem(int idCarrinho)
        {
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();
                var cmd = new MySqlCommand("DELETE FROM Carrinho_tbl WHERE id_carrinho = @idCarrinho", conexao);
                cmd.Parameters.Add("@idCarrinho", MySqlDbType.Int32).Value = idCarrinho;

                cmd.ExecuteNonQuery();
            }
        }

        // Método para atualizar itens no carrinho
        // Criado pq precisa de atualização por conta do usuário poder acresentar ou diminir do carrinho um plano especifico 
        // Ao decorrer percebi que se diminuir ou almentar o valor tambem vai mudar 
        // Pq o valor esta sendo calcula de acordo com o Valor de plano vezes a quantidad
        // Update para atualizar os dados
        public void AtualizarItem(CarrinhoViewModel carrinho)
        {
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();
                var cmd = new MySqlCommand(
                    "UPDATE Carrinho_tbl SET itens_carrinho = @itens, valor_total_carrinho = @valor WHERE id_carrinho = @idCarrinho", conexao);

                cmd.Parameters.Add("@itens", MySqlDbType.Int32).Value = carrinho.ItensCarrinho;
                cmd.Parameters.Add("@valor", MySqlDbType.Decimal).Value = carrinho.ValorTotalCarrinho;
                cmd.Parameters.Add("@idCarrinho", MySqlDbType.Int32).Value = carrinho.IdCarrinho;

                cmd.ExecuteNonQuery();
            }
        }


        // Método para obter os itens do carrinho de acordo com o clinte
        // Feito para carregar o carrinho só do cliente que esta logado atraves do id 
        // Caso for utilizar, mas provavelmente não pois a tabela carrinho esta funcionando somente com registro
        public IEnumerable<CarrinhoViewModel> ObterItensCarrinho(int idCliente)
        {
            var itensCarrinho = new List<CarrinhoViewModel>();

            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();
                var cmd = new MySqlCommand("SELECT * FROM Carrinho_tbl WHERE id_cliente = @idCliente", conexao);
                cmd.Parameters.Add("@idCliente", MySqlDbType.Int32).Value = idCliente;

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var carrinho = new CarrinhoViewModel
                        {
                            IdCarrinho = reader.GetInt32("id_carrinho"),
                            ItensCarrinho = reader.GetInt32("itens_carrinho"),
                            ValorTotalCarrinho = reader.GetDecimal("valor_total_carrinho"),
                            IdPlano = reader.GetInt32("id_plano"),
                            IdCliente = reader.GetInt32("id_cliente")
                        };

                        itensCarrinho.Add(carrinho); // Passando para a variavel 
                    }
                }
            }

            return itensCarrinho;
        }

        // Método para remover item do carrinho
        // Vai deletar todo carrinho de um respectivo do carrinho 
        // Deleta Por Completo 
        // Ele vai estar no Icone de lixeira na Página "Carrinho" 
        public void DeletarCarrinhoPorId(int idCarrinho)
        {
            using var conexao = new MySqlConnection(_conexaoMySQL);
            conexao.Open();

            using var transacao = conexao.BeginTransaction();
            try
            {
                // Exclui registros dependentes na tabela nota_fiscal_tbl
                var deleteNotaFiscalQuery = @"
            DELETE FROM NF_tbl
            WHERE id_pagamento IN (
                SELECT id_pagamento
                FROM pagamento_tbl
                WHERE id_carrinho = @IdCarrinho
            );";
                using var cmdNotaFiscal = new MySqlCommand(deleteNotaFiscalQuery, conexao, transacao);
                cmdNotaFiscal.Parameters.AddWithValue("@IdCarrinho", idCarrinho);
                cmdNotaFiscal.ExecuteNonQuery();

                // Exclui registros na tabela pagamento_tbl
                var deletePagamentoQuery = "DELETE FROM pagamento_tbl WHERE id_carrinho = @IdCarrinho;";
                using var cmdPagamento = new MySqlCommand(deletePagamentoQuery, conexao, transacao);
                cmdPagamento.Parameters.AddWithValue("@IdCarrinho", idCarrinho);
                cmdPagamento.ExecuteNonQuery();

                // Exclui registros na tabela carrinho_tbl
                var deleteCarrinhoQuery = "DELETE FROM carrinho_tbl WHERE id_carrinho = @IdCarrinho;";
                using var cmdCarrinho = new MySqlCommand(deleteCarrinhoQuery, conexao, transacao);
                cmdCarrinho.Parameters.AddWithValue("@IdCarrinho", idCarrinho);
                cmdCarrinho.ExecuteNonQuery();

                // Confirma a transação
                transacao.Commit();
            }
            catch (Exception ex)
            {
                // Reverte a transação em caso de erro
                transacao.Rollback();
                throw new Exception("Erro ao excluir o carrinho: " + ex.Message, ex);
            }
        }


        public CarrinhoViewModel ObterCarrinhoPorId(int idCarrinho)
        {
            CarrinhoViewModel carrinho = null;

            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();
                var cmd = new MySqlCommand("SELECT * FROM Carrinho_tbl WHERE id_carrinho = @idCarrinho", conexao);
                cmd.Parameters.Add("@idCarrinho", MySqlDbType.Int32).Value = idCarrinho;

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        carrinho = new CarrinhoViewModel
                        {
                            IdCarrinho = reader.GetInt32("id_carrinho"),
                            ItensCarrinho = reader.GetInt32("itens_carrinho"),
                            ValorTotalCarrinho = reader.GetDecimal("valor_total_carrinho"),
                            IdPlano = reader.GetInt32("id_plano"),
                            IdCliente = reader.GetInt32("id_cliente")
                        };
                    }
                }
            }

            return carrinho;
        }


    }
}

