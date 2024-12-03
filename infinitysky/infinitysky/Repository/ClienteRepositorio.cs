using infinitysky.Models;
using MySql.Data.MySqlClient;
using MySqlX.XDevAPI;
using System.Data;

namespace infinitysky.Repository
{
    public class ClienteRepositorio : IClienteRepositorio
    {

        //declarando a varival de da string de conxão

        private readonly string? _conexaoMySQL;

        //metodo da conexão com banco de dados
        public ClienteRepositorio(IConfiguration conf) => _conexaoMySQL = conf.GetConnectionString("ConexaoMySQL");

        //Login Cliente(metodo)

        public Cliente Login(string Email, string Senha)
        {
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();

                // Comando SQL para buscar o ID, email e senha do cliente
                MySqlCommand cmd = new MySqlCommand("SELECT id_cliente, email_cliente, senha_cliente FROM Cliente_tbl WHERE email_cliente = @email_cliente AND senha_cliente = @senha_cliente", conexao);

                cmd.Parameters.Add("@email_cliente", MySqlDbType.VarChar).Value = Email;
                cmd.Parameters.Add("@senha_cliente", MySqlDbType.VarChar).Value = Senha;

                MySqlDataReader dr = cmd.ExecuteReader();

                Cliente cliente = new Cliente();
                if (dr.Read())
                {
                    cliente.Codigo = Convert.ToInt32(dr["id_cliente"]); // Pega o ID do cliente
                    cliente.Email = Convert.ToString(dr["email_cliente"]);
                    cliente.Senha = Convert.ToString(dr["senha_cliente"]);
                }

                return cliente;
            }
        }

        //Cadastrar CLIENTE

        public void Cadastrar(Cliente cliente) //instanciando a classe 
        {
            using (var conexao = new MySqlConnection(_conexaoMySQL))

            {
                conexao.Open();

                //Primeiro temos que inserir na tabela Cliente 
                // Inserir o telefone na tabela Telefone_tbl e recuperar o id gerado
                MySqlCommand cmdTelefone = new MySqlCommand("INSERT INTO Telefone_tbl (telefone_cliente) VALUES (@telefone_cliente); SELECT LAST_INSERT_ID();", conexao);
                cmdTelefone.Parameters.Add("@telefone_cliente", MySqlDbType.VarChar).Value = cliente.Telefone;
                int idTelefone = Convert.ToInt32(cmdTelefone.ExecuteScalar());

                // Agora insira o cliente na tabela Cliente_tbl usando o id gerado para o telefone
                MySqlCommand cmd = new MySqlCommand("INSERT INTO Cliente_tbl (nome_cliente, id_telefone, data_nascimento, cpf_cliente, email_cliente, senha_cliente) VALUES (@nome_cliente, @id_telefone, @data_nascimento, @cpf_cliente, @email_cliente, @senha_cliente)", conexao);

                cmd.Parameters.Add("@nome_cliente", MySqlDbType.VarChar).Value = cliente.Nome;
                cmd.Parameters.Add("@id_telefone", MySqlDbType.Int32).Value = idTelefone;  // Use o idTelefone gerado pela inserção anterior
                cmd.Parameters.Add("@data_nascimento", MySqlDbType.Date).Value = cliente.DataNascimento;
                cmd.Parameters.Add("@cpf_cliente", MySqlDbType.VarChar).Value = cliente.Cpf_Cliente;
                cmd.Parameters.Add("@email_cliente", MySqlDbType.VarChar).Value = cliente.Email;
                cmd.Parameters.Add("@senha_cliente", MySqlDbType.VarChar).Value = cliente.Senha;



                cmd.ExecuteNonQuery();
                conexao.Close();
            }

        }

