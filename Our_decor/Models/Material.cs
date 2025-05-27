namespace Our_decor.Models
{
    public class Material
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Cost { get; set; }
        public string Unit { get; set; }
        public decimal StockQuantity { get; set; }
        public decimal MinQuantity { get; set; }
        public int MaterialTypeId { get; set; }
        public MaterialType MaterialType { get; set; }
    }
}