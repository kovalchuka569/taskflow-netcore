using Microsoft.EntityFrameworkCore;
using TaskFlow.CoreService.Infrastructure.Postgres.EntityFrameworkCore.Context;
using TaskFlow.SharedKernel.Domain;

namespace TaskFlow.CoreService.Infrastructure.Postgres.EntityFrameworkCore.Repositories;

public abstract class Repository<TEntity, TEntityId>(CoreDbContext dbContext)
    where TEntity : Entity<TEntityId>
    where TEntityId : StronglyTypedId<TEntityId>
{
    public async Task<TEntity?> GetByIdAsync(TEntityId id, CancellationToken cancellationToken)
    {
        return await dbContext
            .Set<TEntity>()
            .SingleOrDefaultAsync(e => e.Id.Equals(id), cancellationToken);
    }

    public async Task AddAsync(TEntity entity, CancellationToken cancellationToken)
    {
        await dbContext
            .Set<TEntity>()
            .AddAsync(entity, cancellationToken);
    }

    public void Update(TEntity entity)
    {
        dbContext
            .Set<TEntity>()
            .Update(entity);
    }

    public void Remove(TEntity entity)
    {
        dbContext
            .Set<TEntity>()
            .Remove(entity);
    }
}