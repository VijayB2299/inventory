using Inventory.Domain;

namespace Inventory.Services;

public interface IInventoryService
{
  Task<Product> AddProduct(int id, string name, decimal price, int quantity);
  Task<IReadOnlyList<Product>> ListProducts();

  Task<Product> GetProduct(int id);

  Task<IReadOnlyList<Product>> SearchProductsByName(string name);
  Task<decimal> GetTotalInventoryValue();
  Task<Product?> FindHighPricedProduct();
  Task<Product> UpdateProductQuantity(int productId, int newQuantity);
  Task<Product> UpdateProductPrice(int productId, decimal newPrice);
  Task DeleteProduct(int productId);
}