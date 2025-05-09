using Mapper.Common;

namespace Mapper.Attributes;

public static class ProjectSettingsAttribute
{
    public const string Namespace = Constants.Namespace;
    public const string Name = nameof(ProjectSettingsAttribute);

    public const string FullName = Namespace + "." + Name;

    public const string Text = $$"""
        namespace {{Namespace}};

        [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
        internal class {{Name}} : {{SettingsAttribute.Name}};
        """;
}