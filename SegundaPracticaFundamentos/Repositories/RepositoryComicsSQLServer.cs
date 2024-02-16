using SegundaPracticaFundamentos.Models;
using System.Data;
using System.Data.SqlClient;
using static System.Runtime.InteropServices.JavaScript.JSType;


#region PROCEDURES
/*
	 create alter procedure SP_INSERTAR_COMIC( 
	@nombre NVARCHAR(50),
	@imagen NVARCHAR(500),
	@descripcion NVARCHAR(100))
	as
		declare @maxId INT;
		select @maxId = MAX(IDCOMIC) from COMICS
		insert into COMICS (IDCOMIC, NOMBRE, IMAGEN, DESCRIPCION) values (@maxId + 1, @nombre, @imagen, @descripcion)
	GO
 */
#endregion

namespace SegundaPracticaFundamentos.Repositories
{
	public class RepositoryComicsSQLServer: IRepositoryComics
	{
		private DataTable tablaComics;
		private SqlConnection cn;
		private SqlCommand com;

		public RepositoryComicsSQLServer() 
		{
			string connectionString = @"Data Source=LOCALHOST\SQLEXPRESS;Initial Catalog=PracticaAdoNET;Persist Security Info=True;User ID=SA";
			this.cn = new SqlConnection(connectionString);
			this.com = new SqlCommand();
			this.com.Connection = this.cn;
			string sql = "select* from COMICS";
			SqlDataAdapter adCom = new SqlDataAdapter(sql, connectionString);
			this.tablaComics = new DataTable();
			adCom.Fill(tablaComics);
		}

		public void DeleteComic(int idComic)
		{
            string sql = "delete from COMICS where idComic=@idComic";
            this.com.Parameters.AddWithValue("@idComic", idComic);
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
			int maxId = consulta.Max(x => x.Field<int>("IDCOMIC")) +1;

            string sql = "insert into COMICS values (@idComic, @nombre, @imagen, @descripcion)";
			this.com.Parameters.AddWithValue("@idComic", maxId);
			this.com.Parameters.AddWithValue("@nombre", nombre);
			this.com.Parameters.AddWithValue("@imagen", imagen);
			this.com.Parameters.AddWithValue("@descripcion", descripcion);

			this.com.CommandType = CommandType.Text;
			this.com.CommandText = sql;
			this.cn.Open();

			int af = this.com.ExecuteNonQuery();
			this.cn.Close();
			this.com.Parameters.Clear();
        }

        public void InsertComicProcedure(string nombre, string imagen, string descripcion)
        {
            this.com.Parameters.AddWithValue("@nombre", nombre);
            this.com.Parameters.AddWithValue("@imagen", imagen);
            this.com.Parameters.AddWithValue("@descripcion", descripcion);

			this.com.CommandType = CommandType.StoredProcedure;
			this.com.CommandText = "SP_INSERTAR_COMIC";
			this.cn.Open();

            int af = this.com.ExecuteNonQuery();
            this.cn.Close();
            this.com.Parameters.Clear();
        }
    }
}
