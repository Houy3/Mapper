using Mapper.Core.Entity.Common;
using Mapper.Core.Settings;

namespace Mapper.Core.Entity;

public abstract record BaseMethod(
    MethodSignature Signature,
    MethodDetails Details)
{
    public string Name => Signature.Name;
    public DataType ReturnType => Signature.ReturnType;
    public EquatableArrayWrap<Variable> ParameterList => Signature.ParameterList;


    public DataType SourceType => ParameterList[0].Type;
    public DataType DestinationType => Signature.ReturnType;


    public bool Is(MethodDetails detail)
        => Details.HasFlag(detail);
    public bool IsNot(MethodDetails detail)
        => !Is(detail);
}

public record Method(
    MethodSignature Signature,
    MethodDetails Details,
    MethodSource Source,
    EquatableArrayWrap<NamedValue> SettingOverrideList)
    : BaseMethod(Signature, Details)
{
    public bool IsFrom(MethodSource source)
        => Source == source;
}

public record MappingMethod(
    MethodSignature Signature,
    MethodDetails Details,
    MappingMethod? ConnectedMethod,
    EquatableArrayWrap<NamedValue> SettingOverrideList)
    : BaseMethod(Signature, Details)
{
    public MappingMethod(MethodSignature signature, MethodDetails details,  EquatableArrayWrap<NamedValue> settingOverrideList)
        : this(signature, details, null, settingOverrideList) { }
}

public record ConfiguredMethod(
    MethodSignature Signature,
    MethodDetails Details,
    MappingMethod? ConnectedMethod,
    SettingsStorage SettingsStorage)
    : BaseMethod(Signature, Details);


[Flags]
public enum MethodDetails
{
    None = 0,
    Public = 1 << 0,
    Static = 1 << 1,
    Override = 1 << 2,
    Partial = 1 << 3,
    WithoutBody = 1 << 4,
}

public enum MethodSource
{
    Class,
    Interface
}
