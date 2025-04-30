using Mapper.Core.Entity.Common;
using Mapper.Core.Settings;
using Mapper.Core.TypeMapping;

namespace Mapper.Core.Entity;

public record Interface(
    string Namespace,
    string Name,
    EquatableArrayWrap<Method> MethodList,
    EquatableArrayWrap<NamedValue> SettingOverrideList)
    : TypeId(Namespace, Name);

public record InterfaceWithSettings(
    string Namespace,
    string Name,
    EquatableArrayWrap<MethodWithSettings> MethodList,
    SettingsStorage SettingsStorage,
    TypeMappingStorage TypeMappingStorage)
    : TypeId(Namespace, Name);
