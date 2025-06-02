using Mapper.Core.Settings;

namespace Mapper.Attributes;

public class SettingsAttribute : GeneratedAttribute
{
    public override string Name => nameof(SettingsAttribute);

    public override string Text => $$"""
        namespace {{Namespace}};

        internal abstract class {{Name}} : Attribute
        {
            public MappingRuleEnum {{MappingRulePropertyName}} = MappingRuleEnum.MapByDestination;

            public bool {{NullableStructAutoResolvingPropertyName}} = true;
        }

        public enum MappingRuleEnum
        {
            UseDefaultValue,
        
            MapByDestination,

            MapBySource,

            MapOnlyPairs
        }   
        """;


    public const string MappingRulePropertyName = nameof(SettingsStorage.MappingRule);
    public const string NullableStructAutoResolvingPropertyName = nameof(SettingsStorage.NullableStructAutoResolving);
}
