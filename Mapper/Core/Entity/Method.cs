using Mapper.Core.Entity.Common;
using Mapper.Core.Settings;

namespace Mapper.Core.Entity;

public record Method(
    string Name,
    DataType ReturnType,
    EquatableArrayWrap<Variable> ParameterList,
    EquatableArrayWrap<NamedValue> SettingOverrideList
    );

public record MethodWithSettings(
    string Name,
    DataType ReturnType,
    EquatableArrayWrap<Variable> ParameterList,
    SettingsStorage SettingsStorage
    );
