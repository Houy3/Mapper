using Mapper.Attributes;
using Mapper.Core;
using Mapper.Core.Builder;
using Mapper.Core.Entity;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using System.Text;

namespace Mapper;

[Generator]
public class Generator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        context.RegisterPostInitializationOutput(RegisterAutoImplementationAttribute);

        var mappings = context.SyntaxProvider
            .ForAttributeWithMetadataName(
                fullyQualifiedMetadataName: AutoImplementationAttributeInfo.FullName,
                predicate: (node, _) => node is InterfaceDeclarationSyntax,
                transform: (ctx, _) => ImplementationTypeInfoBuilder.Build(ctx.TargetSymbol))
            .Where(static m => m != default);

        context.RegisterSourceOutput(mappings, WriteEntitySource);
    }


    private void RegisterAutoImplementationAttribute(IncrementalGeneratorPostInitializationContext context)
        => context.AddSource(AutoImplementationAttributeInfo.FullName, SourceText.From(AutoImplementationAttributeInfo.Text, Encoding.UTF8));
    

    private static void WriteEntitySource(
        SourceProductionContext context,
        ImplementationType implementationInfo)
    {
        context.AddSource(implementationInfo.FullName, SourceText.From(CodeBuilder.Build(implementationInfo), Encoding.UTF8));
    }
}
