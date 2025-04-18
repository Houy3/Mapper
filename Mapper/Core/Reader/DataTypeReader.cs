using Mapper.Core.Entity;
using Microsoft.CodeAnalysis;

namespace Mapper.Core.Reader;

public static class DataTypeReader
{
    public static DataType From(ITypeSymbol symbol)
        => new(
            GetNamespace(symbol),
            GetName(symbol),
            PropertyReader.From(symbol.GetMembers()));

    public static string GetName(ITypeSymbol symbol)
        => symbol.ToDisplayString(NullableFlowState.NotNull, new(typeQualificationStyle: SymbolDisplayTypeQualificationStyle.NameOnly));

    public static string GetNamespace(ITypeSymbol symbol)
        => symbol.ContainingNamespace.ToDisplayString(); 
}
