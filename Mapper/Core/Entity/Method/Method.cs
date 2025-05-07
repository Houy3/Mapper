using Mapper.Core.Entity.Common;
using Mapper.Core.Settings;
using Mapper.Core.TypeMapping;

namespace Mapper.Core.Entity;

public record Method(
    MethodSignature Signature,
    MethodDetails Details,
    MethodSource Source,
    EquatableArrayWrap<NamedValue> SettingOverrideList);

public record PlannedMethod(
    MethodSignature Signature,
    MethodDetails Details,
    MethodSource Source,
    EquatableArrayWrap<NamedValue> SettingOverrideList,
    bool IsReal,
    PlannedMethod? ConnectedMethod);


public record ConfiguredMethod(
    MethodSignature Signature,
    MethodDetails Details,
    MethodSource Source,
    SettingsStorage SettingsStorage,
    TypeMappingStorage TypeMappingStorage);



public enum MethodSource
{
    Original,
    BaseType,
    Interface
}
