using Microsoft.AspNetCore.Mvc;

using Inventory.Models;
using Inventory.Services;
using Inventory.Exceptions;
using Inventory.Dtos;
using Microsoft.AspNetCore.Authorization;
using Inventory.Common.Pagination;

namespace Inventory.Controllers;

[ApiController]
[Route("api/products")]
public class ProductController : ControllerBase
{
  private readonly IInventoryService inventory;

  public ProductController(IInventoryService inventory)
  {
    this.inventory = inventory;
  }
  
  [Authorize(Roles = "Admin")]
  [HttpPost]
  public async Task<ActionResult<ProductDto>> CreateProduct(CreateProductDto payload)
  {
    var product = await inventory.AddProduct(payload);
    return Ok(product);
  }

  [Authorize(Roles = "Admin,User")]
  [HttpGet]
  public async Task<ActionResult<PaginatedList<ProductDto>>> GetProducts([FromQuery] PageParameters pageParameters, [FromQuery] SortParameters sortParameters, [FromQuery] FilterParameters filterParameters)
  {
    var products = await inventory.ListProducts(pageParameters, sortParameters, filterParameters);
    return Ok(products);
  }

  [Authorize(Roles = "Admin,User")]
  [HttpGet("{id:Guid}")]
  public async Task<ActionResult<ProductDto>> GetProduct(Guid id)
  {
    var product = await inventory.GetProduct(id);
    return Ok(product);
  }

  [Authorize(Roles = "Admin")]
  [HttpPut("{id:Guid}/quantity")]
  public async Task<ActionResult<ProductDto>> UpdateProductQuantity(Guid id, UpdateProductQuantityDto payload)
  {
    var product = await inventory.UpdateProductQuantity(id, payload.Quantity);
    return Ok(product);
  }

  [Authorize(Roles = "Admin")]
  [HttpPut("{id:Guid}/price")]
  public async Task<ActionResult<ProductDto>> UpdateProductPrice(Guid id, UpdateProductPriceDto payload)
  {
    var product = await inventory.UpdateProductPrice(id, payload.Price);
    return Ok(product);
  }

  [Authorize(Roles = "Admin")]
  [HttpDelete("{id:Guid}")]
  public async Task<IActionResult> DeleteProduct(Guid id)
  {
    await inventory.DeleteProduct(id);
    return NoContent();
  }
}