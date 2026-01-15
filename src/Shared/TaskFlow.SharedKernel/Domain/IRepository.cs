namespace TaskFlow.SharedKernel.Domain;

public interface IRepository<TEntity, in TEntityId>
    where TEntity : Entity<TEntityId>
    where TEntityId : StronglyTypedId<TEntityId>
{
    Task<TEntity?> GetByIdAsync(TEntityId id, CancellationToken cancellationToken = default);
    Task AddAsync(TEntity entity, CancellationToken cancellationToken = default);
    void Update(TEntity entity);
    void Remove(TEntity entity);
}