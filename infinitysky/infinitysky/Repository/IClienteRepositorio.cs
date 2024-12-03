using infinitysky.Models;

namespace infinitysky.Repository
{
    public interface IClienteRepositorio
    {
        // CRUD
        //login
        // Em verde model, amarelo = método (dentro dele  está as funçoes do sql(select, insert, etc))
        Cliente Login(string Email, string Senha);


        // listar 
        //excluir

        //cadastrar cliente 
        //void sem retorno 
        void Cadastrar(Cliente cliente);



        //Método da tela PainelCliente, vai aparecer só o nome 
        // Método da tela Dados, vai aparecer todos os Dados
        // Método da tela AtualizarDados, vai atualizar os dados 
        Cliente ObterClientePorId(int clienteId);

        Cliente DadosCliente(int clienteId);

        void AtualizarCliente(Cliente cliente);
    }
}
