using System;

namespace Our_decor.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Article { get; set; }
        public int ProductTypeId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal MinPartnerCost { get; set; }
        public decimal RollWidth { get; set; }
        public int ProductionTime { get; set; }
        public int WorkshopNumber { get; set; }
        public int WorkersCount { get; set; }
        public decimal CalculatedCost { get; set; }
        public ProductType ProductType { get; set; }
        public string TypeName
        {
            get
            {
                return ProductType?.TypeName ?? "Не указан";
            }
        }
    }
}
