using System.Text;
using Mapper.Core.Entity;
using Mapper.Core.Entity.Common;

namespace Mapper.Core.Builder;

public static class CodeBuilder
{
    public static string Build(ImplementedMapperType type)
    {
        var t = new TextBuilder()
            .AppendLine("namespace ", type.Namespace, ";")
            .AppendLine()
            .AppendLine("public partial class ", type.Name)
            .AppendBlock(tb => AppendMethodList(tb, type.MethodImplementationList))
            .ToString();

        return t;
    }

    public static void AppendMethodList(TextBuilder textBuilder, EquatableArrayWrap<MethodImplementation> methodList)
        => textBuilder.AppendLineJoin(AppendMethod, methodList, TextBuilder.NewLine);

    public static void AppendMethod(TextBuilder textBuilder, MethodImplementation method)
        => textBuilder
            .Append(method.AppendDeclare)
            .AppendBlock(method.AppendBody);
    
}
