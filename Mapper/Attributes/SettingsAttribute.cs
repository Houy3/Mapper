using Mapper.Core.Settings;

namespace Mapper.Attributes;

public class SettingsAttribute : GeneratedAttribute
{
    public override string Name => nameof(SettingsAttribute);

    public override string Text => $$"""
        namespace {{Namespace}};

        internal abstract class {{Name}} : Attribute
        {
            public MappingRuleEnum {{MappingRulePropertyName}} = MappingRuleEnum.UseDefaultValue;
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
}
