namespace Mapper.Attributes;

public class ProjectSettingsAttribute : GeneratedAttribute
{
    public override string Name => nameof(ProjectSettingsAttribute);

    public override string Text => $$"""
        namespace {{Namespace}};

        [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
        internal class {{Name}} : {{nameof(SettingsAttribute)}};
        """;
}