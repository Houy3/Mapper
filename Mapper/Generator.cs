using Mapper.Attributes;
using Mapper.Core.Settings;
using Mapper.Core.Entity;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using System.Text;
using Mapper.Core.Builder;
using Mapper.Core.TypeMapping;
using Mapper.Core.Entity.Common;
using System.Collections.Immutable;

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
        context.RegisterPostInitializationOutput(RegisterImportTypeMappingsAttribute);

        //попробовать искать поля по атрибуту


        //достаем настройки на проект
        var globalSettings = context.SyntaxProvider
            .ForAttributeWithMetadataName(
                fullyQualifiedMetadataName: GlobalSettingsAttribute.FullName,
                predicate: (node, _) => node is TypeDeclarationSyntax,
                transform: (ctx, _) => SettingsHelper.From(ctx.Attributes))
            .Where(x => x is not null)
            .Collect()
            .Select((x, _) => SettingsHelper.FirstOrDefaultSetting(x));

        //достаем интерфейсы для реализации
        var interfaceList = context.SyntaxProvider
            .ForAttributeWithMetadataName(
                fullyQualifiedMetadataName: AutoImplementationAttribute.FullName,
                predicate: (node, _) => node is InterfaceDeclarationSyntax,
                transform: (ctx, _) => InterfaceReader.From(ctx.TargetSymbol))
            .Where(x => x is not null)
            .Select((x, _) => x!);


        var outsideTypeMappingListStorage = context.SyntaxProvider
            .ForAttributeWithMetadataName(
                fullyQualifiedMetadataName: ImportTypeMappingsAttribute.FullName,
                predicate: (node, _) => node is TypeDeclarationSyntax,
                transform: (ctx, _) => TypeMappingHelper.From(ctx.Attributes))
            .Collect();


        var internalTypeMappingListStorage = interfaceList
            .Select((x, _) => TypeMappingHelper.From(x))
            .Collect();

        var typeMappingStorage = outsideTypeMappingListStorage.Combine(internalTypeMappingListStorage)
            .Select((x, _) => TypeMappingHelper.BuildStorage(x.Left, x.Right));


        context.RegisterSourceOutput(typeMappingStorage, RegisterTest);

        //высчитываем настройки на каждой реализации
        var interfaceWithSettingsList = interfaceList
            .Combine(globalSettings)
            .Select((x, ct) => SettingsHelper.SpreadOutSettings(x.Left, x.Right, ct));


        //collect importsSettings


        var implementationList = interfaceWithSettingsList.Select((x, ct) => x.Implement());

        context.RegisterSourceOutput(implementationList, RegisterMapper);
    }


    private void RegisterAutoImplementationAttribute(IncrementalGeneratorPostInitializationContext context)
        => context.AddSource(AutoImplementationAttribute.FullName, SourceText.From(AutoImplementationAttribute.Text, Encoding.UTF8));

    private void RegisterGlobalSettingsAttribute(IncrementalGeneratorPostInitializationContext context)
        => context.AddSource(GlobalSettingsAttribute.FullName, SourceText.From(GlobalSettingsAttribute.Text, Encoding.UTF8));

    private void RegisterSettingsAttribute(IncrementalGeneratorPostInitializationContext context)
        => context.AddSource(SettingsAttribute.FullName, SourceText.From(SettingsAttribute.Text, Encoding.UTF8));

    private void RegisterImportTypeMappingsAttribute(IncrementalGeneratorPostInitializationContext context)
        => context.AddSource(ImportTypeMappingsAttribute.FullName, SourceText.From(ImportTypeMappingsAttribute.Text, Encoding.UTF8));



    private static void RegisterMapper(SourceProductionContext context, Implementation implementationInfo)
        => context.AddSource(implementationInfo.FullName, SourceText.From(CodeBuilder.Build(implementationInfo), Encoding.UTF8));

    private static void RegisterTest(SourceProductionContext context, TypeMappingStorage test)
    {
        //context.AddSource("ttt.txt", SourceText.From("ttt", Encoding.UTF8));
    }

}
