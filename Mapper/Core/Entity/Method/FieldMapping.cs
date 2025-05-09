using Mapper.Core.Builder;

namespace Mapper.Core.Entity;

public record FieldMapping(
    Variable SourceVariable,
    Field? SourceField,
    string? DestinationFieldName)
{
    public string SourceFieldRef => SourceVariable.Name + "." + SourceField!.Name;

    public virtual void AppendSource(TextBuilder textBuilder)
    {
        if (SourceField is null)
            textBuilder.Append("Unknown /* No source found. */");
        else
            textBuilder.Append(SourceFieldRef);
    }
}
public record SimpleFieldMapping(
    Variable SourceVariable,
    Field? SourceField,
    string? DestinationFieldName)
    : FieldMapping(SourceVariable, SourceField, DestinationFieldName)
{
    public override void AppendSource(TextBuilder textBuilder)
    {
        if (SourceField is null)
            textBuilder.Append("Unknown /* No source found. */");
        else
            textBuilder.Append(SourceFieldRef);
    }
}
