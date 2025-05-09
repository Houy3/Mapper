namespace Mapper.Core.Reader;

public static class DictionaryExtension
{

    public static TValue GetValueOrDefault<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key)
    {
        dictionary.TryGetValue(key, out TValue value);
        return value;
    }
}
