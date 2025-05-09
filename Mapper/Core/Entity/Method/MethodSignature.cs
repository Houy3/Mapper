using Mapper.Core.Entity.Common;

namespace Mapper.Core.Entity;

public record MethodSignature(
    string Name,
    DataType ReturnType,
    EquatableArrayWrap<Variable> ParameterList);
