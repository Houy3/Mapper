using Mapper.Core.Builder;
using Mapper.Core.Entity.Common;

using static Mapper.Core.Entity.MethodDetails;

namespace Mapper.Core.Entity;

public abstract record MethodImplementation(
    MethodSignature Signature,
    MethodDetails Details)
    : BaseMethod(Signature, Details)
{
    private readonly Variable _sourceVariable = Signature.ParameterList[0];
    public Variable SourceVariable => _sourceVariable;

    private readonly Variable _destinationVariable = Signature.ParameterList.Length > 1 ? Signature.ParameterList[1] : new Variable("destination", Signature.ReturnType);
    public Variable DestinationVariable => _destinationVariable;

    public void AppendDeclare(TextBuilder textBuilder)
    {
        AppendModifierList(textBuilder);

        textBuilder
            .Append(ReturnType.FullName, " ", Name)
            .Append("(").AppendJoin(ParameterList, ", ").AppendLine(")");
    }

    public void AppendModifierList(TextBuilder textBuilder)
    {
        if (Is(Public))
            textBuilder.Append("public ");
        else
            textBuilder.Append("protected ");

        if (Is(Static))
            textBuilder.Append("static ");

        if (Is(Override))
            textBuilder.Append("override ");

        if (Is(Partial))
            textBuilder.Append("partial ");
    }

    public abstract void AppendBody(TextBuilder textBuilder);
}


//todo маппинг по соседней функции (массив)


public record MethodImplementationByOtherMethod(
    MethodSignature Signature,
    MethodDetails Details,
    MethodSignature OtherMethodSignature)
    : MethodImplementation(Signature, Details)
{
    public override void AppendBody(TextBuilder textBuilder)
    {
        textBuilder.Append("return ", OtherMethodSignature.Name, "(", SourceVariable.Name);

        if (OtherMethodSignature.ParameterList.Length == 2)
            textBuilder.Append(", ", DestinationVariable.Name);

        textBuilder.Append(");");
    }
}

public record MappingMethodImplementation(
    MethodSignature Signature,
    MethodDetails Details,
    EquatableArrayWrap<FieldMapping> MappingList)
    : MethodImplementation(Signature, Details)
{
    public override void AppendBody(TextBuilder textBuilder)
    {
        if (ParameterList.Length == 2)
            AppendBodyForTwoParameterMethod(textBuilder);
        else
            AppendBodyForOneParameterMethod(textBuilder);
    }

    private void AppendBodyForOneParameterMethod(TextBuilder textBuilder)
        => textBuilder
            .AppendLine("return new()")
            .AppendBlock(tb => tb.AppendLineJoin((tb, x) => AppendFieldMapping(tb, x), MappingList, ","), "{", "};");
    
    private void AppendBodyForTwoParameterMethod(TextBuilder textBuilder)
        => textBuilder
            .AppendLine((tb, x) => AppendFieldMapping(tb, x, true), MappingList)
            .Append("return ", DestinationVariable.Name, ";");
    
    private void AppendFieldMapping(TextBuilder textBuilder, FieldMapping mapping, bool withDestinationVariable = false)
    {
        if (withDestinationVariable)
            textBuilder.Append(DestinationVariable.Name, ".");

        textBuilder.Append(mapping.DestinationFieldName, " = ").Append(mapping.AppendSource);

        if (withDestinationVariable)
            textBuilder.Append(";");
    }

}