using Mapper.Core.Entity.Common;

namespace Mapper.Core.Entity;

public abstract record MethodImplementation(
    MethodSignature Signature,
    MethodDetails Details)
{
    public Variable _sourceVariable = Signature.ParameterList.Array[0];
    public Variable SourceVariable => _sourceVariable;

    public Variable _destinationVariable = Signature.ParameterList.Array[1] ?? new Variable("destination", Signature.ReturnType);
    public Variable DestinationVariable => _destinationVariable;

    public abstract string Body { get; }
}


//todo маппинг по соседней функции (массив)


public record MethodImplementationByOtherMethod(
    MethodSignature Signature,
    MethodDetails Details,
    string OtherMethodName)
    : MethodImplementation(Signature, Details)
{
    public override string Body => "";
}

public record MappingMethodImplementation(
    MethodSignature Signature,
    MethodDetails Details,
    EquatableArrayWrap<FieldMapping> MappingList)
    : MethodImplementation(Signature, Details)
{
    public override string Body => "";
}

public record FieldMapping(
    string? SourceFieldName,
    string? DestinationFieldName);

