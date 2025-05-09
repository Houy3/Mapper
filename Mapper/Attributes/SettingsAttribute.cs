using Mapper.Common;
using Mapper.Core.Settings;

namespace Mapper.Attributes;

public static class SettingsAttribute
{
    public const string Namespace = Constants.Namespace;
    public const string Name = nameof(SettingsAttribute);

    public const string FullName = Namespace + "." + Name;

    public const string Text = $$"""
        namespace {{Namespace}};

        [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
        internal class {{Name}} : Attribute
        {
            public MappingRuleEnum {{MappingRulePropertyName}} = MappingRuleEnum.UseDefaultValue;
        }

        public enum MappingRuleEnum
        {
            UseDefaultValue,
        
            /// <summary>
            /// Мапить только по полям приемника. "Лишние" поля источника игнорируются.
            /// </summary>
            MapByDestination,

            /// <summary>
            /// Мапить только по полям источника. "Лишние" поля приемника игнорируются.
            /// </summary>
            MapBySource,

            /// <summary>
            /// Мапить только найденные пары полей. "Лишние" поля источника и приемника игнорируются.
            /// </summary>
            MapOnlyPairs
        }
        
        """;


    public const string MappingRulePropertyName = nameof(SettingsStorage.MappingRule);
}
