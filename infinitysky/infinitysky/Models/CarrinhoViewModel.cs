namespace infinitysky.Models
{
    public class CarrinhoViewModel
    {
        public int IdCarrinho { get; set; }
        public int ItensCarrinho { get; set; }
        public decimal ValorTotalCarrinho { get; set; }
        public int IdPlano { get; set; }
        public string Nome { get; set; }
        public string HospedagemPlano { get; set; }
        public string CursoPlano { get; set; }
        public string InstituicaoPlano { get; set; }
        public string PeriodoPlano { get; set; }
        public string ParcelaPlano { get; set; }
        public decimal Valor { get; set; }
        public string image_plano { get; set; }
        public int IdCliente { get; set; }
        public int IdPagamento { get; internal set; }
    }
}

