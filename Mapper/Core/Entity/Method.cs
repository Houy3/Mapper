using Mapper.Core.Entity.Common;

namespace Mapper.Core.Entity;

public record Method(
    string Name,
    DataType ReturnType,
    EquatableArrayWrap<Variable> ParameterList);
