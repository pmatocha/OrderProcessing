namespace OrderProcessing.Domain.Common;

public abstract class AggregateRoot
{
    public Guid Id { get; protected set; }

    protected AggregateRoot()
    {
        Id = Guid.NewGuid();
    }
}