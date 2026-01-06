using Microsoft.AspNetCore.Mvc;

using Inventory.Domain;
using Inventory.Services;
using Inventory.Exceptions;
using Inventory.Dtos;


namespace Inventory.Controllers;

[ApiController]
[Route("api/products")]
public class ProductController : ControllerBase
{
  private readonly IInventoryService inventory;
  private int _nextId = 1;

  public ProductController(IInventoryService inventory)
  {
    this.inventory = inventory;
  }
  
  [HttpPost]
  public async Task<ActionResult<ProductDto>> CreateProduct(CreateProductDto payload)
  {

    try
    {
      var product = await inventory.AddProduct(
        _nextId++,
        payload.Name,
        payload.Price,
        payload.Quantity
      );

      return Ok(ToResponse(product));
    }
    catch (DuplicateProductNameException)
    {
      return Conflict("A product with the same name already exists.");
    }
  }

  [HttpGet]
  public async Task<ActionResult<IEnumerable<ProductDto>>> GetProducts()
  {
    var products = await inventory.ListProducts();
    return Ok(products.Select(ToResponse));
  }

  [HttpGet("{id:int}")]
  public async Task<ActionResult<ProductDto>> GetProduct(int id)
  {
    try
    {
      var product = await inventory.GetProduct(id);
      return Ok(ToResponse(product));
    }
    catch (ProductNotFoundException)
    {
      return NotFound("Product doesn't exist.");
    }
  }

  [HttpPut("{id:int}/quantity")]
  public async Task<ActionResult<ProductDto>> UpdateProductQuantity(int id, UpdateProductQuantityDto payload)
  {
    var product = await inventory.UpdateProductQuantity(id, payload.Quantity);
    return Ok(ToResponse(product));
  }

  [HttpPut("{id:int}/price")]
  public async Task<ActionResult<ProductDto>> UpdateProductPrice(int id, UpdateProductPriceDto payload)
  {
    var product = await inventory.UpdateProductPrice(id, payload.Price);
    return Ok(ToResponse(product));
  }

  [HttpDelete("{id:int}")]
  public async Task<IActionResult> DeleteProduct(int id)
  {
    try
    {  
      await inventory.DeleteProduct(id);
      return NoContent();
    }
    catch(ProductNotFoundException)
    {
      return NotFound("Product doesn't exist.");
    }
  }

   private static ProductDto ToResponse(Product p) => new ProductDto
    {
        Id = p.Id,
        Name = p.Name,
        Price = p.Price,
        Quantity = p.Quantity
    };

}