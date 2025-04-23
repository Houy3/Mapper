using Mapper.Core.Entity;
using Mapper.Core.Entity.Common;
using Mapper.Core.Settings;

namespace Mapper.Core.Builder;

public static class ImplementationBuilder
{
    public static Implementation Implement(this InterfaceWithSettings @interface)
        => new(
            @interface.Namespace,
            GetImplementationName(@interface.Name),
            @interface.Name,
            ImplementList(@interface.MethodList.Array));


    public static EquatableArrayWrap<MethodImplementation> ImplementList(MethodWithSettings[] methodList)
    {
        var implementationList = new List<MethodImplementation>(methodList.Length);
        foreach (var method in methodList)
            implementationList.AddRange(CreateMethodImplementation(method, methodList));

        return new([.. implementationList]);
    }

    public static MethodImplementation[] CreateMethodImplementation(MethodWithSettings method, MethodWithSettings[] methodList)
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

    public static MethodImplementation[] CreateMethodImplementationByMappingList(MethodWithSettings method)
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

    public static MethodImplementationByMappingList CreateMethodImplementationByMappingList(MethodWithSettings method, Variable sourceVariable, Variable destinationVariable)
        => new(
            method.Name,
            method.ReturnType,
            method.ParameterList,
            new(MapFieldList(sourceVariable, destinationVariable, method.SettingsStorage)));


    //todo map by rule
    public static FieldMapping[] MapFieldList(Variable sourceVariable, Variable destinationVariable, SettingsStorage settings)
    {
        var mappingList = new List<FieldMapping>();

        if (settings.MappingRule == MappingRuleEnum.MapByDestination)
        {
            var sourcePropertyDictionary = sourceVariable.Type.FieldList.Array.ToDictionary(x => x.Name, x => x);
            foreach (var destinationProperty in destinationVariable.Type.FieldList.Array)
            {
                if (sourcePropertyDictionary.TryGetValue(destinationProperty.Name, out var sourceProperty))
                {
                    mappingList.Add(new(destinationVariable.Name + '.' + destinationProperty.Name, sourceVariable.Name + '.' + sourceProperty.Name));
                }
                else
                {
                    mappingList.Add(new(destinationVariable.Name + '.' + destinationProperty.Name, null));
                }
            }
        }

        if (settings.MappingRule == MappingRuleEnum.MapBySource)
        {
            var destinationPropertyDictionary = destinationVariable.Type.FieldList.Array.ToDictionary(x => x.Name, x => x);
            foreach (var sourceProperty in sourceVariable.Type.FieldList.Array)
            {
                if (destinationPropertyDictionary.TryGetValue(sourceProperty.Name, out var destinationProperty))
                {
                    mappingList.Add(new(destinationVariable.Name + '.' + sourceProperty.Name, sourceVariable.Name + '.' + sourceProperty.Name));
                }
                else
                {
                    mappingList.Add(new(null, sourceVariable.Name + '.' + sourceProperty.Name));
                }
            }
        }

        if (settings.MappingRule == MappingRuleEnum.MapOnlyPairs)
        {
            var sourcePropertyDictionary = sourceVariable.Type.FieldList.Array.ToDictionary(x => x.Name, x => x);
            foreach (var destinationProperty in destinationVariable.Type.FieldList.Array)
            {
                if (sourcePropertyDictionary.TryGetValue(destinationProperty.Name, out var sourceProperty))
                {
                    mappingList.Add(new(destinationVariable.Name + '.' + destinationProperty.Name, sourceVariable.Name + '.' + sourceProperty.Name));
                }
            }
        }

        return [.. mappingList];
    }

    #endregion

    #region MethodImplementationByBaseMethod

    public static MethodImplementation[] CreateMethodImplementationByBaseMethod(MethodWithSettings method, MethodWithSettings[] methodList)
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

            baseMethod = new MethodWithSettings(method.Name, method.ReturnType, new([sourceVariable, destinationVariable]), method.SettingsStorage);

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
