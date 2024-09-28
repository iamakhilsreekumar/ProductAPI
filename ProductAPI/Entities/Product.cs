namespace ProductAPI.Entities
{
    public record Product : IEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int StockAvailable { get; set; }
        public int ProductId { get; set; } // Unique product number
    }
}
