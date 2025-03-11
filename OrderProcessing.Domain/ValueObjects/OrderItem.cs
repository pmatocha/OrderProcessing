namespace OrderProcessing.Domain.ValueObjects;

public class OrderItem
{
    public Guid ProductId { get; }
    public string ProductName { get; }
    public int Quantity { get; }
    public decimal Price { get; }

    public OrderItem(Guid productId, string productName, int quantity, decimal price)
    {
        ProductName = productName;
        Quantity = quantity;
        Price = price;
        ProductId = productId;
    }
}