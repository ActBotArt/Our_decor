using System;
using System.Collections.Generic;
public class MaterialType
{
    public int Id { get; set; }
    public string TypeName { get; set; }
    public decimal WastePercentage { get; set; }
    public decimal Coefficient { get; set; }

    // Навигационное свойство
    public List<Material> Materials { get; set; }
}