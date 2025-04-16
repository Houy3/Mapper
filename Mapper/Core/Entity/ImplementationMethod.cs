using Mapper.Core.Entity.Common;

namespace Mapper.Core.Entity;

public record ImplementationMethod(
    string Name,
    Type ReturnType,
    EquatableArrayWrap<Parameter> ParameterList);