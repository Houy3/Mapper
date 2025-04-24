using Mapper.Common;

namespace Mapper.Attributes;

public static class ImportTypeMappingsAttribute
{
    public const string Namespace = Constants.Namespace;
    public const string Name = nameof(ImportTypeMappingsAttribute);

    public const string FullName = Namespace + "." + Name;

    public const string Text = $$"""
        namespace {{Namespace}};

        [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
        internal class {{Name}} : Attribute
        {
            public Type[] {{ImportPropertyName}} = [];
        }
        """;

    public const string ImportPropertyName = "Import";
}