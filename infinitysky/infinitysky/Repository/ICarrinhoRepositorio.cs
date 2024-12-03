using infinitysky.Models;


namespace infinitysky.Repository
{
    public interface ICarrinhoRepositorio
    {
        // void AdicionarItem(Carrinho carrinho);
        void RemoverItem(int idCarrinho);

        void AdicionarItem(CarrinhoViewModel carrinho);

        void DeletarCarrinhoPorId(int idCarrinho);
        IEnumerable<CarrinhoViewModel> ObterItensCarrinho(int idCliente);
        void AtualizarItem(CarrinhoViewModel carrinho);
        CarrinhoViewModel ObterCarrinhoPorId(int idCarrinho);
    }
}
