using Newtonsoft.Json.Linq;
using System.ComponentModel;

namespace DumbFramework;

[Description]
public interface IJObjectMapper
{
    public string ToString(JObject json);
}
