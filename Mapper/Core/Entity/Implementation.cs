using Mapper.Core.Entity.Common;

namespace Mapper.Core.Entity;

public record Implementation(
    string Namespace,
    string Name,
    string InterfaceName,

    EquatableArrayWrap<MethodImplementationByMappingList> MethodImplementationList)
{
    public string FullName => Namespace + "." + Name;
}

public record Type;


//todo маппинг по соседней функции (массив - 1 параметр)
//todo маппинг по соседней функции (массив - 2 параметра) ???

public record MethodImplementation;



//маппинг по соседней функции (1 параметр)
public record MethodImplementationByBaseMethod(
    string Name,
    DataType ReturnType,
    EquatableArrayWrap<Variable> ParameterList,
    Method? BaseMethod)
    : Method(Name, ReturnType, ParameterList);

//конечный маппинг (2 параметра)
public record MethodImplementationByMappingList(
    string Name,
    DataType ReturnType,
    EquatableArrayWrap<Variable> ParameterList,
    Variable DestinationVariable,
    bool IsDestinationVariableInited,
    EquatableArrayWrap<FieldMapping> MappingList)
    : Method(Name, ReturnType, ParameterList);

public record FieldMapping(
    string SourceFieldName,
    string DestinationFieldName);
