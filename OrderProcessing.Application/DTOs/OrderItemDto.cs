namespace OrderProcessing.Application.DTOs;

public record OrderItemDto(Guid ProductId, string ProductName, int ProductAmount, decimal ProductPrice);
