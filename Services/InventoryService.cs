using Inventory.Exceptions;
using Inventory.Models;
using Inventory.Data;
using Microsoft.EntityFrameworkCore;
using Inventory.Common.Pagination;

namespace Inventory.Services;

public class InventoryService : IInventoryService
{
  private readonly InventoryDbContext _db;
  public InventoryService(InventoryDbContext db) => _db = db;
  public async Task<Product> AddProduct(CreateProductDto product)
  {
    if (await ProductExists(product.Name))
    {
      throw new DuplicateProductNameException();
    }

    var newProduct = new Product(
      Guid.NewGuid(),
      product.Name,
      product.Price,
      product.Quantity
    );

    _db.Products.Add(newProduct);
    await _db.SaveChangesAsync();
    return newProduct;
  }

  public async Task<PaginatedList<Product>> ListProducts(PageParameters pageParameters, SortParameters sortParameters, FilterParameters filterParameters)
  {
    var query = _db.Products.AsQueryable();

    if (sortParameters.SortBy == "price")
    {
      query = sortParameters.Descending ? query.OrderByDescending(p => p.Price) : query.OrderBy(p => p.Price);
    }
    else if (sortParameters.SortBy == "name")
    {
      query = sortParameters.Descending ? query.OrderByDescending(p => p.Name) : query.OrderBy(p => p.Name);
    }
    else
    {
      query = sortParameters.Descending ? query.OrderByDescending(p => p.Id) : query.OrderBy(p => p.Id);
    }

    if (filterParameters.MinPrice > 0)
    {
      query = query.Where(p => p.Price >= filterParameters.MinPrice);
    }
    
    if (filterParameters.MaxPrice < int.MaxValue)
    {
      query = query.Where(p => p.Price <= filterParameters.MaxPrice);
    }

    if (filterParameters.LowStock)
    {
      query = query.Where(p => p.Quantity <= 5);
    }

    var results = await PaginatedList<Product>.FetchAsync(query, pageParameters.Page, pageParameters.PageSize);
    return results;
  }

  public async Task<Product> GetProduct(Guid id)
  {
    var product = await _db.Products.Where(p => p.Id == id).FirstOrDefaultAsync();
    if (product != null)
    {
      return new Product(product.Id, product.Name, product.Price, product.Quantity);
    }
    else
    {
      throw new ProductNotFoundException();
    }
  }

  public async Task<IEnumerable<Product>> SearchProductsByName(string name)
  {
    var results = await _db.Products.Where(p => p.Name.Contains(name, StringComparison.OrdinalIgnoreCase)).ToListAsync();
    return results.Select(p => new Product(p.Id, p.Name, p.Price, p.Quantity));
  }

  public async Task<decimal> GetTotalInventoryValue()
  {
    decimal totalValue = await _db.Products.SumAsync(p => p.Price * p.Quantity);
    return totalValue;
  }

  public async Task<Product?> FindHighPricedProduct()
  {
    var highPricedProduct = await _db.Products.OrderByDescending(p => p.Price)
            .Select(p => new Product(p.Id, p.Name, p.Price, p.Quantity))
            .FirstOrDefaultAsync();

    if (highPricedProduct != null)
    {
      return highPricedProduct;
    }
    else
    {
      return null;
    }
  }

  public async Task<Product> UpdateProductQuantity(Guid productId, int newQuantity)
  {
    var product = await _db.Products.FirstAsync(p => p.Id == productId);
    product.Quantity = newQuantity;
    await _db.SaveChangesAsync();

    return new Product(product.Id, product.Name, product.Price, product.Quantity);
  }

  public async Task<Product> UpdateProductPrice(Guid productId, decimal newPrice)
  {
    var product = await _db.Products.FirstAsync(p => p.Id == productId);
    product.Price = newPrice;
    await _db.SaveChangesAsync();

    return new Product(product.Id, product.Name, product.Price, product.Quantity);
  }

  public async Task DeleteProduct(Guid productId)
  {
    var productToDelete = await _db.Products.FirstOrDefaultAsync(p => p.Id == productId);
    if (productToDelete != null)
    {
      _db.Products.Remove(productToDelete);
      await _db.SaveChangesAsync();
    }
    else
    {
      throw new ProductNotFoundException();
    }
  }

  private async Task<Boolean> ProductExists(string name)
  {
    return await _db.Products.AnyAsync(p => p.Name == name);
  }
}