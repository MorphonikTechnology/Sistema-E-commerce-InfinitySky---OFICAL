using System.Numerics;

namespace infinitysky.Models
{
    public class CompraViewModel
    {
        public string NomePlano { get; set; }
        public decimal ValorPlano { get; set; }
        public string StatusPagamento { get; set; }
        public DateTime DataPagamento { get; set; }
        public string FormaPagamento { get; set; }
        public string image_plano { get; set; }
        public string DescricaoPlano { get; set; }
        public List<CarrinhoViewModel> ItensCarrinho { get; set; }

    }


}
