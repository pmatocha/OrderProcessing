using JsmeNaLede.API.Model.Interfaces;

namespace OrderProcessing.Infrastructure.Entities;

public class InventoryEntity : BaseEntity, IAuditable
{
    public required Guid ProductId { get; set; }
    
    public int AvailableQuantity { get; set; }
    public DateTimeOffset CreatedDate { get; set; }
    public DateTimeOffset? UpdatedDate { get; set; }
}