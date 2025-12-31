public interface IInventoryService
{
  Task AddProduct(Product product);
  Task<IReadOnlyList<Product>> ListProducts();
  Task<IReadOnlyList<Product>> SearchProductsByName(string name);
  Task<decimal> GetTotalInventoryValue();
  Task<Product?> FindHighPricedProduct();
  Task<Product> UpdateProductQuantity(int productId, int newQuantity);
  Task<Product> UpdateProductPrice(int productId, decimal newPrice);
  Task DeleteProduct(int productId);
}