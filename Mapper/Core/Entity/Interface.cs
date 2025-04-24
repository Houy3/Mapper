using Mapper.Core.Entity.Common;
using Mapper.Core.Settings;

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
    SettingsStorage SettingsStorage)
    : TypeId(Namespace, Name);
