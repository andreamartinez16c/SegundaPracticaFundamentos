using SegundaPracticaFundamentos.Models;

namespace SegundaPracticaFundamentos.Repositories
{
	public interface IRepositoryComics
	{
		List<Comic> GetComics();
		void InsertComic(string nombre, string imagen, string descripcion);
		void InsertComicProcedure(string nombre, string imagen, string descripcion);
        Comic FindComic(int idComic);
		void DeleteComic(int idComic);
		List<string> GetAllNombresComics();
	}
}
