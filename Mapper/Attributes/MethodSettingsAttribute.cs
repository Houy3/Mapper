using Mapper.Core.Settings;

namespace Mapper.Attributes;

internal class MethodSettingsAttribute : GeneratedAttribute
{
    public override string Name => nameof(MethodSettingsAttribute);

    public override string Text => $$"""
        namespace {{Namespace}};

        [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
        internal class {{Name}} : {{nameof(SettingsAttribute)}}
        {
            public string[] {{IgnoreFieldListPropertyName}} = [];
        }
        """;

    public const string IgnoreFieldListPropertyName = nameof(SettingsStorage.IgnoreFieldList);
}