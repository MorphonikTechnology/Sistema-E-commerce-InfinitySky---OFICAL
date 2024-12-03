namespace infinitysky.Models
{
    public class Pagamento
    {
        public int IdPagamento { get; set; }
        public string FormaPagamento { get; set; }
        public string StatusPagamentos { get; set; }
        public TimeSpan HoraPagamento { get; set; }
        public decimal ValorPagamento { get; set; }
        public int IdCarrinho { get; set; }
        public int IdPlano { get; set; }

    
        public Carrinho Carrinho { get; set; }


    
        public string Nome { get; set; }
        public string HospedagemPlano { get; set; }
        public string CursoPlano { get; set; }
        public string InstituicaoPlano { get; set; }
        public string PeriodoPlano { get; set; }
        public string ParcelaPlano { get; set; }
        public decimal Valor { get; set; }
        public string image_plano { get; set; }
    }
}
