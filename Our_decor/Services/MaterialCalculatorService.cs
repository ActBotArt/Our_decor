using System;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Our_decor.Models;

namespace Our_decor.Services
{
    public class MaterialCalculatorService
    {
        private readonly DatabaseService _db;

        public MaterialCalculatorService()
        {
            _db = DatabaseService.Instance;
        }

        // Исправляем возвращаемый тип на Task<decimal>
        public async Task<decimal> CalculateProductCost(int productId)
        {
            try
            {
                var query = @"
                    SELECT ISNULL(SUM(m.Cost * pm.Quantity), 0) as TotalCost
                    FROM ProductMaterials pm
                    JOIN Materials m ON pm.MaterialId = m.Id
                    WHERE pm.ProductId = @ProductId";

                var parameter = new SqlParameter("@ProductId", productId);
                var result = await _db.ExecuteScalarAsync(query, parameter);

                return result != null ? Convert.ToDecimal(result) : 0;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        // Исправляем возвращаемый тип на Task<int>
        public async Task<int> CalculateRequiredMaterial(
            int productTypeId,
            int materialTypeId,
            int productQuantity,
            decimal param1,
            decimal param2,
            decimal stockQuantity)
        {
            try
            {
                // Проверка входных параметров
                if (productQuantity <= 0 || param1 <= 0 || param2 <= 0 || stockQuantity < 0)
                    return -1;

                // Получаем коэффициент типа продукции
                var productTypeQuery = "SELECT Coefficient FROM ProductTypes WHERE Id = @Id";
                var productTypeParam = new SqlParameter("@Id", productTypeId);
                var productTypeResult = await _db.ExecuteScalarAsync(productTypeQuery, productTypeParam);

                if (productTypeResult == null)
                    return -1;

                decimal productTypeCoef = Convert.ToDecimal(productTypeResult);

                // Получаем процент брака материала
                var materialTypeQuery = "SELECT WastePercentage FROM MaterialTypes WHERE Id = @Id";
                var materialTypeParam = new SqlParameter("@Id", materialTypeId);
                var materialTypeResult = await _db.ExecuteScalarAsync(materialTypeQuery, materialTypeParam);

                if (materialTypeResult == null)
                    return -1;

                decimal wastePercentage = Convert.ToDecimal(materialTypeResult);

                // Расчет необходимого количества материала
                decimal baseAmount = param1 * param2 * productTypeCoef * productQuantity;
                decimal withWaste = baseAmount * (1 + wastePercentage / 100);
                decimal required = Math.Max(0, withWaste - stockQuantity);

                return (int)Math.Ceiling(required);
            }
            catch (Exception)
            {
                return -1;
            }
        }

        public async Task UpdateProductCost(Product product)
        {
            if (product == null) return;

            decimal calculatedCost = await CalculateProductCost(product.Id);

            var query = @"
        UPDATE Products 
        SET CalculatedCost = @CalculatedCost
        WHERE Id = @Id";

            var parameters = new[]
            {
        new SqlParameter("@CalculatedCost", calculatedCost),
        new SqlParameter("@Id", product.Id)
    };

            await _db.ExecuteNonQueryAsync(query, parameters);
            product.CalculatedCost = calculatedCost; // Исправлено с Product.CalculatedCost на product.CalculatedCost
        }
    }
}