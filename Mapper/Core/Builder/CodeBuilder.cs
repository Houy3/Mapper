using Mapper.Core.Entity;
using System.Text;

namespace Mapper.Core.Builder;

public static class CodeBuilder
{
    public static string Build(Implementation type)
        => new StringBuilder()
            .Append($"namespace {type.Namespace};")
            .AppendLine()
            .AppendLine($"public partial class {type.Name} : {type.InterfaceName}")
            .AppendBlock(BuildMethodList(type.MethodImplementationList.Array))
            .ToString();

    public static string BuildMethodList(MethodImplementation[] methodList)
        => string.Join("\n\n", methodList.Select(BuildMethod));//todo \n is bad
    
    public static string BuildMethod(MethodImplementation method)
        => new StringBuilder()
            .Append($"public {method.ReturnType.FullName} {method.Name}")
            .AppendLine($"({string.Join<Variable>(", ", method.ParameterList.Array)})")
            .AppendBlock(BuildMethodBody(method))
            .ToString();

    public static string BuildMethodBody(MethodImplementation method)
    {
        if (method is MethodImplementationByMappingList methodByMappingList)
            return BuildMethodBody(methodByMappingList);

        if (method is MethodImplementationByBaseMethod methodByBaseMethod)
            return BuildMethodBody(methodByBaseMethod);

        return string.Empty;
    }

    public static string BuildMethodBody(MethodImplementationByMappingList method)
    {
        var text = new StringBuilder();
        foreach (var mapping in method.MappingList.Array)
            text.AppendLine($"{mapping.SourceFieldName} = {mapping.DestinationFieldName};");
        text.Append($"return {method.DestinationVariable.Name};");
        return text.ToString();
    }

    public static string BuildMethodBody(MethodImplementationByBaseMethod method)
        => $"return {method.BaseMethodName}({method.SourceVariable.Name}, new {method.ReturnType.FullName}()); ";


    public static StringBuilder AppendBlock(this StringBuilder text, string blockBody)
        => text
            .AppendLine("{")
            .AppendLine(blockBody.Offset())
            .Append("}");

    public static string Offset(this string text)
        => "\t" + text.Replace("\n", "\n\t");//TODO \n is bad
}
