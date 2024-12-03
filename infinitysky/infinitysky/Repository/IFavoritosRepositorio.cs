using infinitysky.Models;

namespace infinitysky.Repository
{
    public interface IFavoritosRepositorio
    {

        void AdicionarFavorito(FavoritosViewModel favorito);


        FavoritosViewModel ObterFavorito(int clienteId, int idPlano);
        List<FavoritosViewModel> ObterFavoritosPorCliente(int clienteId);

        void RemoverFavorito(int idFavorito);


        // IEnumerable<FavoritosViewModel> ObterFavoritos(int idCliente);
    }

}
