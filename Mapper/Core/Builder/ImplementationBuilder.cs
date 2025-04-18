using Mapper.Core.Entity;
using Mapper.Core.Entity.Common;

namespace Mapper.Core.Builder;

public static class ImplementationBuilder
{
    public static Implementation Implement(this Interface @interface)
        => new(
            @interface.Namespace,
            GetImplementationName(@interface.Name),
            @interface.Name,
            ImplementList(@interface.MethodList));


    public static EquatableArrayWrap<MethodImplementationByMappingList> ImplementList(EquatableArrayWrap<Method> methodList)
    {


        var implementationList = new List<MethodImplementationByMappingList>(methodList.Array.Length);
        foreach (var method in methodList.Array)
        {
            Variable sourceVariable;
            Variable destinationVariable;
            bool isDestinationVariableInited;

            var methodParameterCount = method.ParameterList.Array.Length;
            if (methodParameterCount > 2)
                continue;
            else if (methodParameterCount == 2)
            {
                var sourceParameter = method.ParameterList.Array[0];
                sourceVariable = new Variable(sourceParameter.Name, sourceParameter.Type);

                var destinationParameter = method.ParameterList.Array[1];
                if (destinationParameter.Type != method.ReturnType)
                    continue;

                destinationVariable = new Variable(destinationParameter.Name, destinationParameter.Type);
                isDestinationVariableInited = true;
            }
            else
            {
                var parameter = method.ParameterList.Array[0];
                sourceVariable = new Variable(parameter.Name, parameter.Type);
                destinationVariable = new Variable("destination", method.ReturnType);
                isDestinationVariableInited = false;
            }

            var mappingList = new List<FieldMapping>();

            var sourcePropertyDictionary = sourceVariable.Type.FieldList.Array.ToDictionary(x => x.Name, x => x);
            foreach (var destinationProperty in destinationVariable.Type.FieldList.Array)
            {
                if (sourcePropertyDictionary.TryGetValue(destinationProperty.Name, out var sourceProperty))
                {
                    mappingList.Add(new(destinationVariable.Name + '.' + destinationProperty.Name, sourceVariable.Name + '.' + sourceProperty.Name));
                }
            }

            implementationList.Add(new(
                method.Name, 
                method.ReturnType, 
                method.ParameterList, 
                destinationVariable, 
                isDestinationVariableInited, 
                new([.. mappingList])));

        }

        return new([.. implementationList]);
    }

    public static string GetImplementationName(string interfaceName)
        => interfaceName.StartsWith("I") ? interfaceName.Substring(1) : interfaceName + "Impl";
}
