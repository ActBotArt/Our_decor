using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Our_decor.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Article { get; set; }
        public ProductType ProductType { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal MinPartnerCost { get; set; }
        public decimal RollWidth { get; set; }
        public decimal MaterialCost { get; set; }
        public int ProductionTime { get; set; }
        public int WorkshopNumber { get; set; }
        public int WorkersCount { get; set; }

        public async Task SaveAsync()
        {
            using (var connection = new SqlConnection(DatabaseService.ConnectionString))
            {
                await connection.OpenAsync();
                using (var command = connection.CreateCommand())
                {
                    if (Id == 0)
                    {
                        command.CommandText = @"
                            INSERT INTO Products (
                                Article, ProductTypeId, Name, Description, 
                                MinPartnerCost, RollWidth, ProductionTime, 
                                WorkshopNumber, WorkersCount
                            ) VALUES (
                                @Article, 
                                @ProductTypeId,
                                @Name, @Description, @MinPartnerCost, @RollWidth,
                                @ProductionTime, @WorkshopNumber, @WorkersCount
                            );
                            SELECT SCOPE_IDENTITY();";
                    }
                    else
                    {
                        command.CommandText = @"
                            UPDATE Products 
                            SET ProductTypeId = @ProductTypeId,
                                Name = @Name,
                                Description = @Description,
                                MinPartnerCost = @MinPartnerCost,
                                RollWidth = @RollWidth,
                                ProductionTime = @ProductionTime,
                                WorkshopNumber = @WorkshopNumber,
                                WorkersCount = @WorkersCount
                            WHERE Id = @Id";
                    }

                    command.Parameters.AddWithValue("@Id", Id);
                    command.Parameters.AddWithValue("@Article", Article);
                    command.Parameters.AddWithValue("@ProductTypeId", ProductType.Id);
                    command.Parameters.AddWithValue("@Name", Name);
                    command.Parameters.AddWithValue("@Description", (object)Description ?? DBNull.Value);
                    command.Parameters.AddWithValue("@MinPartnerCost", MinPartnerCost);
                    command.Parameters.AddWithValue("@RollWidth", RollWidth);
                    command.Parameters.AddWithValue("@ProductionTime", ProductionTime);
                    command.Parameters.AddWithValue("@WorkshopNumber", WorkshopNumber);
                    command.Parameters.AddWithValue("@WorkersCount", WorkersCount);

                    if (Id == 0)
                    {
                        var result = await command.ExecuteScalarAsync();
                        Id = Convert.ToInt32(result);
                    }
                    else
                    {
                        await command.ExecuteNonQueryAsync();
                    }
                }
            }
        }

        public static async Task<List<Product>> GetAllProductsAsync()
        {
            var products = new List<Product>();

            using (var connection = new SqlConnection(DatabaseService.ConnectionString))
            {
                await connection.OpenAsync();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = @"
                        SELECT p.*, pt.Id as TypeId, pt.TypeName, pt.Coefficient
                        FROM Products p
                        INNER JOIN ProductTypes pt ON p.ProductTypeId = pt.Id";

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            products.Add(new Product
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("Id")),
                                Article = reader.GetString(reader.GetOrdinal("Article")),
                                ProductType = new ProductType
                                {
                                    Id = reader.GetInt32(reader.GetOrdinal("TypeId")),
                                    TypeName = reader.GetString(reader.GetOrdinal("TypeName")),
                                    Coefficient = reader.GetDecimal(reader.GetOrdinal("Coefficient"))
                                },
                                Name = reader.GetString(reader.GetOrdinal("Name")),
                                Description = reader.IsDBNull(reader.GetOrdinal("Description"))
                                    ? null
                                    : reader.GetString(reader.GetOrdinal("Description")),
                                MinPartnerCost = reader.GetDecimal(reader.GetOrdinal("MinPartnerCost")),
                                RollWidth = reader.GetDecimal(reader.GetOrdinal("RollWidth")),
                                ProductionTime = reader.GetInt32(reader.GetOrdinal("ProductionTime")),
                                WorkshopNumber = reader.GetInt32(reader.GetOrdinal("WorkshopNumber")),
                                WorkersCount = reader.GetInt32(reader.GetOrdinal("WorkersCount"))
                            });
                        }
                    }
                }
            }
            return products;
        }

        public static async Task DeleteAsync(int id)
        {
            using (var connection = new SqlConnection(DatabaseService.ConnectionString))
            {
                await connection.OpenAsync();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "DELETE FROM Products WHERE Id = @Id";
                    command.Parameters.AddWithValue("@Id", id);
                    await command.ExecuteNonQueryAsync();
                }
            }
        }

        public static async Task<List<ProductType>> GetProductTypesAsync()
        {
            var types = new List<ProductType>();

            using (var connection = new SqlConnection(DatabaseService.ConnectionString))
            {
                await connection.OpenAsync();
                using (var command = connection.CreateCommand())
                {
                    command.CommandText = "SELECT Id, TypeName, Coefficient FROM ProductTypes ORDER BY TypeName";

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            types.Add(new ProductType
                            {
                                Id = reader.GetInt32(0),
                                TypeName = reader.GetString(1),
                                Coefficient = reader.GetDecimal(2)
                            });
                        }
                    }
                }
            }
            return types;
        }
    }
}