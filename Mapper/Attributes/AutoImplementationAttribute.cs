using Mapper.Common;

namespace Mapper.Attributes;

public static class AutoImplementationAttributeInfo
{
    public const string Namespace = Constants.Namespace;
    public const string Name = "AutoImplementationAttribute";

    public const string FullName = Namespace + "." + Name;

    public const string Text = $$"""
        namespace {{Namespace}};

        [AttributeUsage(AttributeTargets.Interface, AllowMultiple = false, Inherited = false)]
        internal class {{Name}} : Attribute;
        """;
}