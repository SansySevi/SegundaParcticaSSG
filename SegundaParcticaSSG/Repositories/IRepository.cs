using SegundaParcticaSSG.Models;

namespace SegundaParcticaSSG.Repositories
{
    public interface IRepository
    {
        List<Comic> GetComics();
        void CreateComic(string nombre, string imagen, string descripcion);
    }
}
