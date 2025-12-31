public class DuplicateProductNameException : Exception
{
  public override string Message
  {
    get
    {
      return "A product with the same name already exists in the inventory.";
    }
  }
}