using Microsoft.AspNetCore.Mvc;
using SegundaParcticaSSG.Models;
using SegundaParcticaSSG.Repositories;

namespace SegundaParcticaSSG.Controllers
{
    public class ComicsController : Controller
    {
        private IRepository repo;

        public ComicsController(IRepository repo)
        {
            this.repo = repo;
        }

        public IActionResult Index()
        {
            List<Comic> comics = this.repo.GetComics();
            return View(comics);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Comic comic)
        {
            this.repo.CreateComic(comic.Nombre, comic.Imagen, comic.Descripcion);
            return RedirectToAction("Index");
        }
    }
}
