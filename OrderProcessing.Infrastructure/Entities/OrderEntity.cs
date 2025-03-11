using JsmeNaLede.API.Model.Interfaces;

namespace OrderProcessing.Infrastructure.Entities;

public class OrderEntity : BaseEntity, IAuditable
{
    public string InvoiceAddress { get; set; }
    public string InvoiceEmail { get; set; }
    public string InvoiceCreditCardNumber { get; set; }  

    public DateTimeOffset CreatedDate { get; set; }
    public DateTimeOffset? UpdatedDate { get; set; }
    
    public ICollection<OrderItemEntity> OrderItems { get; set; } = new List<OrderItemEntity>();
}
