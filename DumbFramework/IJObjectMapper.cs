using Newtonsoft.Json.Linq;

namespace DumbFramework;

public static class JObjectMapper
{
    public static string ToString(JObject json)
        => json.ToString();
}
