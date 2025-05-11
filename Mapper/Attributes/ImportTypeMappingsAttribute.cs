namespace Mapper.Attributes;

public class ImportTypeMappingsAttribute : GeneratedAttribute
{
    public override string Name => nameof(ImportTypeMappingsAttribute);

    public override string Text => $$"""
        namespace {{Namespace}};

        [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
        internal class {{Name}} : Attribute
        {
            public Type[] {{ImportPropertyName}} = [];
        }
        """;

    public const string ImportPropertyName = "Import";
}