using Mapper.Core.Entity;
using Mapper.Core.Entity.Common;

namespace Mapper.Core.TypeMapping;

public record TypeMappingStorage(
    EquatableDictionaryWrap<TypeIdPair, TypeMappingMethodId> TypeMappingList);

public record TypeMappingMethodId(
    string TypeNamespace,
    string TypeName,
    string Name);

public record TypeIdPair(
    TypeId Source,
    TypeId Destination);


public record TypeMappingMethod(
    TypeIdPair Key,
    TypeMappingMethodId Value);