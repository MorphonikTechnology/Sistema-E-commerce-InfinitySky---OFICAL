using infinitysky.Models;
using System.Collections.Generic;

namespace infinitysky.Repository
{
    public interface IPagamentoRepositorio
    {
        // Adiciona um novo pagamento
        void Adicionar(Pagamento novoPagamento);

        // Atualiza um pagamento existente
        void Atualizar(Pagamento pagamento);

        // Obtém um pagamento específico pelo ID
        Pagamento ObterPorId(int idPagamento);

        // Obtém todos os pagamentos de um cliente, com base no ID do carrinho relacionado ao cliente
        IEnumerable<Pagamento> ObterPorCliente(int idCliente);
        Pagamento ObterPagamentoPorCarrinhoId(int carrinhoId);
        IEnumerable<Pagamento> ObterPagamentosPorClienteId(int clienteId);
    }
}
