using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Our_decor.Services;

namespace Our_decor.Models
{
    public class ProductRepository
    {
        private readonly DatabaseService _db;

        public ProductRepository()
        {
            _db = DatabaseService.Instance;
        }

        public async Task<List<Product>> GetAllProductsAsync()
        {
            var result = new List<Product>();
            var query = @"SELECT p.*, pt.TypeName, pt.Coefficient 
                         FROM Products p 
                         INNER JOIN ProductTypes pt ON p.ProductTypeId = pt.Id";

            var dt = await _db.ExecuteQueryAsync(query);

            foreach (DataRow row in dt.Rows)
            {
                result.Add(new Product
                {
                    Id = Convert.ToInt32(row["Id"]),
                    Article = row["Article"].ToString(),
                    ProductTypeId = Convert.ToInt32(row["ProductTypeId"]),
                    Name = row["Name"].ToString(),
                    Description = row["Description"].ToString(),
                    MinPartnerCost = Convert.ToDecimal(row["MinPartnerCost"]),
                    RollWidth = Convert.ToDecimal(row["RollWidth"]),
                    ProductionTime = Convert.ToInt32(row["ProductionTime"]),
                    WorkshopNumber = Convert.ToInt32(row["WorkshopNumber"]),
                    WorkersCount = Convert.ToInt32(row["WorkersCount"]),
                    ProductType = new ProductType
                    {
                        Id = Convert.ToInt32(row["ProductTypeId"]),
                        TypeName = row["TypeName"].ToString(),
                        Coefficient = Convert.ToDecimal(row["Coefficient"])
                    }
                });
            }

            return result;
        }

        public async Task<List<ProductType>> GetProductTypesAsync()
        {
            var result = new List<ProductType>();
            var query = "SELECT * FROM ProductTypes";

            var dt = await _db.ExecuteQueryAsync(query);

            foreach (DataRow row in dt.Rows)
            {
                result.Add(new ProductType
                {
                    Id = Convert.ToInt32(row["Id"]),
                    TypeName = row["TypeName"].ToString(),
                    Coefficient = Convert.ToDecimal(row["Coefficient"])
                });
            }

            return result;
        }

        public async Task SaveAsync(Product product)
        {
            if (product.Id == 0)
            {
                var query = @"INSERT INTO Products (Article, ProductTypeId, Name, Description, 
                            MinPartnerCost, RollWidth, ProductionTime, WorkshopNumber, WorkersCount)
                            VALUES (@Article, @ProductTypeId, @Name, @Description, 
                            @MinPartnerCost, @RollWidth, @ProductionTime, @WorkshopNumber, @WorkersCount);
                            SELECT SCOPE_IDENTITY();";

                var parameters = new[]
                {
                    new SqlParameter("@Article", product.Article),
                    new SqlParameter("@ProductTypeId", product.ProductTypeId),
                    new SqlParameter("@Name", product.Name),
                    new SqlParameter("@Description", (object)product.Description ?? DBNull.Value),
                    new SqlParameter("@MinPartnerCost", product.MinPartnerCost),
                    new SqlParameter("@RollWidth", product.RollWidth),
                    new SqlParameter("@ProductionTime", product.ProductionTime),
                    new SqlParameter("@WorkshopNumber", product.WorkshopNumber),
                    new SqlParameter("@WorkersCount", product.WorkersCount)
                };

                var result = await _db.ExecuteScalarAsync(query, parameters);
                product.Id = Convert.ToInt32(result);
            }
            else
            {
                var query = @"UPDATE Products SET 
                            ProductTypeId = @ProductTypeId,
                            Name = @Name,
                            Description = @Description,
                            MinPartnerCost = @MinPartnerCost,
                            RollWidth = @RollWidth,
                            ProductionTime = @ProductionTime,
                            WorkshopNumber = @WorkshopNumber,
                            WorkersCount = @WorkersCount
                            WHERE Id = @Id";

                var parameters = new[]
                {
                    new SqlParameter("@Id", product.Id),
                    new SqlParameter("@ProductTypeId", product.ProductTypeId),
                    new SqlParameter("@Name", product.Name),
                    new SqlParameter("@Description", (object)product.Description ?? DBNull.Value),
                    new SqlParameter("@MinPartnerCost", product.MinPartnerCost),
                    new SqlParameter("@RollWidth", product.RollWidth),
                    new SqlParameter("@ProductionTime", product.ProductionTime),
                    new SqlParameter("@WorkshopNumber", product.WorkshopNumber),
                    new SqlParameter("@WorkersCount", product.WorkersCount)
                };

                await _db.ExecuteNonQueryAsync(query, parameters);
            }
        }

        public async Task DeleteAsync(int id)
        {
            var query = "DELETE FROM Products WHERE Id = @Id";
            var parameter = new SqlParameter("@Id", id);
            await _db.ExecuteNonQueryAsync(query, parameter);
        }
    }
}