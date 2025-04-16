using Mapper.Core.Entity.Common;

namespace Mapper.Core.Entity;

public readonly record struct Method(
    string Name,
    Type ReturnType,
    EquatableArrayWrap<Parameter> ParameterList);