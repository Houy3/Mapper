using Newtonsoft.Json.Linq;

namespace DumbFramework;

public static class JObjectMapper
{
    public static string ToJObject(JObject jobject)
        => jobject.ToString();

    public static JObject ToString(string json)
        => JObject.Parse(json);
}
