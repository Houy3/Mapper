using Mapper.Core.Entity.Common;
using Mapper.Core.Entity;
using static Mapper.Core.Entity.MethodDetails;
using static Mapper.Core.Entity.MethodSource;

namespace Mapper.Core.Builder;

public static class PlannedMapperTypeBuilder
{
    public const string STATIC_METHOD_NAME_SUFFIX = "Internal";
    public const string BUILDER_METHOD_NAME_SUFFIX = "Builder";
    public const string AFTER_MAPPING_METHOD_NAME_SUFFIX = "AfterMapping";


    #region Plan Methods

    public static PlannedMapperType Plan(this MapperType mapperType, CancellationToken ct)
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

        if (method.Is(Static))
            return [new(
                method.Signature, 
                method.Details, 
                FindBuilderMethod(method, methodList),
                FindAfterMappingMethod(method, methodList), 
                method.SettingOverrideList)];

        var staticMethod = ResolveStaticMappingMethod(method, methodList);

        var methodDetails = method.Details;
        if (method.IsNot(Partial) && method.IsFrom(Class))
            methodDetails |= Override;

        var mappingMethod = new MappingMethod(
                method.Signature,
                methodDetails,
                staticMethod.Signature,
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

    public static MappingMethod ResolveStaticMappingMethod(Method method, EquatableArrayWrap<Method> methodList)
    {
        var staticMethodSignature = StaticMethodSignature(method);

        var staticMethod = methodList.FirstOrDefault(x => x.Signature == staticMethodSignature && x.Is(Static));
        if (staticMethod is not null)
            return new(staticMethod.Signature, staticMethod.Details, staticMethod.SettingOverrideList);

        return new MappingMethod(
            staticMethodSignature,
            (method.Details & ~(Partial | Override)) | Static,
            FindBuilderMethod(method, methodList),
            FindAfterMappingMethod(method, methodList),
            method.SettingOverrideList);
    }

    public static MethodSignature StaticMethodSignature(Method method)
        => new(method.Name + STATIC_METHOD_NAME_SUFFIX, method.ReturnType, method.ParameterList);

    public static MethodSignature? FindBuilderMethod(Method method, EquatableArrayWrap<Method> methodList)
    {
        var builderMethodName = method.Name + BUILDER_METHOD_NAME_SUFFIX;
        return methodList.FirstOrDefault(x =>
               x.ReturnType == method.ReturnType
            && x.Name == builderMethodName
            && (x.ParameterList.Length == 0 || x.ParameterList.Length == 1 && x.SourceType == method.SourceType))?.Signature;
    }

    public static MethodSignature? FindAfterMappingMethod(Method method, EquatableArrayWrap<Method> methodList)
    {
        var afterMappingMethodName = method.Name + AFTER_MAPPING_METHOD_NAME_SUFFIX;
        return methodList.FirstOrDefault(x =>
               x.ReturnType == method.ReturnType
            && x.Name == afterMappingMethodName
            && (x.ParameterList.Length == 1 && x.SourceType == method.ReturnType
                || x.ParameterList.Length == 2 && x.SourceType == method.SourceType && x.ParameterList[1].Type == method.ReturnType))?.Signature;
    }

    #endregion
}
