using Mapper.Attributes;
using Mapper.Core.Reader;
using Mapper.Core.Entity;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using System.Text;
using Mapper.Core.Builder;

namespace Mapper;

//nullable
//array
//where for method
//where for propertyList

[Generator]
public class Generator : IIncrementalGenerator
{
    public void Initialize(IncrementalGeneratorInitializationContext context)
    {
        context.RegisterPostInitializationOutput(RegisterAutoImplementationAttribute);


        var interfaceList = context.SyntaxProvider
            .ForAttributeWithMetadataName(
                fullyQualifiedMetadataName: AutoImplementationAttributeInfo.FullName,
                predicate: (node, _) => node is InterfaceDeclarationSyntax,
                transform: (ctx, _) => InterfaceReader.From(ctx.TargetSymbol))
            .Where(x => x is not null)
            .Select((x, ct) => x!);

        //get settings
        //collect to global base

        var implementationList = interfaceList.Select((x, ct) => x.Implement());

        context.RegisterSourceOutput(implementationList, WriteEntitySource);
    }


    private void RegisterAutoImplementationAttribute(IncrementalGeneratorPostInitializationContext context)
        => context.AddSource(AutoImplementationAttributeInfo.FullName, SourceText.From(AutoImplementationAttributeInfo.Text, Encoding.UTF8));
    

    private static void WriteEntitySource(
        SourceProductionContext context,
        Implementation? implementationInfo)
    {
        if (implementationInfo is not null)
            context.AddSource(implementationInfo.FullName, SourceText.From(CodeBuilder.Build(implementationInfo), Encoding.UTF8));
    }
}
