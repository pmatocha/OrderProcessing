using System.ComponentModel.DataAnnotations;
using JsmeNaLede.API.Model.Interfaces;

namespace OrderProcessing.Infrastructure.Entities;

public class OrderEntity : BaseEntity, IAuditable
{
    [StringLength(255)]
    public string InvoiceAddress { get; set; }
    
    [StringLength(255)]
    public string InvoiceEmail { get; set; }
    
    [StringLength(255)]
    public string InvoiceCreditCardNumber { get; set; }  

    public DateTimeOffset CreatedDate { get; set; }
    public DateTimeOffset? UpdatedDate { get; set; }
    
    public ICollection<OrderItemEntity> OrderItems { get; set; } = new List<OrderItemEntity>();
}
