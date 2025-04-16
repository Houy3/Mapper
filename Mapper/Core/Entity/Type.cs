using Mapper.Core.Entity.Common;

namespace Mapper.Core.Entity;

public record Type(
    string Namespace,
    string Name,
    EquatableArrayWrap<Property> PropertyList);

public record PropertyType(
    string Namespace,
    string Name);
