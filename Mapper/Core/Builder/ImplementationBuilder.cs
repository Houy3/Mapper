using Mapper.Core.Entity;
using Mapper.Core.Entity.Common;
using Mapper.Core.Reader;
using Mapper.Core.Settings;
using static Mapper.Core.Entity.MethodDetails;
using static Mapper.Core.Entity.MethodSource;

namespace Mapper.Core.Builder;

public static class ImplementationBuilder
{
    public const string INTERNAL_METHOD_NAME_PREFIX = "Internal";


    #region Plan Methods

    public static PlannedMapperType PlanMethodList(this MapperType mapperType, CancellationToken ct)
        => new(
            mapperType.Namespace,
            mapperType.Name,
            PlanMethodList(mapperType.MethodList, ct),
            mapperType.SettingOverrideList);

    public static EquatableArrayWrap<MappingMethod> PlanMethodList(EquatableArrayWrap<Method> methodList, CancellationToken ct)
        => new(methodList.SelectMany(x => PlanMethod(x, methodList, ct)));

    public static MappingMethod[] PlanMethod(Method method, EquatableArrayWrap<Method> methodList, CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();

        if (HasBodyOrImplementation(method, methodList))
            return [];

        var staticMethod = ResolveStaticMappingMethod(method, methodList);

        var methodDetails = method.Details;
        if (method.IsNot(Partial) && method.IsFrom(Class))
            methodDetails |= Override;

        var mappingMethod = new MappingMethod(
            method.Signature,
            methodDetails,
            staticMethod,
            method.SettingOverrideList);

        if (staticMethod?.Is(WithoutBody) ?? false)
            return [mappingMethod, staticMethod];

        return [mappingMethod];
    }

    public static bool HasBodyOrImplementation(Method method, EquatableArrayWrap<Method> methodList)
    {
        if (!method.Is(WithoutBody))
            return true;

        var possibleImplementationList = methodList.Where(x => x.Signature == method.Signature && x.IsNot(Static));

        if (method.IsFrom(Interface))
            return possibleImplementationList.Where(x => x.IsFrom(Class)).Any();

        if (method.Is(Partial))
            return false;

        return possibleImplementationList.Where(x => x.Is(Override)).Any();
    }

    public static MappingMethod? ResolveStaticMappingMethod(Method method, EquatableArrayWrap<Method> methodList)
    {
        if (method.Is(Static))
            return null;

        var staticMethodSignature = StaticMethodSignature(method);

        var staticMethod = methodList.FirstOrDefault(x => x.Signature == staticMethodSignature && x.Is(Static));
        if (staticMethod is not null)
            return new(staticMethod.Signature, staticMethod.Details, staticMethod.SettingOverrideList);

        return new MappingMethod(
            staticMethodSignature, 
            (method.Details & ~(Partial | Override)) | Static, 
            method.SettingOverrideList);
    }

    public static MethodSignature StaticMethodSignature(Method method)
        => new(method.Name + INTERNAL_METHOD_NAME_PREFIX, method.ReturnType, method.ParameterList);

    #endregion

    public static ImplementedMapperType ImplementType(this ConfiguredMapperType mapperType)
        => new(
            mapperType.Namespace,
            mapperType.Name,
            ImplementMethodList(mapperType.ConfiguredMethodList));

    public static EquatableArrayWrap<MethodImplementation> ImplementMethodList(EquatableArrayWrap<ConfiguredMethod> methodList)
        => new(methodList.Select(ImplementMethod));


    public static MethodImplementation ImplementMethod(
        ConfiguredMethod method)
    {
        if (method.ConnectedMethod is not null)
            return ImplementByOtherMethod(method);
        return ImplementMappingMethod(method);
    }

    #region MethodImplementationByMappingList

    public static MappingMethodImplementation ImplementMappingMethod(ConfiguredMethod method)
        => new(method.Signature, method.Details, MapFieldList(new(method.ParameterList[0].Name, method.SourceType), method.DestinationType, method.SettingsStorage));

    public static EquatableArrayWrap<FieldMapping> MapFieldList(Variable sourceVariable, DataType destinationType, SettingsStorage settings)
    {
        var mappingList = new List<FieldMapping>();

        //if (settings.MappingRule == MappingRuleEnum.MapByDestination)
        //{
        var sourcePropertyDictionary = sourceVariable.Type.FieldList.ToDictionary(x => x.Name, x => x);
        foreach (var destinationProperty in destinationType.FieldList)
        {
            var sourceProperty = sourcePropertyDictionary.GetValueOrDefault(destinationProperty.Name);
            mappingList.Add(new FieldMapping(sourceVariable, sourceProperty, destinationProperty.Name));
        }
        //}

        //if (settings.MappingRule == MappingRuleEnum.MapBySource)
        //{
        //    var destinationPropertyDictionary = destinationVariable.Type.FieldList.Array.ToDictionary(x => x.Name, x => x);
        //    foreach (var sourceProperty in sourceVariable.Type.FieldList.Array)
        //    {
        //        if (destinationPropertyDictionary.TryGetValue(sourceProperty.Name, out var destinationProperty))
        //        {
        //            mappingList.Add(new(destinationVariable.Name + '.' + sourceProperty.Name, sourceVariable.Name + '.' + sourceProperty.Name));
        //        }
        //        else
        //        {
        //            mappingList.Add(new(null, sourceVariable.Name + '.' + sourceProperty.Name));
        //        }
        //    }
        //}

        //if (settings.MappingRule == MappingRuleEnum.MapOnlyPairs)
        //{
        //    var sourcePropertyDictionary = sourceVariable.Type.FieldList.Array.ToDictionary(x => x.Name, x => x);
        //    foreach (var destinationProperty in destinationVariable.Type.FieldList.Array)
        //    {
        //        if (sourcePropertyDictionary.TryGetValue(destinationProperty.Name, out var sourceProperty))
        //        {
        //            mappingList.Add(new(destinationVariable.Name + '.' + destinationProperty.Name, sourceVariable.Name + '.' + sourceProperty.Name));
        //        }
        //    }
        //}

        return new(mappingList);
    }

    #endregion

    public static MethodImplementationByOtherMethod ImplementByOtherMethod(ConfiguredMethod method)
        => new(method.Signature, method.Details, method.ConnectedMethod!.Signature);
    
}
