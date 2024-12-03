using System.Numerics;

namespace infinitysky.Models
{
    public class PaisesDetalhadosViewModel //CLASSE VIEWMODEL -> SERVE PARA "JUNTAR 2 CLASSES EM UMA SÓ", ESSENCIAL NAS CHAMADAS NO LAYOUT
    {
        //Neste caso, esta viewModel foi utilizada para chamar tanto os planos de cada país, quanto o país em si mesmo.
        public Pais Pais { get; set; }
        public List<Planos> Planos { get; set; }


    }
}
