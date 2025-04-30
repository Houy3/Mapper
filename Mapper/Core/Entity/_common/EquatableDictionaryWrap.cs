namespace Mapper.Core.Entity.Common;

public sealed record EquatableDictionaryWrap<TKey, TValue>(Dictionary<TKey, TValue> Dictionary)
    where TKey : notnull
    where TValue : notnull
{
    public EquatableDictionaryWrap() : this([]) { }

    public bool Equals(EquatableDictionaryWrap<TKey, TValue> other)
        => Dictionary.SequenceEqual(other.Dictionary);

    public override int GetHashCode()
    {
        int hashCode = Dictionary.Count;
        foreach (var key in Dictionary.Keys)
            hashCode = hashCode * 11 * key.GetHashCode() * Dictionary[key].GetHashCode();

        return hashCode;
    }
}