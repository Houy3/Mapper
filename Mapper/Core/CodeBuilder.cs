using Mapper.Core.Entity;

namespace Mapper.Core;

public static class CodeBuilder
{
    public static string Build(ImplementationType implementationInfo)
    {
        return $$"""
        namespace {{implementationInfo.Namespace}};

        public partial class {{implementationInfo.Name}} : {{implementationInfo.InterfaceName}} 
        {

        }
        """;
    }
}
