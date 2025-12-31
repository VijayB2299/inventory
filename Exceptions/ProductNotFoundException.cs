public class ProductNotFoundException : Exception
{
  public override string Message
  {
    get
    {
      return "The requested product was not found in the inventory.";
    }
  }
}