using Mapper.Core.Entity;
using Mapper.Core.Entity.Common;
using Mapper.Core.Settings;
using Microsoft.CodeAnalysis;

namespace Mapper.Core.Reader;

public static class MethodReader
{
    public static EquatableArrayWrap<Method> From(ITypeSymbol symbol)
        => new([.. FromOriginal(symbol), .. FromBaseType(symbol), .. FromInterfaceList(symbol)]);


    public static IEnumerable<Method> FromOriginal(ITypeSymbol symbol)
    {
        foreach (var methodSymbol in symbol.GetMembers().OfType<IMethodSymbol>())
        {
            var method = From(methodSymbol, MethodSource.Original);
            if (method is not null)
                yield return method;
        }
    }

    public static IEnumerable<Method> FromBaseType(ITypeSymbol symbol)
    {
        while (symbol.BaseType is not null)
        {
            symbol = symbol.BaseType;
            foreach (var methodSymbol in symbol.GetMembers().OfType<IMethodSymbol>())
            {
                var method = From(methodSymbol, MethodSource.BaseType);
                if (method is not null)
                    yield return method;
            }
        }

    }

    public static IEnumerable<Method> FromInterfaceList(ITypeSymbol symbol)
    {
        foreach (var methodSymbol in symbol.AllInterfaces.SelectMany(x => x.GetMembers()).OfType<IMethodSymbol>())
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

        if (parameterList.Length == 0
            || parameterList.Length == 2 && parameterList[1].Type != returnType
            || parameterList.Length > 1)
            return null;

        return new(
            new(symbol.Name, returnType, new(parameterList)),
            DetailFrom(symbol, source),
            source,
            SettingOverrideReader.From(symbol));
    }

    public static MethodDetails DetailFrom(IMethodSymbol symbol, MethodSource source)
    {
        var type = MethodDetails.None;

        if (symbol.IsPublic())
            type |= MethodDetails.Public;

        if (symbol.IsProtected())
            type |= MethodDetails.Protected;

        if (symbol.IsStatic)
            type |= MethodDetails.Static;

        if (symbol.IsPartialDefinition)
        {
            type |= MethodDetails.Partial;
            if (symbol.PartialDefinitionPart is null)
                type |= MethodDetails.WithoutBody;
        }

        if (symbol.IsAbstract)
            type |= MethodDetails.Abstract | MethodDetails.WithoutBody;

        if (symbol.IsOverride)
            type |= MethodDetails.Override;

        if (source == MethodSource.Interface)
            type |= MethodDetails.WithoutBody;

        return type;
    }

    
}