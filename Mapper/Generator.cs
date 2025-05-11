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
        context.RegisterPostInitializationOutput(RegisterAttributes);

        //импортируем сторонние маппинги
        var externalTypeMappingList = context.SyntaxProvider
            .ForAttributeWithMetadataName(
                fullyQualifiedMetadataName: new ImportTypeMappingsAttribute().FullName,
                predicate: (node, _) => node is ClassDeclarationSyntax,
                transform: (ctx, _) => ctx.Attributes.ReadExternalTypeMappingList())
            .SelectMany((x, _) => x)
            .Collect();

        //ищем мапперы и их методы для реализации
        var mapperTypeList = context.SyntaxProvider
            .ForAttributeWithMetadataName(
                fullyQualifiedMetadataName: new AutoImplementationAttribute().FullName,
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
                fullyQualifiedMetadataName: new ProjectSettingsAttribute().FullName,
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

    public readonly GeneratedAttribute[] GeneratedAttributeList = [
        new SettingsAttribute(),
        new ProjectSettingsAttribute(),
        new AutoImplementationAttribute(),
        new MethodSettingsAttribute(),
        new ImportTypeMappingsAttribute(),
        ];

    private void RegisterAttributes(IncrementalGeneratorPostInitializationContext context)
    {
        foreach (var attribute in GeneratedAttributeList)
            context.AddSource(attribute.FullName, SourceText.From(attribute.Text, Encoding.UTF8));
    }

    private static void RegisterMapper(SourceProductionContext context, ImplementedMapperType implementation)
        => context.AddSource(implementation.FullName, SourceText.From(CodeBuilder.Build(implementation), Encoding.UTF8));

}
