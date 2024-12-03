namespace infinitysky.Models
{
    public class NotaFiscal
    {
        public int IdNF { get; set; } // id_nf
        public decimal ValorNF { get; set; } // valor_nf
        public DateTime DataEmissao { get; set; } // data_emissao
        public TimeSpan HoraEmissao { get; set; } // hora_emissao
        public int IdPagamento { get; set; } // id_pagamento
    }

}
