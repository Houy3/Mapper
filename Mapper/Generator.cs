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

//find other functions
//ignore
//nullable
//array
//strange types: valueType enum
//async
//todo import settings not types

//read about cancelToken

[Generator]
public class Generator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        context.RegisterPostInitializationOutput(RegisterAutoImplementationAttribute);
        context.RegisterPostInitializationOutput(RegisterProjectSettingsAttribute);
        context.RegisterPostInitializationOutput(RegisterSettingsAttribute);
        context.RegisterPostInitializationOutput(RegisterImportTypeMappingsAttribute);

        //импортируем сторонние маппинги
        var externalTypeMappingList = context.SyntaxProvider
            .ForAttributeWithMetadataName(
                fullyQualifiedMetadataName: ImportTypeMappingsAttribute.FullName,
                predicate: (node, _) => node is ClassDeclarationSyntax,
                transform: (ctx, _) => ctx.Attributes.ReadExternalTypeMappingList())
            .SelectMany((x, _) => x)
            .Collect();

        //ищем мапперы и их методы для реализации
        var mapperTypeList = context.SyntaxProvider
            .ForAttributeWithMetadataName(
                fullyQualifiedMetadataName: AutoImplementationAttribute.FullName,
                predicate: (node, _) => node is ClassDeclarationSyntax,
                transform: (ctx, _) => ctx.TargetSymbol.ReadMapperType())
            .Where(x => x is not null)
            .Select((x, ct) => x!.Plan(ct));

        //ищем внутренние маппинги
        var internalTypeMappingList = mapperTypeList
            .SelectMany((x, _) => x.FindTypeMappingMethodList())
            .Collect();

        //объединяем маппинги в общее хранилище
        var typeMappingStorage = externalTypeMappingList.Combine(internalTypeMappingList)
            .Select((x, ct) => x.Left.CombineInStorage(x.Right, ct));

        //достаем настройки на проект
        var projectSettings = context.SyntaxProvider
            .ForAttributeWithMetadataName(
                fullyQualifiedMetadataName: ProjectSettingsAttribute.FullName,
                predicate: (node, _) => node is ClassDeclarationSyntax,
                transform: (ctx, _) => ctx.Attributes.ReadProjectSettings())
            .Where(x => x is not null)
            .Collect()
            .Combine(typeMappingStorage)
            .Select((x, _) => x.Left.FirstOrDefault(x.Right));

        //высчитываем настройки на каждой реализации
        var configuredMapperTypeList = mapperTypeList
            .Combine(projectSettings)
            .Select((x, ct) => x.Left.Configure(x.Right, ct));


        var implementationList = configuredMapperTypeList
            .Select((x, ct) => x.Implement());

        context.RegisterSourceOutput(implementationList, RegisterMapper);
    }


    private void RegisterAutoImplementationAttribute(IncrementalGeneratorPostInitializationContext context)
        => context.AddSource(AutoImplementationAttribute.FullName, SourceText.From(AutoImplementationAttribute.Text, Encoding.UTF8));

    private void RegisterProjectSettingsAttribute(IncrementalGeneratorPostInitializationContext context)
        => context.AddSource(ProjectSettingsAttribute.FullName, SourceText.From(ProjectSettingsAttribute.Text, Encoding.UTF8));

    private void RegisterSettingsAttribute(IncrementalGeneratorPostInitializationContext context)
        => context.AddSource(SettingsAttribute.FullName, SourceText.From(SettingsAttribute.Text, Encoding.UTF8));

    private void RegisterImportTypeMappingsAttribute(IncrementalGeneratorPostInitializationContext context)
        => context.AddSource(ImportTypeMappingsAttribute.FullName, SourceText.From(ImportTypeMappingsAttribute.Text, Encoding.UTF8));



    private static void RegisterMapper(SourceProductionContext context, ImplementedMapperType implementation)
        => context.AddSource(implementation.FullName, SourceText.From(CodeBuilder.Build(implementation), Encoding.UTF8));

}
