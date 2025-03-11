using Microsoft.EntityFrameworkCore;
using OrderProcessing.Application.Repositories;

namespace OrderProcessing.Infrastructure.Repositories;

public class InventoryRepository(OrderProcessingDbContext dbContext) : IInventoryRepository
{
    public async Task<bool> IsInStock(Guid productId, int quantity, CancellationToken cancellationToken = default)
    {
        var inventoryEntity = await dbContext.Inventories.FirstOrDefaultAsync(x => x.ProductId == productId, cancellationToken: cancellationToken);

        return inventoryEntity != null && inventoryEntity.AvailableQuantity >= quantity;
    }
}