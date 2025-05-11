using Mapper.Core.Entity;
using Mapper.Core.Entity.Common;
using Mapper.Core.Settings;

namespace Mapper.Core.Builder;

public static class ImplementationBuilder
{
    public static ImplementedMapperType Implement(this ConfiguredMapperType mapperType)
        => new(
            mapperType.Namespace,
            mapperType.Name,
            ImplementMethodList(mapperType.ConfiguredMethodList));

    public static EquatableArrayWrap<MethodImplementation> ImplementMethodList(EquatableArrayWrap<ConfiguredMethod> methodList)
        => new(methodList.Select(ImplementMethod));

    public static MethodImplementation ImplementMethod(ConfiguredMethod method)
    {
        if (method.ConnectedMethod is not null)
            return ImplementByOtherMethod(method);
        return ImplementMappingMethod(method);
    }

    public static MethodImplementationByOtherMethod ImplementByOtherMethod(ConfiguredMethod method)
        => new(method.Signature, method.Details, method.ConnectedMethod!.Signature);

    public static DataTypeMappingMethodImplementation ImplementMappingMethod(ConfiguredMethod method)
        => new(method.Signature, method.Details, MapFieldList(method.ParameterList[0].Name, method.SourceType, method.DestinationType, method.SettingsStorage));

    public static EquatableArrayWrap<FieldMapping> MapFieldList(
        string sourceParameterName,
        DataType sourceType,
        DataType destinationType, 
        SettingsStorage settings)
    {
        IEnumerable<string> fieldNameList = settings.MappingRule switch
        {
            MappingRule.BySource => sourceType.FieldList.Select(x => x.Name),
            MappingRule.ByDestination => destinationType.FieldList.Select(x => x.Name),
            MappingRule.OnlyPairs => sourceType.FieldList.Select(x => x.Name),
            _ => []
        };

        fieldNameList = fieldNameList.Where(x => !settings.IgnoreFieldList.Contains(x));

        var fieldMappingList = fieldNameList.Select(fieldName => MapField(
                sourceParameterName,
                sourceType.FieldDictionary.GetValueOrDefault(fieldName),
                destinationType.FieldDictionary.GetValueOrDefault(fieldName),
                settings));

        if (settings.MappingRule == MappingRule.OnlyPairs)
            fieldMappingList = fieldMappingList.Where(x => x.ErrorMessage is null);

        return new(fieldMappingList);
    }

    public static FieldMapping MapField(
        string sourceParameterName,
        Field? sourceField,
        Field? destinationField,
        SettingsStorage settings)
    {
        if (sourceField is null)
            return new FieldMapping(sourceParameterName, sourceField, destinationField, "No source found.");
        
        if (destinationField is null)
            return new FieldMapping(sourceParameterName, sourceField, destinationField, "No destination found.");

        if (settings.TypeMappingStorage.TypeMappingDictionary.TryGetValue(new(sourceField.Type.ToId(), destinationField.Type.ToId()), out var typeMappingMethodId))
            return new FieldMappingByMethod(sourceParameterName, sourceField, destinationField, typeMappingMethodId);
        
        return new FieldMapping(sourceParameterName, sourceField, destinationField);
    }

    
}
