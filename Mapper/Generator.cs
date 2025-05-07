using Mapper.Attributes;
using Mapper.Core.Settings;
using Mapper.Core.Entity;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using System.Text;
using Mapper.Core.Builder;
using Mapper.Core.TypeMapping;
using Mapper.Core.Reader;

namespace Mapper;

//nullable
//array
//valueType
//async

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

        //достаем настройки на проект
        var globalSettings = context.SyntaxProvider
            .ForAttributeWithMetadataName(
                fullyQualifiedMetadataName: GlobalSettingsAttribute.FullName,
                predicate: (node, _) => node is ClassDeclarationSyntax,
                transform: (ctx, _) => SettingsHelper.From(ctx.Attributes))
            .Where(x => x is not null)
            .Collect()
            .Select((x, _) => SettingsHelper.FirstOrDefaultSetting(x));

        //достаем интерфейсы для реализации
        var interfaceList = context.SyntaxProvider
            .ForAttributeWithMetadataName(
                fullyQualifiedMetadataName: AutoImplementationAttribute.FullName,
                predicate: (node, _) => node is ClassDeclarationSyntax,
                transform: (ctx, _) => MapperTypeReader.From(ctx.TargetSymbol))
            .Where(x => x is not null)
            .Select((x, _) => x!);

        //импортируем сторонние маппинги
        var outsideTypeMappingList = context.SyntaxProvider
            .ForAttributeWithMetadataName(
                fullyQualifiedMetadataName: ImportTypeMappingsAttribute.FullName,
                predicate: (node, _) => node is ClassDeclarationSyntax,
                transform: (ctx, _) => TypeMappingHelper.From(ctx.Attributes))
            .Collect();
        
        //складываем маппинги, которые будут реализованы
        var insideTypeMappingList = interfaceList
            .Select((x, _) => TypeMappingHelper.From(x))
            .Collect();

        //объединяем маппинги в общее хранилище
        var typeMappingStorage = outsideTypeMappingList.Combine(insideTypeMappingList)
            .Select((x, ct) => TypeMappingHelper.BuildStorage(x.Left, x.Right, ct));

        //высчитываем настройки на каждой реализации
        var interfaceWithSettingsList = interfaceList
            .Combine(globalSettings.Combine(typeMappingStorage))
            .Select((x, ct) => SettingsHelper.SpreadOutSettings(x.Left, x.Right.Left, x.Right.Right, ct));


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



    private static void RegisterMapper(SourceProductionContext context, ImplementedMapperType implementationInfo)
        => context.AddSource(implementationInfo.FullName, SourceText.From(CodeBuilder.Build(implementationInfo), Encoding.UTF8));

}
