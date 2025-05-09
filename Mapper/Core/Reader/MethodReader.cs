using Mapper.Core.Entity;
using Mapper.Core.Entity.Common;
using Mapper.Core.Settings;
using Microsoft.CodeAnalysis;

namespace Mapper.Core.Reader;

public static class MethodReader
{
    //todo
    //public static readonly string[] IgnoreMethodNameList = [nameof(Equals), nameof(GetHashCode)];


    public static EquatableArrayWrap<Method> From(ITypeSymbol symbol)
        => new(FromClassRecursive(symbol).Concat(FromInterfaceList(symbol)));


    public static IEnumerable<Method> FromClassRecursive(ITypeSymbol symbol)
    {
        var isBase = false;

        while (symbol.BaseType is not null)
        {
            foreach (var methodSymbol in symbol.GetMembers().OfType<IMethodSymbol>().Where(x => !isBase || !x.IsPrivate()))
            {
                var method = From(methodSymbol, MethodSource.Class);
                if (method is not null)
                    yield return method;
            }

            symbol = symbol.BaseType;
            isBase = true;
        }
    }

    public static IEnumerable<Method> FromInterfaceList(ITypeSymbol symbol)
    {
        foreach (var methodSymbol in symbol.AllInterfaces.SelectMany(x => x.GetMembers()).OfType<IMethodSymbol>().Where(x => !x.IsPrivate()))
        {
            var method = From(methodSymbol, MethodSource.Interface);
            if (method is not null)
                yield return method;
        }
    }


    public static Method? From(IMethodSymbol symbol, MethodSource source)
    {
        if (symbol.ReturnsVoid)
            return null;

        var returnType = DataTypeReader.From(symbol.ReturnType);
        var parameterList = VariableReader.From(symbol.Parameters);

        if (   parameterList.Length == 0 || parameterList.Length > 2
            || parameterList.Length == 2 && parameterList[1].Type != returnType)
            return null;

        return new(
            new(symbol.Name, returnType, new(parameterList)),
            DetailsFrom(symbol),
            source,
            SettingOverrideReader.From(symbol));
    }

    public static MethodDetails DetailsFrom(IMethodSymbol symbol)
    {
        var type = MethodDetails.None;

        if (symbol.IsPublic())
            type |= MethodDetails.Public;

        if (symbol.IsStatic)
            type |= MethodDetails.Static;

        if (symbol.IsPartialDefinition)
        {
            type |= MethodDetails.Partial;
            if (symbol.PartialImplementationPart is null)
                type |= MethodDetails.WithoutBody;
        }

        if (symbol.IsAbstract)
            type |= MethodDetails.WithoutBody;

        if (symbol.IsOverride)
            type |= MethodDetails.Override;

        return type;
    }

    
}