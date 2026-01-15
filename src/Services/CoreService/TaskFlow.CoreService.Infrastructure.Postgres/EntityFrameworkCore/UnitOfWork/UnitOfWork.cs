using Microsoft.EntityFrameworkCore.Storage;
using TaskFlow.CoreService.Infrastructure.Postgres.EntityFrameworkCore.Context;
using TaskFlow.SharedKernel.Interfaces;

namespace TaskFlow.CoreService.Infrastructure.Postgres.EntityFrameworkCore.UnitOfWork;

public sealed class UnitOfWork(CoreDbContext dbContext) : IUnitOfWork
{
    private IDbContextTransaction? _currentTransaction;

    public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_currentTransaction is not null) return;

        _currentTransaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);
    }

    public async Task<int> CommitAsync(CancellationToken cancellationToken)
    {
        int changesCount;

        try
        {
            changesCount = await dbContext.SaveChangesAsync(cancellationToken);
            if (_currentTransaction is not null) await _currentTransaction.CommitAsync(cancellationToken);
        }
        catch
        {
            await RollbackAsync(cancellationToken);
            throw;
        }
        finally
        {
            if (_currentTransaction is not null)
            {
                await _currentTransaction.DisposeAsync();
                _currentTransaction = null;
            }
        }

        return changesCount;
    }

    public async Task RollbackAsync(CancellationToken cancellationToken)
    {
        if (_currentTransaction is not null) await _currentTransaction.RollbackAsync(cancellationToken);
    }
}