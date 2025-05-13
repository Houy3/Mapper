using Mapper.Core.Builder;
using Mapper.Core.Entity.Common;

using static Mapper.Core.Entity.MethodDetails;

namespace Mapper.Core.Entity;

public abstract record MethodImplementation(
    MethodSignature Signature,
    MethodDetails Details)
    : BaseMethod(Signature, Details)
{
    public Variable SourceVariable = Signature.ParameterList[0];

    public Variable DestinationVariable = Signature.ParameterList.Length > 1 ? Signature.ParameterList[1] : new Variable("destination", Signature.ReturnType);

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

public record DataTypeMappingMethodImplementation(
    MethodSignature Signature,
    MethodDetails Details,
    MethodSignature? BuilderMethod,
    MethodSignature? AfterMappingMethod,
    EquatableArrayWrap<FieldMapping> MappingList)
    : MethodImplementation(Signature, Details)
{
    public override void AppendBody(TextBuilder textBuilder)
    {
        if (ParameterList.Length == 1)
        {
            if (BuilderMethod is null) {
                AppendBodyForOneParameterMethod(textBuilder);
                return;
            }
            AppendBuilderMethod(textBuilder);
        }
        AppendBodyForTwoParameterMethod(textBuilder);
    }

    private void AppendBodyForOneParameterMethod(TextBuilder textBuilder)
    {
        textBuilder
            .Append("return ");

        if (AfterMappingMethod is not null)
        {
            textBuilder.Append(AfterMappingMethod.Name, "(");
            if (AfterMappingMethod.ParameterList.Length == 2)
                textBuilder.Append(SourceVariable.Name, ", ");
        }

        textBuilder
            .AppendLine("new()")
            .AppendBlock(tb => tb.AppendLineJoin((tb, x) => AppendFieldMapping(tb, x), MappingList, ","));


        if (AfterMappingMethod is not null)
            textBuilder.Append(")");
        
        textBuilder.Append(";");
    }
    
    private void AppendBodyForTwoParameterMethod(TextBuilder textBuilder)
    {
        textBuilder
            .AppendLine((tb, x) => AppendFieldMapping(tb, x, true), MappingList)
            .Append("return ");

        if (AfterMappingMethod is not null)
        {
            textBuilder.Append(AfterMappingMethod.Name, "(");
            if (AfterMappingMethod.ParameterList.Length == 2)
                textBuilder.Append(SourceVariable.Name, ", ");
        }

        textBuilder.Append(DestinationVariable.Name);

        if (AfterMappingMethod is not null)
            textBuilder.Append(")");

        textBuilder.Append(";");

    }
    private void AppendFieldMapping(TextBuilder textBuilder, FieldMapping mapping, bool withDestinationVariable = false)
    {
        if (withDestinationVariable)
            textBuilder.Append(DestinationVariable.Name, ".");

        textBuilder.Append(mapping.DestinationField?.Name, " = ").Append(mapping.AppendSource);

        if (withDestinationVariable)
            textBuilder.Append(";");
    }


    private void AppendBuilderMethod(TextBuilder textBuilder)
    {
        textBuilder.Append("var ", DestinationVariable.Name, " = ", BuilderMethod!.Name, "(");
        if (BuilderMethod.ParameterList.Length == 1)
            textBuilder.Append(SourceVariable.Name);
        textBuilder.AppendLine(");");
    }
}