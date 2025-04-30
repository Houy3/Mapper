using Mapper.Core.Entity.Common;

namespace Mapper.Core.Entity;

public record Implementation(
    string Namespace,
    string Name,
    string InterfaceName,
    EquatableArrayWrap<MethodImplementation> MethodImplementationList,
    EquatableArrayWrap<StaticMappingMethod> StaticMappingMethodList)
    : TypeId(Namespace, Name);


//todo маппинг по соседней функции (массив)

public abstract record StaticMappingMethod(
    string Name,
    Variable Source,
    Variable Destination,
    EquatableArrayWrap<FieldMapping> MappingList)
{

}



public abstract record MethodImplementation(
    string Name,
    DataType ReturnType,
    EquatableArrayWrap<Variable> ParameterList)
{
    public Variable SourceVariable => ParameterList.Array[0];
}

//маппинг по соседней функции (1 параметр)
public record MethodImplementationByBaseMethod(
    string Name,
    DataType ReturnType,
    EquatableArrayWrap<Variable> ParameterList,
    string BaseMethodName)
    : MethodImplementation(Name, ReturnType, ParameterList);

//конечный маппинг (2 параметра)
public record MethodImplementationByMappingList(
    string Name,
    DataType ReturnType,
    EquatableArrayWrap<Variable> ParameterList,
    EquatableArrayWrap<FieldMapping> MappingList)
    : MethodImplementation(Name, ReturnType, ParameterList)
{
    public Variable DestinationVariable => ParameterList.Array[1];
}

public record FieldMapping(
    string? SourceFieldName,
    string? DestinationFieldName);
