using Mapper.Core.Entity;
using Mapper.Core.Entity.Common;
using Microsoft.CodeAnalysis;

namespace Mapper.Core.Reader;

public static class FieldReader
{
    public static EquatableArrayWrap<Field> From(ITypeSymbol symbol)
    {
        var memberList = symbol.GetMembers();

        while (symbol.BaseType is not null)
        {
            memberList.Concat(symbol.BaseType.GetMembers());
            symbol = symbol.BaseType;
        }

        return new([.. memberList.Select(From).Where(x => x is not null)!]);
    }


    public static Field? From(ISymbol symbol)
    {
        if (symbol.IsStatic || !symbol.IsPublic())
            return null;

        if (symbol is IPropertySymbol propertySymbol)
        {
            if (propertySymbol.GetMethod is null || propertySymbol.SetMethod is null 
                || !propertySymbol.GetMethod.IsPublic() || !propertySymbol.SetMethod.IsPublic())
                return null;

            return new(symbol.Name, TypeFrom(propertySymbol.Type));
        }

        if (symbol is IFieldSymbol fieldSymbol)
        {
            if (fieldSymbol.IsReadOnly)
                return null;

            return new(symbol.Name, TypeFrom(fieldSymbol.Type));
        }

        return null;
    }
        

    public static FieldType TypeFrom(ITypeSymbol symbol)
        => new(TypeIdReader.GetNamespace(symbol), TypeIdReader.GetName(symbol));
}


