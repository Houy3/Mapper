using Mapper.Core.Entity.Common;

namespace Mapper.Core.Entity;

public record ImplementedMapperType(
    string Namespace,
    string Name,
    EquatableArrayWrap<MethodImplementation> MethodImplementationList)
    : TypeId(Namespace, Name);
