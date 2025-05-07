using Mapper.Core.Entity.Common;

namespace Mapper.Core.Entity;

public record MethodSignature(
    string Name,
    DataType ReturnType,
    EquatableArrayWrap<Variable> ParameterList);

[Flags]
public enum MethodDetails
{
    None = 0,
    Public = 1 << 0,
    Protected = 1 << 1,
    Partial = 1 << 2,
    Abstract = 1 << 3,
    Override = 1 << 4,
    Static = 1 << 5,
    WithoutBody = 1 << 6,
}