using Mapper.Attributes;
using Mapper.Core.Settings;
using Mapper.Core.Entity;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using System.Text;
using Mapper.Core.Builder;

namespace Mapper;

//nullable
//array
//inner mapping
//base type
//where for method (2 params and other)
//where for propertyList (fieldList, getter, sett enable)

//read about cancelToken

[Generator]
public class Generator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        context.RegisterPostInitializationOutput(RegisterAutoImplementationAttribute);
        context.RegisterPostInitializationOutput(RegisterGlobalSettingsAttribute);
        context.RegisterPostInitializationOutput(RegisterSettingsAttribute);


        //попробовать искать поля по атрибуту
        var globalSettings = context.SyntaxProvider
            .ForAttributeWithMetadataName(
                fullyQualifiedMetadataName: GlobalSettingsAttribute.FullName,
                predicate: (node, _) => node is TypeDeclarationSyntax,
                transform: (ctx, _) => SettingsHelper.From(ctx.TargetSymbol))
            .Where(x => x is not null)
            .Collect()
            .Select(SettingsHelper.FirstOrDefaultSetting);

        var interfaceList = context.SyntaxProvider
            .ForAttributeWithMetadataName(
                fullyQualifiedMetadataName: AutoImplementationAttribute.FullName,
                predicate: (node, _) => node is InterfaceDeclarationSyntax,
                transform: (ctx, _) => InterfaceReader.From(ctx.TargetSymbol))
            .Where(x => x is not null)
            .Select((x, _) => x!);

        var interfaceWithSettingsList = interfaceList
            .Combine(globalSettings)
            .Select((x, _) => SettingsHelper.SpreadOutSettings(x.Left, x.Right));


        //get settings
        //collect othe impls
        //collect to global base


        var implementationList = interfaceWithSettingsList.Select((x, ct) => x.Implement());

        context.RegisterSourceOutput(implementationList, RegisterMapper);
    }


    private void RegisterAutoImplementationAttribute(IncrementalGeneratorPostInitializationContext context)
        => context.AddSource(AutoImplementationAttribute.FullName, SourceText.From(AutoImplementationAttribute.Text, Encoding.UTF8));

    private void RegisterGlobalSettingsAttribute(IncrementalGeneratorPostInitializationContext context)
        => context.AddSource(GlobalSettingsAttribute.FullName, SourceText.From(GlobalSettingsAttribute.Text, Encoding.UTF8));

    private void RegisterSettingsAttribute(IncrementalGeneratorPostInitializationContext context)
        => context.AddSource(SettingsAttribute.FullName, SourceText.From(SettingsAttribute.Text, Encoding.UTF8));



    private static void RegisterMapper(SourceProductionContext context, Implementation implementationInfo)
        => context.AddSource(implementationInfo.FullName, SourceText.From(CodeBuilder.Build(implementationInfo), Encoding.UTF8));

}
