using Mapper.Core.Entity.Common;

namespace Mapper.Core.Entity;

public record Interface(
    string Namespace,
    string Name,
    EquatableArrayWrap<Method> MethodList);
