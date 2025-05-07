using Mapper.Core.Entity;
using Mapper.Core.Entity.Common;
using Mapper.Core.Settings;

namespace Mapper.Core.Builder;

public static class ImplementationBuilder
{
    public static PlannedMapperType PlanMethods(this MapperType mapperType)
        => new(
            mapperType.Namespace,
            mapperType.Name,
            PlanMethods(mapperType.MethodList.Array));

    public static EquatableArrayWrap<PlannedMethod> PlanMethods(Method[] methodList)
    {
        //отобрать для реализации
        //внести запланированные статические методы
        var newMethodList = new List<PlannedMethod>();
        foreach (var method in methodList)
        {
            if (!method.Details.HasFlag(MethodDetails.WithoutBody))
            {
                newMethodList.Add(new(
                    method.Signature,
                    method.Details,
                    method.Source,
                    method.SettingOverrideList,
                    true,
                    null
                    ));
                continue;
            }


        }

        return new([.. newMethodList]);
    }

    public static PlannedMethod NewPlannedMethod(Method method, bool isReal, PlannedMethod connectedMethod)
        => new(method.Signature, method.Details, method.Source, method.SettingOverrideList, isReal, connectedMethod);

    public static ImplementedMapperType Implement(this ConfiguredMapperType mapperType)
        => new(
            mapperType.Namespace,
            GetImplementationName(mapperType.Name),
            ImplementList(mapperType.ConfiguredMethodList.Array.ToList()));


    public static EquatableArrayWrap<MethodImplementation> ImplementList(List<ConfiguredMethod> methodList)
    {
        var implementationList = new List<MethodImplementation>();

        foreach (var method in methodList.Where(x => x.Details.HasFlag(MethodDetails.WithoutBody)))
        {
            var overrideMethod = 

            Implement(method, methodList);
        }

            //foreach (var method in methodList)
            //implementationList.AddRange(CreateMethodImplementation(method, methodList));

        return new([.. implementationList]);
    }

    public static ConfiguredMethod? FindImplementationMethod(
        ConfiguredMethod baseMethod,
        List<ConfiguredMethod> methodList)
    {
        var fullMethodList = methodList.Select(x => x as MethodSignature).Concat(implementationList);



        return fullMethodList.FirstOrDefault(x => 
            && x.ReturnType == baseMethod.ReturnType
            && x.Name == baseMethod.Name
            && x.ParameterList == baseMethod.ParameterList);

        if (baseMethod.Source == MethodSource.Interface)
            return methodList.FirstOrDefault(x =>
                   !x.Details.HasFlag(MethodDetails.WithoutBody)
                && x.ReturnType == baseMethod.ReturnType
                && x.Name == baseMethod.Name
                && x.ParameterList == baseMethod.ParameterList);

        return null;
    }

    public static IEnumerable<MethodImplementation> Implement(
        ConfiguredMethod method, 
        ConfiguredMethod[] methodList,
        List<MethodImplementation> implementationList)
    {
        return [];
    }

    public static MethodImplementation[] CreateMethodImplementation(ConfiguredMethod method, ConfiguredMethod[] methodList)
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

    public static MethodImplementation[] CreateMethodImplementationByMappingList(ConfiguredMethod method)
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

    public static MappingMethodImplementation CreateMethodImplementationByMappingList(ConfiguredMethod method, Variable sourceVariable, Variable destinationVariable)
        => new(
            method.Details,
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

    public static MethodImplementation[] CreateMethodImplementationByBaseMethod(ConfiguredMethod method, ConfiguredMethod[] methodList)
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


        ////создаем базовый метод с реализацией, если он не нашелся
        //MethodImplementationByOtherMethod? baseMethodImplementation = null;
        //if (baseMethod is null)
        //{
        //    var sourceVariable = new Variable("source", sourceType);
        //    var destinationVariable = new Variable("destination", destinationType);

        //    baseMethod = new ConfiguredMethodSignature(method.Name, method.ReturnType, new([sourceVariable, destinationVariable]), method.SettingsStorage);

        //    baseMethodImplementation = CreateMethodImplementationByMappingList(baseMethod, sourceVariable, destinationVariable);
        //}

        //var methodImplementation = new MethodImplementationByOtherMethod(
        //    method.Name,
        //    method.ReturnType,
        //    method.ParameterList,
        //    baseMethod!.Name);

        //if (baseMethodImplementation is not null)
        //    return [methodImplementation, baseMethodImplementation];
        //else
        //    return [methodImplementation];

        return [];
    }

    #endregion


    public static string GetImplementationName(string interfaceName)
        => interfaceName.StartsWith("I") ? interfaceName.Substring(1) : interfaceName + "Impl";
}
