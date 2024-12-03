
using infinitysky.Models;
using MySql.Data.MySqlClient;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace infinitysky.Repository
{
    public class FavoritosRepositorio : IFavoritosRepositorio
    {

        //declarando a varival de da string de conxão

        private readonly string? _conexaoMySQL;

        //metodo da conexão com banco de dados
        public FavoritosRepositorio(IConfiguration conf) => _conexaoMySQL = conf.GetConnectionString("ConexaoMySQL");


        // Metodo para adicionar o favorito 
        // declarando a classe ViewModel presente/instanciada na página Favoritos
        // junto com a variavel
        public void AdicionarFavorito(FavoritosViewModel favorito)
        {
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();

                // Fazendo insert na nossa tabela favoritos, menso o id pq é automatico
                var cmd = new MySqlCommand(
                    "INSERT INTO Favorito_tbl (status_favorito, id_plano, id_cliente) VALUES (@status, @idPlano, @idCliente)", conexao);

                cmd.Parameters.Add("@status", MySqlDbType.VarChar).Value = favorito.StatusFavorito;
                cmd.Parameters.Add("@idPlano", MySqlDbType.Int32).Value = favorito.IdPlano;
                cmd.Parameters.Add("@idCliente", MySqlDbType.Int32).Value = favorito.IdCliente;

                cmd.ExecuteNonQuery();
            }
        }



        // Metodo para obter os favoritos de um determinado cliente e plano, atraves do id de ambos
        // Para uso de verificação na home controller 
        public FavoritosViewModel ObterFavorito(int clienteId, int idPlano)
        {
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();


                // select para consultar 
                // where para especificar o cliente
                // and pois são 2 condições do cliente e do plano 
                var cmd = new MySqlCommand(
                    "SELECT * FROM Favorito_tbl WHERE id_cliente = @idCliente AND id_plano = @idPlano", conexao);

                cmd.Parameters.Add("@idCliente", MySqlDbType.Int32).Value = clienteId;
                cmd.Parameters.Add("@idPlano", MySqlDbType.Int32).Value = idPlano;

                // Executa a consulta e obtém o resultado
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read()) // ve se algum registro foi retornado pela consulta
                    {
                        return new FavoritosViewModel // Retorna um objeto FavoritosViewModel populado com os dados do registro encontrado

                        {
                            IdFavorito = reader.GetInt32("id_favorito"), // PEga o id do favorito e em todos os campos em baixo 
                            StatusFavorito = reader.GetString("status_favorito"),
                            IdPlano = reader.GetInt32("id_plano"),
                            IdCliente = reader.GetInt32("id_cliente")
                        };
                    }
                }
            }
            return null;  // Se nao foi encontrado algum registro nenhum registro vai retornar 1
        }



        // método responsável por retornar uma lista de todos os favoritos de um cliente especifico 
        // para a pagina Favoritos 

        public List<FavoritosViewModel> ObterFavoritosPorCliente(int idCliente)
        {
            var favoritos = new List<FavoritosViewModel>();   // Cria uma lista para armazenar os favoritos do cliente

            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();

                //Utilizando o SELECT para a consulta 
                // wHERE para especificar o cliente atraves do ID 
                var cmd = new MySqlCommand("SELECT * FROM Favorito_tbl WHERE id_cliente = @idCliente", conexao);
                cmd.Parameters.Add("@idCliente", MySqlDbType.Int32).Value = idCliente;

                // Executa a consulta e obtém o resultado
                using (var reader = cmd.ExecuteReader())
                {
                    // Utilizando um Laço de repetição -> enquanto estiver registros 
                    while (reader.Read())
                    {
                        // Adiciona cada favorito encontrado na lista 'favoritos'
                        favoritos.Add(new FavoritosViewModel
                        {
                            IdFavorito = reader.GetInt32("id_favorito"),
                            StatusFavorito = reader.GetString("status_favorito"),
                            IdPlano = reader.GetInt32("id_plano"),
                            IdCliente = reader.GetInt32("id_cliente")
                        });
                    }
                }
            }

            return favoritos;  // retorna a lista 
        }



        // Metodo para apagar por completo 
        // Atraves do id do favorito (user o DELETE FROM) 
        // Semelhante ao Carrinho 
        // utilizar o WHERE para evitar que delete outros 
        public void RemoverFavorito(int idFavorito) // Removendo pelo id 
        {
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();
                using (var cmd = new MySqlCommand("DELETE FROM Favorito_tbl WHERE id_favorito = @IdFavorito", conexao))
                {
                    cmd.Parameters.Add("@IdFavorito", MySqlDbType.Int32).Value = idFavorito;
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }

}