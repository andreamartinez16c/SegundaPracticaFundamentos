using Oracle.ManagedDataAccess.Client;
using SegundaPracticaFundamentos.Models;
using System.Data;

#region PROCEDURES
/*
	 create or replace procedure sp_insertar_comic
	(p_NOMBRE COMICS.NOMBRE%TYPE,
	p_IMAGEN COMICS.IMAGEN%TYPE,
	p_DESCRIPCION COMICS.DESCRIPCION%TYPE)
	AS
	begin
		 insert into comics values((SELECT MAX(IDCOMIC) FROM COMICS) + 1 ,p_nombre,p_imagen,p_descripcion);
	commit;
	end;
 */
#endregion

namespace SegundaPracticaFundamentos.Repositories
{
	public class RepositoryComicsOracle : IRepositoryComics
	{
		private DataTable tablaComics;
		private OracleConnection cn;
		private OracleCommand com;

		public RepositoryComicsOracle()
		{
			string connectionString = @"Data Source=LOCALHOST:1521/XE; Persist Security Info=True;User Id=SYSTEM; Password=oracle";
			this.cn = new OracleConnection(connectionString);
			this.com = new OracleCommand();
			this.com.Connection = this.cn;
			string sql = "select* from COMICS";
			OracleDataAdapter ad = new OracleDataAdapter(sql, this.cn);
			this.tablaComics = new DataTable();
			ad.Fill(this.tablaComics);
		}
		public void DeleteComic(int idComic)
		{
			string sql = "delete from COMICS where idComic=:idComic";
			OracleParameter pamId = new OracleParameter(":idComic", idComic);
			this.com.Parameters.Add(pamId);

			this.com.CommandType = CommandType.Text;
			this.com.CommandText = sql;
			this.cn.Open();

			int af = this.com.ExecuteNonQuery();
			this.cn.Close();
			this.com.Parameters.Clear();
		}

		public Comic FindComic(int idComic)
		{
			var consulta = from datos in this.tablaComics.AsEnumerable()
						   where datos.Field<int>("IDCOMIC") == idComic
						   select datos;
			var row = consulta.First();
			Comic com = new Comic()
			{

				IdComic = row.Field<int>("IDCOMIC"),
				Nombre = row.Field<string>("NOMBRE"),
				Imagen = row.Field<string>("IMAGEN"),
				Descripcion = row.Field<string>("DESCRIPCION")
			};
			return com;
		}

		public List<string> GetAllNombresComics()
		{
			var consulta = from datos in this.tablaComics.AsEnumerable()
						   select datos;
			List<string> nombre = new List<string>();
			foreach (var row in consulta)
			{
				nombre.Add(row.Field<string>("NOMBRE"));
			}
			return nombre;
		}

		public List<Comic> GetComics()
		{
			var consulta = from datos in this.tablaComics.AsEnumerable()
						   select datos;
			List<Comic> comics = new List<Comic>();
			foreach (var row in consulta)
			{
				Comic com = new Comic()
				{
					IdComic = row.Field<int>("IDCOMIC"),
					Nombre = row.Field<string>("NOMBRE"),
					Imagen = row.Field<string>("IMAGEN"),
					Descripcion = row.Field<string>("DESCRIPCION")
				};
				comics.Add(com);
			}
			return comics;
		}

		public void InsertComic(string nombre, string imagen, string descripcion)
		{
			var consulta = from datos in this.tablaComics.AsEnumerable()
						   select datos;
			int maxId = consulta.Max(x => x.Field<int>("IDCOMIC")) + 1;

			string sql = "insert into COMICS values (:idComic, :nombre, :imagen, :descripcion)";
			OracleParameter pamId = new OracleParameter(":idComic", maxId);
			this.com.Parameters.Add(pamId);
			OracleParameter pamNombre = new OracleParameter(":nombre", nombre);
			this.com.Parameters.Add(pamNombre);
			OracleParameter pamImagen = new OracleParameter(":imagen", imagen);
			this.com.Parameters.Add(pamImagen);
			OracleParameter pamDescripcion = new OracleParameter(":descripcion", descripcion);
			this.com.Parameters.Add(pamDescripcion);

			this.com.CommandType = CommandType.Text;
			this.com.CommandText = sql;
			this.cn.Open();

			int af = this.com.ExecuteNonQuery();
			this.cn.Close();
			this.com.Parameters.Clear();
		}

		public void InsertComicProcedure(string nombre, string imagen, string descripcion)
		{
			OracleParameter pamNombre = new OracleParameter(":nombre", nombre);
			this.com.Parameters.Add(pamNombre);
			OracleParameter pamImagen = new OracleParameter(":imagen", imagen);
			this.com.Parameters.Add(pamImagen);
			OracleParameter pamDescripcion = new OracleParameter(":descripcion", descripcion);
			this.com.Parameters.Add(pamDescripcion);

			this.com.CommandType= CommandType.StoredProcedure;
			this.com.CommandText = "sp_insertar_comic";
			this.cn.Open();
			int af = this.com.ExecuteNonQuery();
			this.cn.Close();
			this.com.Parameters.Clear();
		}
	}
}
