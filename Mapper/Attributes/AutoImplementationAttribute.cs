namespace Mapper.Attributes;

public class AutoImplementationAttribute : GeneratedAttribute
{
    public override string Name => nameof(AutoImplementationAttribute);

    public override string Text => $$"""
        namespace {{Namespace}};

        [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
        internal class {{Name}} : {{nameof(SettingsAttribute)}};
        """;
}