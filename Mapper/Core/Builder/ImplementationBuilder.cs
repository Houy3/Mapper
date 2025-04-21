using Mapper.Core.Entity;
using Mapper.Core.Entity.Common;

namespace Mapper.Core.Builder;

public static class ImplementationBuilder
{
    public static Implementation Implement(this Interface @interface)
        => new(
            @interface.Namespace,
            GetImplementationName(@interface.Name),
            @interface.Name,
            ImplementList(@interface.MethodList.Array));


    public static EquatableArrayWrap<MethodImplementation> ImplementList(Method[] methodList)
    {
        var implementationList = new List<MethodImplementation>(methodList.Length);
        foreach (var method in methodList)
            implementationList.AddRange(CreateMethodImplementation(method, methodList));

        return new([.. implementationList]);
    }

    public static MethodImplementation[] CreateMethodImplementation(Method method, Method[] methodList)
    {
        var methodParameterCount = method.ParameterList.Array.Length;

        if (methodParameterCount == 1)
            return CreateMethodImplementationByBaseMethod(method, methodList);
        else if (methodParameterCount == 2)
            return CreateMethodImplementationByMappingList(method);
        else
            return [];
    }


    #region MethodImplementationByMappingList

    public static MethodImplementation[] CreateMethodImplementationByMappingList(Method method)
    {
        //первый параметр - источник
        var sourceParameter = method.ParameterList.Array[0];
        var sourceVariable = new Variable(sourceParameter.Name, sourceParameter.Type);

        //второй параметр - приемник
        var destinationParameter = method.ParameterList.Array[1];
        if (destinationParameter.Type != method.ReturnType)
            return [];
        var destinationVariable = new Variable(destinationParameter.Name, destinationParameter.Type);

        return [CreateMethodImplementationByMappingList(method, sourceVariable, destinationVariable)];
    }

    public static MethodImplementationByMappingList CreateMethodImplementationByMappingList(Method method, Variable sourceVariable, Variable destinationVariable)
        => new(
            method.Name,
            method.ReturnType,
            method.ParameterList,
            new(MapFieldList(sourceVariable, destinationVariable)));


    //todo map by rule
    public static FieldMapping[] MapFieldList(Variable sourceVariable, Variable destinationVariable)
    {
        var mappingList = new List<FieldMapping>();
        var sourcePropertyDictionary = sourceVariable.Type.FieldList.Array.ToDictionary(x => x.Name, x => x);
        foreach (var destinationProperty in destinationVariable.Type.FieldList.Array)
        {
            if (sourcePropertyDictionary.TryGetValue(destinationProperty.Name, out var sourceProperty))
            {
                mappingList.Add(new(destinationVariable.Name + '.' + destinationProperty.Name, sourceVariable.Name + '.' + sourceProperty.Name));
            }
        }
        return [.. mappingList];
    }

    #endregion

    #region MethodImplementationByBaseMethod

    public static MethodImplementation[] CreateMethodImplementationByBaseMethod(Method method, Method[] methodList)
    {
        var sourceType = method.ParameterList.Array[0].Type;
        var destinationType = method.ReturnType;

        //ищем базовый метод
        var baseMethod = methodList
            .FirstOrDefault(x =>
                x.Name == method.Name
                && x.ReturnType == method.ReturnType
                && x.ParameterList.Array.Length == 2
                && x.ParameterList.Array[0].Type == sourceType
                && x.ParameterList.Array[1].Type == destinationType);


        //создаем базовый метод с реализацией, если он не нашелся
        MethodImplementationByMappingList? baseMethodImplementation = null;
        if (baseMethod is null)
        {
            var sourceVariable = new Variable("source", sourceType);
            var destinationVariable = new Variable("destination", destinationType);

            baseMethod = new Method(method.Name, method.ReturnType, new([sourceVariable, destinationVariable]));

            baseMethodImplementation = CreateMethodImplementationByMappingList(baseMethod, sourceVariable, destinationVariable);
        }

        var methodImplementation = new MethodImplementationByBaseMethod(
            method.Name,
            method.ReturnType,
            method.ParameterList,
            baseMethod!.Name);

        if (baseMethodImplementation is not null)
            return [methodImplementation, baseMethodImplementation];
        else
            return [methodImplementation];
    }

    #endregion


    public static string GetImplementationName(string interfaceName)
        => interfaceName.StartsWith("I") ? interfaceName.Substring(1) : interfaceName + "Impl";
}
