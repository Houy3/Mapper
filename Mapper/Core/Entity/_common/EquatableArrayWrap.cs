namespace Mapper.Core.Entity.Common;

public readonly record struct EquatableArrayWrap<T>(T[] Array)
    where T : struct
{
    public readonly bool Equals(EquatableArrayWrap<T> other)
        => Array.SequenceEqual(other.Array);

    public readonly override int GetHashCode()
    {
        int hashCode = Array.Length;
        foreach (var val in Array)
            hashCode = (hashCode * 11 + val.GetHashCode()) * 17;

        return hashCode;
    }
}