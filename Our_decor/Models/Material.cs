using System;
using System.Collections.Generic;
public class Material
{
    public int Id { get; set; }
    public int MaterialTypeId { get; set; }
    public string Name { get; set; }
    public string Unit { get; set; }
    public decimal Cost { get; set; }
    public decimal StockQuantity { get; set; }
    public decimal MinQuantity { get; set; }

    // Навигационное свойство
    public MaterialType MaterialType { get; set; }
}