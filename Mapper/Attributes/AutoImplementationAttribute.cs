using Mapper.Common;

namespace Mapper.Attributes;

public static class AutoImplementationAttributeInfo
{
    public const string Namespace = Constants.Namespace;
    public const string ClassName = "AutoImplementationAttribute";

    public const string FullClassName = Namespace + "." + ClassName;

    public const string Text = $$"""
        namespace {{Namespace}};

        [AttributeUsage(AttributeTargets.Interface, AllowMultiple = false, Inherited = false)]
        internal class {{ClassName}} : Attribute;
        """;
}