using Mapper.Core.Entity.Common;
using Mapper.Core.Settings;

namespace Mapper.Core.Entity;

public record Interface(
    string Namespace,
    string Name,
    EquatableArrayWrap<Method> MethodList,
    EquatableArrayWrap<SettingOverride> SettingOverrideList
    );

public record InterfaceWithSettings(
    string Namespace,
    string Name,
    EquatableArrayWrap<MethodWithSettings> MethodList,
    SettingsStorage SettingsStorage
    );
