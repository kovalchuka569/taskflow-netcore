namespace TaskFlow.SharedKernel.Domain;

public abstract class Entity<TEntityId>
{
    public TEntityId Id { get; } = default!;

    protected Entity()
    {
    }

    protected Entity(TEntityId id)
    {
        Id = id;
    }

    public override bool Equals(object? obj)
    {
        if (obj is not Entity<TEntityId> other) return false;
        if (ReferenceEquals(this, other)) return true;
        if (GetType() != other.GetType()) return false;
        return EqualityComparer<TEntityId>.Default.Equals(Id, other.Id);
    }

    public override int GetHashCode()
    {
        return EqualityComparer<TEntityId>.Default.GetHashCode(Id!);
    }

    public static bool operator ==(Entity<TEntityId> left, Entity<TEntityId> right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(Entity<TEntityId> left, Entity<TEntityId> right)
    {
        return !Equals(left, right);
    }
}