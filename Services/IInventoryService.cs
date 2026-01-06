using Inventory.Models;
using Inventory.Common.Pagination;

namespace Inventory.Services;

public interface IInventoryService
{
  Task<Product> AddProduct(CreateProductDto product);
  Task<PaginatedList<Product>> ListProducts(PageParameters pageParameters, SortParameters sortParameters, FilterParameters filterParameters);
  Task<Product> GetProduct(Guid id);
  Task<IEnumerable<Product>> SearchProductsByName(string name);
  Task<decimal> GetTotalInventoryValue();
  Task<Product?> FindHighPricedProduct();
  Task<Product> UpdateProductQuantity(Guid productId, int newQuantity);
  Task<Product> UpdateProductPrice(Guid productId, decimal newPrice);
  Task DeleteProduct(Guid productId);
}