using System.Collections;

namespace Mapper.Core.Entity.Common;

public sealed record EquatableArrayWrap<T>(T[] Array) : IEnumerable<T>
    where T : notnull
{
    public EquatableArrayWrap() : this((T[])[]) { }
    public EquatableArrayWrap(IEnumerable<T> list) : this(list.ToArray()) { }


    public int Length = Array.Length;

    public T this[int index]
    {
        get => Array[index];
    }


    public bool Equals(EquatableArrayWrap<T> other)
        => Array.SequenceEqual(other.Array);

    public override int GetHashCode()
    {
        int hashCode = Array.Length;
        foreach (var item in Array)
            hashCode = (hashCode * 11 + item.GetHashCode()) * 17;

        return hashCode;
    }

    IEnumerator<T> IEnumerable<T>.GetEnumerator()
        => Array.AsEnumerable().GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator()
        => Array.GetEnumerator();
}
