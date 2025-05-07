namespace Mapper.Core.Entity.Common;

public sealed record EquatableArrayWrap<T>(T[] Array)
    where T : notnull
{
    public EquatableArrayWrap() : this([]){ }

    public bool Equals(EquatableArrayWrap<T> other)
        => Array.SequenceEqual(other.Array);

    public override int GetHashCode()
    {
        int hashCode = Array.Length;
        foreach (var item in Array)
            hashCode = (hashCode * 11 + item.GetHashCode()) * 17;

        return hashCode;
    }
}
