namespace OrderProcessing.Application.DTOs;

public class OrderDto
{
    public Guid OrderId { get; set; }
    public string InvoiceAddress { get; set; }
    public string InvoiceEmailAddress { get; set; }
    public string InvoiceCreditCardNumber { get; set; }  
    public List<OrderItemDto> OrderItems { get; set; }
}