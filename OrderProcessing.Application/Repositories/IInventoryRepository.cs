namespace OrderProcessing.Application.Repositories;

public interface IInventoryRepository
{
    Task<bool> IsInStock(Guid productId, int quantity, CancellationToken cancellationToken = default);
}