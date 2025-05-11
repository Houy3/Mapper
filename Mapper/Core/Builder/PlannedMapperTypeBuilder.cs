using Mapper.Core.Entity.Common;
using Mapper.Core.Entity;
using static Mapper.Core.Entity.MethodDetails;
using static Mapper.Core.Entity.MethodSource;

namespace Mapper.Core.Builder;

public static class PlannedMapperTypeBuilder
{
    public const string INTERNAL_METHOD_NAME_PREFIX = "Internal";


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
}
