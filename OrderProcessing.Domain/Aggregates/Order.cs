using OrderProcessing.Domain.Common;
using OrderProcessing.Domain.ValueObjects;

namespace OrderProcessing.Domain.Aggregates;

public class Order : AggregateRoot
{
    public string InvoiceAddress { get; private set; }
    public string InvoiceEmail { get; private set; }
    public string InvoiceCreditCardNumber { get; private set; }
    public List<OrderItem> Items { get; private set; }

    public Order(string invoiceAddress, string invoiceEmail, string invoiceCreditCardNumber)
    {
        InvoiceAddress = invoiceAddress;
        InvoiceEmail = invoiceEmail;
        InvoiceCreditCardNumber = invoiceCreditCardNumber;
        Items = new List<OrderItem>();
    }
    
    public Order(Guid id, string invoiceAddress, string invoiceEmail, string invoiceCreditCardNumber)
    {
        Id = id;
        InvoiceAddress = invoiceAddress;
        InvoiceEmail = invoiceEmail;
        InvoiceCreditCardNumber = invoiceCreditCardNumber;
        Items = new List<OrderItem>();
    }

    public void AddItem(OrderItem item)
    {
        Items.Add(item);
    }
}