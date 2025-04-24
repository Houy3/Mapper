using Mapper.Core.Entity;
using Mapper.Core.Entity.Common;

namespace Mapper.Core.TypeMapping;

public record TypeMappingStorage(
    EquatableArrayWrap<TypeMappingMethod> TypeMappingList);//todo use dictionary

public record TypeMappingMethod(
    string TypeNamespace,
    string TypeName,
    string Name,
    TypeId Source,
    TypeId Destination);
