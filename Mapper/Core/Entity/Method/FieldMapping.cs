using Mapper.Core.Builder;

namespace Mapper.Core.Entity;

public record FieldMapping(
    string SourceParameterName,
    Field? SourceField,
    Field? DestinationField,
    string? ErrorMessage = null)
{
    public virtual void AppendSource(TextBuilder textBuilder)
    {
        textBuilder.Append(SourceParameterName, ".", SourceField?.Name);

        if (ErrorMessage is not null)
            textBuilder.Append("/* ", ErrorMessage, " */");
    }
}

public record FieldMappingByMethod(
    string SourceParameterName,
    Field? SourceField,
    Field? DestinationField,
    TypeMappingMethodId Method)
    : FieldMapping(SourceParameterName, SourceField, DestinationField)
{
    public override void AppendSource(TextBuilder textBuilder)
    {
        textBuilder.Append(Method.Class.FullName, ".", Method.Name, "(", SourceParameterName, ".", SourceField?.Name, ")");
    }
}
