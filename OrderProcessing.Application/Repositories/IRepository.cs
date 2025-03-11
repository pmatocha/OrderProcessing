namespace OrderProcessing.Application.Repositories;

public interface IRepository<TEntity> where TEntity : class
{
    Task<TEntity?> GetAsync(Guid id, CancellationToken cancellationToken);
    
    Task SaveAsync(TEntity entity, CancellationToken cancellationToken);
}