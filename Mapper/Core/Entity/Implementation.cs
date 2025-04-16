using Mapper.Core.Entity.Common;

namespace Mapper.Core.Entity;

public readonly record struct Implementation(
    string Namespace,
    string InterfaceName,
    string ClassName,
    EquatableArrayWrap<Method> MethodList)
{
    public string FullClassName => Namespace + "." + ClassName;
}


