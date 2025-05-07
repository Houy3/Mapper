using Mapper.Core.Entity.Common;

namespace Mapper.Core.Entity;

public record MapperType(
    string Namespace,
    string Name,
    EquatableArrayWrap<Method> MethodList,
    EquatableArrayWrap<NamedValue> SettingOverrideList)
    : TypeId(Namespace, Name);

public record PlannedMapperType(
    string Namespace,
    string Name,
    EquatableArrayWrap<PlannedMethod> PlannedMethodList)
    : TypeId(Namespace, Name);



public record ConfiguredMapperType(
    string Namespace,
    string Name,
    EquatableArrayWrap<ConfiguredMethod> ConfiguredMethodList)
    : TypeId(Namespace, Name);
