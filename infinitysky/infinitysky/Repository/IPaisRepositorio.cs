using infinitysky.Models;

namespace infinitysky.Repository
{
    public interface IPaisRepositorio
    {
        Pais ObterPais(int Id);
        IEnumerable<Pais> ObterTodosPaises();
        //Pais Find(int Id);
    }
}
