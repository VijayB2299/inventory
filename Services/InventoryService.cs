public class InventoryService : IInventoryService
{
  private readonly List<Product> _products = new();
  public Task AddProduct(Product product)
  {
    var productWithSameName = _products.Find(p => p.Name.Equals(product.Name, StringComparison.OrdinalIgnoreCase));
    if (productWithSameName != null)
    {
      throw new DuplicateProductNameException();
    }

    _products.Add(product);
    return Task.CompletedTask;
  }

  public Task<IReadOnlyList<Product>> ListProducts()
  {
    return Task.FromResult<IReadOnlyList<Product>>(_products);
  }

  public Task<IReadOnlyList<Product>> SearchProductsByName(string name)
  {
    var results = _products.Where(p => p.Name.Contains(name, StringComparison.OrdinalIgnoreCase)).ToList();
    return Task.FromResult<IReadOnlyList<Product>>(results);
  }

  public Task<decimal> GetTotalInventoryValue()
  {
    decimal totalValue = _products.Sum(p => p.Price * p.Quantity);
    return Task.FromResult(totalValue);
  }

  public Task<Product?> FindHighPricedProduct()
  {
    var highPricedProduct = _products.MaxBy(p => p.Price);
    if (highPricedProduct != null)
    {
      return Task.FromResult<Product?>(highPricedProduct);
    } else
    {
      throw new ProductNotFoundException();
    }
  }

  public Task<Product> UpdateProductQuantity(int productId, int newQuantity)
  {
    var existingProductIndex = _products.FindIndex(p => p.Id == productId);
    var product = _products[existingProductIndex];
    var updatedProduct = new Product(id: product.Id, name: product.Name, price: product.Price, quantity: newQuantity);
    _products[existingProductIndex] = updatedProduct;
    return Task.FromResult(updatedProduct);
  }

  public Task<Product> UpdateProductPrice(int productId, decimal newPrice)
  {
    var existingProductIndex = _products.FindIndex(p => p.Id == productId);
    var product = _products[existingProductIndex];
    var updatedProduct = new Product(id: product.Id, name: product.Name, price: newPrice, quantity: product.Quantity);
    _products[existingProductIndex] = updatedProduct;
    return Task.FromResult(updatedProduct);
  }

  public Task DeleteProduct(int productId)
  {
    var productToDelete = _products.Find(p => p.Id == productId);
    if (productToDelete != null)
    {
      _products.Remove(productToDelete);
      return Task.CompletedTask;
    }
    else
    {
      throw new ProductNotFoundException();
    }
  }
}