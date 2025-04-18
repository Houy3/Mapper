using Mapper.Core.Entity;
using System.Text;

namespace Mapper.Core.Builder;

public static class CodeBuilder
{
    public static string Build(Implementation implementation)
    {
        var text = new StringBuilder()
            .Append("namespace ")
            .Append(implementation.Namespace)
            .AppendLine(";")
            .AppendLine()
            .Append("public partial class ")
            .Append(implementation.Name)
            .Append(" : ")
            .AppendLine(implementation.InterfaceName)
            .AppendLine("{")
            .Append(BuildMethodList(implementation.MethodImplementationList.Array).Offset())
            .AppendLine("}");

        return text.ToString();
    }

    public static string BuildMethodList(MethodImplementationByMappingList[] methodImplementationList)
    {//todo \n is bad
        return string.Join("\n\n", methodImplementationList.Select(BuildMethod));
    }

    public static string BuildMethod(MethodImplementationByMappingList methodImplementation)
    {
        var text = new StringBuilder()
            .Append("public ")
            .Append(methodImplementation.ReturnType.FullName)
            .Append(" ")
            .Append(methodImplementation.Name)
            .Append("(")
            .Append(string.Join<Variable>(", ", methodImplementation.ParameterList.Array))
            .AppendLine(")")
            .AppendLine("{")
            .AppendLine(BuildMethodBody(methodImplementation).Offset())
            .Append("}");

        return text.ToString();
    }

    public static string BuildMethodBody(MethodImplementationByMappingList methodImplementation)
    {
        var text = new StringBuilder();

        if (!methodImplementation.IsDestinationVariableInited)
            text
                .Append("var ")
                .Append(methodImplementation.DestinationVariable.Name)
                .Append(" = new ")
                .Append(methodImplementation.DestinationVariable.Type.FullName)
                .AppendLine("();");


        foreach (var mapping in methodImplementation.MappingList.Array)
            text
                .Append(mapping.SourceFieldName)
                .Append(" = ")
                .Append(mapping.DestinationFieldName)
                .AppendLine(";");

        text
            .Append("return ")
            .Append(methodImplementation.DestinationVariable.Name)
            .Append(";");

        return text.ToString();
    }



    public static string Offset(this string text)
        => "\t" + text.Replace("\n", "\n\t");//TODO вообще капец грех
}
