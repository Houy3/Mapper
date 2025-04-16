using Mapper.Core.Entity.Common;

namespace Mapper.Core.Entity;

public record ImplementationType(
    string Namespace,
    string InterfaceName,
    EquatableArrayWrap<ImplementationMethod> ImplementationMethodList)
{
    public string Name => InterfaceName.StartsWith("I") ? InterfaceName.Substring(1) : InterfaceName + "Impl";

    public string FullName => Namespace + "." + Name;
}


