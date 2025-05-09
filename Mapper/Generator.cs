using Mapper.Attributes;
using Mapper.Core.Settings;
using Mapper.Core.Entity;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using System.Text;
using Mapper.Core.Builder;
using Mapper.Core.Reader;

namespace Mapper;

//nullable
//array
//strange types: valueType enum
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
                fullyQualifiedMetadataName: ProjectSettingsAttribute.FullName,
                predicate: (node, _) => node is ClassDeclarationSyntax,
                transform: (ctx, _) => ctx.Attributes.ReadProjectSettings())
            .Where(x => x is not null)
            .Collect()
            .Select((x, _) => x.FirstOrDefault());

        //импортируем сторонние маппинги
        var externalTypeMappingList = context.SyntaxProvider
            .ForAttributeWithMetadataName(
                fullyQualifiedMetadataName: ImportTypeMappingsAttribute.FullName,
                predicate: (node, _) => node is ClassDeclarationSyntax,
                transform: (ctx, _) => ctx.Attributes.ReadExternalTypeMappingList())
            .SelectMany((x, _) => x)
            .Collect();

        //ищем мапперы для реализации
        var mapperTypeList = context.SyntaxProvider
            .ForAttributeWithMetadataName(
                fullyQualifiedMetadataName: AutoImplementationAttribute.FullName,
                predicate: (node, _) => node is ClassDeclarationSyntax,
                transform: (ctx, _) => ctx.TargetSymbol.ReadMapperType())
            .Where(x => x is not null)
            .Select((x, ct) => x!.PlanMethodList(ct));

        //ищем внутренние маппинги
        var internalTypeMappingList = mapperTypeList
            .SelectMany((x, _) => x.FindTypeMappingMethodList())
            .Collect();

        //объединяем маппинги в общее хранилище
        var typeMappingStorage = externalTypeMappingList.Combine(internalTypeMappingList)
            .Select((x, ct) => x.Left.CombineInStorage(x.Right, ct));

        //высчитываем настройки на каждой реализации и распределяем хранилище
        var configuredMapperType = mapperTypeList
            .Combine(globalSettings.Combine(typeMappingStorage))
            .Select((x, ct) => SettingsHelper.SpreadOutSettings(x.Left, x.Right.Left, x.Right.Right, ct));


        var implementationList = configuredMapperType.Select((x, ct) => x.ImplementType());

        context.RegisterSourceOutput(implementationList, RegisterMapper);
    }


    private void RegisterAutoImplementationAttribute(IncrementalGeneratorPostInitializationContext context)
        => context.AddSource(AutoImplementationAttribute.FullName, SourceText.From(AutoImplementationAttribute.Text, Encoding.UTF8));

    private void RegisterGlobalSettingsAttribute(IncrementalGeneratorPostInitializationContext context)
        => context.AddSource(ProjectSettingsAttribute.FullName, SourceText.From(ProjectSettingsAttribute.Text, Encoding.UTF8));

    private void RegisterSettingsAttribute(IncrementalGeneratorPostInitializationContext context)
        => context.AddSource(SettingsAttribute.FullName, SourceText.From(SettingsAttribute.Text, Encoding.UTF8));

    private void RegisterImportTypeMappingsAttribute(IncrementalGeneratorPostInitializationContext context)
        => context.AddSource(ImportTypeMappingsAttribute.FullName, SourceText.From(ImportTypeMappingsAttribute.Text, Encoding.UTF8));



    private static void RegisterMapper(SourceProductionContext context, ImplementedMapperType implementation)
        => context.AddSource(implementation.FullName, SourceText.From(CodeBuilder.Build(implementation), Encoding.UTF8));

}
