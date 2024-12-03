using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Numerics;

namespace infinitysky.Models
{
    public class Carrinho
    {
        public int IdCarrinho { get; set; }

        [Required]
        public int ItensCarrinho { get; set; }

        [Required]
        [Column(TypeName = "decimal(8,2)")]
        public decimal ValorTotalCarrinho { get; set; }

        [Required]
        public int IdPlano { get; set; }

        [Required]
        public int IdCliente { get; set; }

        [ForeignKey(nameof(IdCliente))]
        public Cliente Cliente { get; set; }
        public List<Planos> Planos { get; set; } = new List<Planos>();
    }
}
