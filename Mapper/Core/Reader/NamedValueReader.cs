﻿using Mapper.Core.Entity;
using Microsoft.CodeAnalysis;

namespace Mapper.Core.Reader;

public static class NamedValueReader
{
    public static IEnumerable<NamedValue> From(AttributeData? settingsAttribute)
    {
        if (settingsAttribute is null)
            return [];

        return settingsAttribute.NamedArguments.Select(From).Where(x => x is not null)!;
    }

    public static NamedValue? From(KeyValuePair<string, TypedConstant> namedConstant)
    {
        var value = From(namedConstant.Value);
        if (value is null)
            return null;

        return new(namedConstant.Key, value);
    }


    public static object? From(TypedConstant constant)
        => constant.Kind switch
        {
            TypedConstantKind.Primitive => NullIfNegative(constant.Value),
            TypedConstantKind.Enum => NullIfNegative((int)constant.Value! - 1),
            TypedConstantKind.Type => constant.Value,
            TypedConstantKind.Array => constant.Values.Select(From).ToArray(),
            _ => null
        };

    public static object? NullIfNegative(object? value)
        => value is int valueInt ? NullIfNegative(valueInt) : value;

    public static int? NullIfNegative(int @int)
        => @int >= 0 ? @int : null;
}
