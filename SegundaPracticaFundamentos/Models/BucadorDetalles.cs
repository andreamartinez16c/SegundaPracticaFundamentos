namespace SegundaPracticaFundamentos.Models
{
	public class BucadorDetalles
	{
		public BucadorDetalles() 
		{
			this.comics = new List<Comic>();
		}
		public List<Comic> comics {  get; set; }
		public Comic comic { get; set; }
	}
}
