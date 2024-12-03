namespace infinitysky.Models
{
    public class PlanosPrimeiraParte 
    {
        //ViewModel utilizada para chamar os 3 primeiros planos e o resto na homepage
        public IEnumerable<Planos> TresPrimeirosPlanos { get; set; }

        public IEnumerable<Planos> RestantePlanos { get; set; }
    }
}
