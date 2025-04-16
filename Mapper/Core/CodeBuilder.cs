using Mapper.Core.Entity;

namespace Mapper.Core;

public static class CodeBuilder
{
    public static string Build(Implementation implementationInfo)
    {
        return $$"""
        namespace {{implementationInfo.Namespace}};

        public partial class {{implementationInfo.ClassName}} : {{implementationInfo.InterfaceName}} 
        {

        }
        """;
    }
}
