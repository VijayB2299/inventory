public class Program
{
  public static async Task Main(string[] args)
  { 
    IInventoryService inventory = new InventoryService();

    while (true)
    {
      Console.WriteLine("=========================");
      Console.WriteLine("Select an option:");
      Console.WriteLine("1. Add Product");
      Console.WriteLine("2. List Products");
      Console.WriteLine("3. Search Products by Name");
      Console.WriteLine("4. Get Total Inventory Value");
      Console.WriteLine("5. Find High Priced Product");
      Console.WriteLine("6. Update Product Quantity");
      Console.WriteLine("7. Update Product Price");
      Console.WriteLine("8. Delete Product");
      Console.WriteLine("9. Exit");
      Console.WriteLine("=========================");

      var option = Console.ReadLine();
      switch(option)
      {
        case "1":
          // Add Product
          Console.WriteLine("Enter Product Id:");
          var id = int.Parse(Console.ReadLine() ?? "0");
          Console.WriteLine("Enter Product Name:");
          var name = Console.ReadLine() ?? "";
          Console.WriteLine("Enter Product Price:");
          var price = decimal.Parse(Console.ReadLine() ?? "0");
          Console.WriteLine("Enter Product Quantity:");
          var quantity = int.Parse(Console.ReadLine() ?? "0");

          var newProduct = new Product(id, name, price, quantity);
          try
          {
            await inventory.AddProduct(newProduct);
            Console.WriteLine("Product added successfully.");
          }
          catch (DuplicateProductNameException)
          {
            Console.WriteLine("Error: A product with the same name already exists.");
          }
          break;

        case "2":
          // List Products 
          var products = await inventory.ListProducts();
          foreach (var p in products)
          {
            Console.WriteLine($"Id: {p.Id}, Name: {p.Name}, Price: {p.Price}, Quantity: {p.Quantity}");
          }
          break;

        case "3":
          // Search products by name
          Console.WriteLine("Enter product name to search:");
          var searchTerm = Console.ReadLine();

          if (searchTerm != null)
          {
            var searchResults = await inventory.SearchProductsByName(searchTerm);
            if (searchResults.Count == 0)
            {
              Console.WriteLine("No products found.");
            } else
            {
              Console.WriteLine("Search Results:");
              foreach (var p in searchResults)
              {
                Console.WriteLine($"Id: {p.Id}, Name: {p.Name}, Price: {p.Price}, Quantity: {p.Quantity}");
              }
            }
          }
        break;

        case "4":
          // Get total inventory value
          var totalValue = await inventory.GetTotalInventoryValue();
          Console.WriteLine($"Total Inventory Value: {totalValue}");
          break;

        case "5":
          // Find high priced product
          try
          {
            var highPricedProduct = await inventory.FindHighPricedProduct();
            if (highPricedProduct is not null)
            {
              Console.WriteLine($"Highest Priced Product: Id: {highPricedProduct.Id}, Name: {highPricedProduct.Name}, Price: {highPricedProduct.Price}, Quantity: {highPricedProduct.Quantity}");
            }
          }
          catch (ProductNotFoundException)
          {
            Console.WriteLine("Error: No products found in inventory.");
          }
          break;

        case "6":
          // Update product quantity
          Console.WriteLine("Enter Product Id to update quantity:");
          var prodIdForQtyUpdate = int.Parse(Console.ReadLine() ?? "0");
          Console.WriteLine("Enter new quantity:");
          var newQuantity = int.Parse(Console.ReadLine() ?? "0");
          var updatedProduct1 = await inventory.UpdateProductQuantity(prodIdForQtyUpdate, newQuantity);
          Console.WriteLine($"Updated Product Quantity: Id: {updatedProduct1.Id}, Name: {updatedProduct1.Name}, Price: {updatedProduct1.Price}, Quantity: {updatedProduct1.Quantity}");
          break;

        case "7":
          // Update product price
          Console.WriteLine("Enter Product Id to update price:");
          var prodIdForPriceUpdate = int.Parse(Console.ReadLine() ?? "0");
          Console.WriteLine("Enter new price:");
          var newPrice = decimal.Parse(Console.ReadLine() ?? "0");
          var updatedProduct2 = await inventory.UpdateProductPrice(prodIdForPriceUpdate, newPrice);
          Console.WriteLine($"Updated Product Price: Id: {updatedProduct2.Id}, Name: {updatedProduct2.Name}, Price: {updatedProduct2.Price}, Quantity: {updatedProduct2.Quantity}");
          break;
        
        case "8":
          // Delete a product
          await inventory.DeleteProduct(productId: 1);
          Console.WriteLine("Product deleted successfully.");
          break;
        
        case "9":
          // Exit
          return;
      }
    }
  }
}