using System.ComponentModel.DataAnnotations;

namespace OrderProcessing.Infrastructure.Entities;

public class OrderItemEntity : BaseEntity
{
    public required Guid ProductId { get; set; }
    
    [StringLength(255)]
    public string Name { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
}