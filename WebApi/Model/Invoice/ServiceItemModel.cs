namespace WebApi.Model.Invoice
{
	public class ServiceItemModel
	{
		public int Id { get; set; }
		public string ServiceName { get; set; }
		public decimal Price { get; set; }
		public string? Description { get; set; }
	}
}
