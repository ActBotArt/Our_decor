using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Threading.Tasks;
using Our_decor.Services;

namespace Our_decor.Models
{
    public class ProductMaterial
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int MaterialId { get; set; }
        public decimal Quantity { get; set; }
        public decimal Cost { get; set; }
        public string ProductArticle { get; set; }
        public string ProductName { get; set; }
        public string MaterialName { get; set; }
        public string MaterialUnit { get; set; }

        public static async Task<List<ProductMaterial>> GetByProductIdAsync(int productId)
        {
            var materials = new List<ProductMaterial>();
            var query = @"SELECT 
                         pm.Id,
                         pm.ProductId,
                         pm.MaterialId,
                         pm.Quantity,
                         p.Article as ProductArticle,
                         p.Name as ProductName,
                         m.Name as MaterialName,
                         m.Unit as MaterialUnit,
                         m.Cost * pm.Quantity as TotalCost
                         FROM ProductMaterials pm
                         INNER JOIN Products p ON pm.ProductId = p.Id
                         INNER JOIN Materials m ON pm.MaterialId = m.Id
                         WHERE pm.ProductId = @ProductId";

            try
            {
                var parameter = new SqlParameter("@ProductId", productId);
                var dataTable = await DatabaseService.Instance.ExecuteQueryAsync(query, parameter);

                foreach (DataRow row in dataTable.Rows)
                {
                    materials.Add(new ProductMaterial
                    {
                        Id = Convert.ToInt32(row["Id"]),
                        ProductId = Convert.ToInt32(row["ProductId"]),
                        MaterialId = Convert.ToInt32(row["MaterialId"]),
                        Quantity = Convert.ToDecimal(row["Quantity"]),
                        ProductArticle = row["ProductArticle"].ToString(),
                        ProductName = row["ProductName"].ToString(),
                        MaterialName = row["MaterialName"].ToString(),
                        MaterialUnit = row["MaterialUnit"].ToString(),
                        Cost = Convert.ToDecimal(row["TotalCost"])
                    });
                }
            }
            catch (Exception)
            {
                throw;
            }

            return materials;
        }

        public async Task SaveAsync()
        {
            try
            {
                if (Id == 0)
                {
                    var query = @"INSERT INTO ProductMaterials (ProductId, MaterialId, Quantity)
                                VALUES (@ProductId, @MaterialId, @Quantity);
                                SELECT SCOPE_IDENTITY();";

                    var parameters = new[]
                    {
                        new SqlParameter("@ProductId", ProductId),
                        new SqlParameter("@MaterialId", MaterialId),
                        new SqlParameter("@Quantity", Quantity)
                    };

                    var result = await DatabaseService.Instance.ExecuteScalarAsync(query, parameters);
                    Id = Convert.ToInt32(result);
                }
                else
                {
                    var query = @"UPDATE ProductMaterials 
                                SET ProductId = @ProductId,
                                    MaterialId = @MaterialId,
                                    Quantity = @Quantity
                                WHERE Id = @Id";

                    var parameters = new[]
                    {
                        new SqlParameter("@Id", Id),
                        new SqlParameter("@ProductId", ProductId),
                        new SqlParameter("@MaterialId", MaterialId),
                        new SqlParameter("@Quantity", Quantity)
                    };

                    await DatabaseService.Instance.ExecuteNonQueryAsync(query, parameters);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static async Task DeleteAsync(int id)
        {
            try
            {
                var query = "DELETE FROM ProductMaterials WHERE Id = @Id";
                var parameter = new SqlParameter("@Id", id);
                await DatabaseService.Instance.ExecuteNonQueryAsync(query, parameter);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static async Task<decimal> CalculateTotalCostAsync(int productId)
        {
            try
            {
                var query = @"SELECT SUM(m.Cost * pm.Quantity) as TotalCost
                             FROM ProductMaterials pm
                             INNER JOIN Materials m ON pm.MaterialId = m.Id
                             WHERE pm.ProductId = @ProductId";

                var parameter = new SqlParameter("@ProductId", productId);
                var result = await DatabaseService.Instance.ExecuteScalarAsync(query, parameter);

                return result == DBNull.Value ? 0 : Convert.ToDecimal(result);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}