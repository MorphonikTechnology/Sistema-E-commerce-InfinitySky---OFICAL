using infinitysky.Models;

namespace infinitysky.Repository
{
    public interface IPlanosRepositorio
    {
        List<Planos> ObterPlanosAleatorios(int IdPlano);

        List<Planos> ObterRestante(int IdPlano);


        IEnumerable<Planos> PesquisaPlanos(string Nome);

        public IEnumerable<Planos> ObterPlanosPorPaisId(int paisId);

        void Adicionar(Planos plano);

        void Atualizar(Planos plano, string image_plano_atual); // Se necessário
        Planos ObterPlano(long Id);

        void Apagar(long Id);
        IEnumerable<Planos> ObterTodosPlanos();
        IEnumerable<Planos> PesquisaPlanosPorNome(string searchTerm);
        Planos ObterPlanoPorIdCarrinho(int carrinhoId);
        Planos ObterPlanoPorPagamento(int pagamentoId);
    }
}
