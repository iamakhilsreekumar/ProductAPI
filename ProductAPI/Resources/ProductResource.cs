namespace ProductAPI.Resources
{
    public class ProductResource
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int StockAvailable { get; set; }
        public string ProductId { get; set; } // Unique product number
    }
}
