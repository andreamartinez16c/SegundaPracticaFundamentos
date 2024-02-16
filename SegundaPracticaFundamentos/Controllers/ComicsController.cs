using Microsoft.AspNetCore.Mvc;
using SegundaPracticaFundamentos.Models;
using SegundaPracticaFundamentos.Repositories;

namespace SegundaPracticaFundamentos.Controllers
{
	public class ComicsController : Controller
	{
		private IRepositoryComics repo;

		public ComicsController(IRepositoryComics repo)
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
			this.repo.InsertComic(comic.Nombre, comic.Imagen, comic.Descripcion);
            return RedirectToAction("Index");
        }

        public IActionResult CreateProcedure()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateProcedure(Comic comic)
        {
            this.repo.InsertComicProcedure(comic.Nombre, comic.Imagen, comic.Descripcion);
            return RedirectToAction("Index");
        }

		public IActionResult BuscadorDetalles()
		{
			List<Comic> comics = this.repo.GetComics();
			BucadorDetalles busc = new BucadorDetalles();
			busc.comics = comics;
			return View(busc);
		}

		[HttpPost]
		public IActionResult BuscadorDetalles(int idComic)
		{
			Comic com = this.repo.FindComic(idComic);
			List<Comic> comics = this.repo.GetComics();
			BucadorDetalles busc = new BucadorDetalles();
			busc.comics = comics;
			busc.comic = com;
			return View(busc);

		}

		[Route("deleteRoute")]
		public IActionResult Delete(int idComic)
		{
			Comic comic = this.repo.FindComic(idComic);
			return View(comic);
		}

		[Route("deleteRoute")]
		[HttpPost]
		public IActionResult DeletePost(int idComic)
		{
			this.repo.DeleteComic(idComic);
			return RedirectToAction("Index");
		}
	}
}
