namespace OrderProcessing.Infrastructure.Entities;

public class OrderItemEntity : BaseEntity
{
    public required Guid ProductId { get; set; }
    public string Name { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
}