        //Um metodo para obter por id 
        // Tambem busca todos os dados 
        // Parte do telefone (outra tabela)
        public Cliente ObterClientePorId(int clienteId)
        {
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();

                // Comando para buscar todos os dados do cliente pelo ID
                MySqlCommand cmd = new MySqlCommand(
                    @"SELECT c.nome_cliente, t.telefone_cliente, c.data_nascimento, c.email_cliente, c.cpf_cliente, c.id_cliente 
                    FROM Cliente_tbl c 
              JOIN Telefone_tbl t ON c.id_telefone = t.id_telefone 
              WHERE c.id_cliente = @id_cliente", conexao);


                // Colocando o id do nosso cliente nessa variavel 
                // Variável está sendo utilizanda na HomeController
                cmd.Parameters.Add("@id_cliente", MySqlDbType.Int32).Value = clienteId;

                MySqlDataReader dr = cmd.ExecuteReader();



                // Criando um novo objeto
                // Para armazenar os dados
                // Depois disso, utilizamos para verificar se o cliente foi encontrado 

                Cliente cliente = new Cliente();
                if (dr.Read())
                {
                    cliente.Codigo = Convert.ToInt32(dr["id_cliente"]);
                    cliente.Nome = dr["nome_cliente"].ToString();
                    cliente.Telefone = dr["telefone_cliente"].ToString();
                    cliente.DataNascimento = dr["data_nascimento"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(dr["data_nascimento"]);
                    cliente.Email = dr["email_cliente"].ToString();
                    cliente.Cpf_Cliente = dr["cpf_cliente"].ToString();
                }

                return cliente;
            }
        }


        public Cliente DadosCliente(int clienteId)
        {
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open();

                // Comando SQL para buscar todos os dados do cliente pelo ID
                // Utilizandoo o JOIN 
                MySqlCommand cmd = new MySqlCommand(
                    @"SELECT Cliente_tbl.nome_cliente, Telefone_tbl.telefone_cliente, Cliente_tbl.data_nascimento, 
                   Cliente_tbl.email_cliente, Cliente_tbl.cpf_cliente 
                   FROM Cliente_tbl
                   JOIN Telefone_tbl ON Cliente_tbl.id_telefone = Telefone_tbl.id_telefone 
                   WHERE Cliente_tbl.id_cliente = @id_cliente", conexao);

                cmd.Parameters.Add("@id_cliente", MySqlDbType.Int32).Value = clienteId;

                // Exucuta e armezana no MySqlDataReader
                // Usada para ler dados, percorre todas as linhas 
                MySqlDataReader dr = cmd.ExecuteReader();

                // Criando um novo objeto
                // Para armazenar os dados
                // Depois disse, utilizamos para verificar se o cliente foi encontrado 
                // Se foi, irá preencher os campos 
                Cliente cliente = new Cliente();
                if (dr.Read())
                {
                    cliente.Nome = dr["nome_cliente"].ToString();
                    cliente.Telefone = dr["telefone_cliente"].ToString();
                    cliente.DataNascimento = Convert.ToDateTime(dr["data_nascimento"]);
                    cliente.Email = dr["email_cliente"].ToString();
                    cliente.Cpf_Cliente = dr["cpf_cliente"].ToString();
                }

                return cliente; // retorna o obejto 
            }
        }


        public void AtualizarCliente(Cliente cliente)
        {
            using (var conexao = new MySqlConnection(_conexaoMySQL))
            {
                conexao.Open(); // Abre a conexão com o banco de dados para permitir operações SQL

                // Atualizando os dados na tabela Cliente_tbl
                MySqlCommand cmd = new MySqlCommand(
                    // Comando SQL para atualizar as informações do cliente
                    @"UPDATE Cliente_tbl 
            SET nome_cliente = @nome_cliente, 
            data_nascimento = @data_nascimento, 
            cpf_cliente = @cpf_cliente, 
            email_cliente = @email_cliente 
            WHERE id_cliente = @id_cliente", conexao);


                cmd.Parameters.Add("@nome_cliente", MySqlDbType.VarChar).Value = cliente.Nome;
                cmd.Parameters.Add("@data_nascimento", MySqlDbType.Date).Value = cliente.DataNascimento;
                cmd.Parameters.Add("@cpf_cliente", MySqlDbType.VarChar).Value = cliente.Cpf_Cliente;
                cmd.Parameters.Add("@email_cliente", MySqlDbType.VarChar).Value = cliente.Email;
                cmd.Parameters.Add("@id_cliente", MySqlDbType.Int32).Value = cliente.Codigo;

                cmd.ExecuteNonQuery(); // Executa o comando para atualizar 

                // Atualizando o telefone do cliente na tabela Telefone_tbl
                MySqlCommand cmdTelefone = new MySqlCommand(
                    // Atualiza o telefone usando o id_telefone associado ao id_cliente
                    // Telefone é uma tabela separada 
                    "UPDATE Telefone_tbl SET telefone_cliente = @telefone_cliente WHERE id_telefone = (SELECT id_telefone FROM Cliente_tbl WHERE id_cliente = @id_cliente)", conexao);

            }
        }
    }
}