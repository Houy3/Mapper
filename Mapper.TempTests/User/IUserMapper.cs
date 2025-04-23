using Entity.User;


namespace Service.User;

[Mapper.AutoImplementation]
//[Mapper.Settings(MappingRule = Mapper.MappingRuleEnum.MapOnlyPairs)]
partial interface IUserMapper
{

    UserDb ToDb(UserDto source);

    [Mapper.Settings(MappingRule = Mapper.MappingRuleEnum.MapOnlyPairs)]
    UserDb ToDb(UserDto source, UserDb destination);


    UserDto ToDto(UserDb source);


    [Mapper.Settings(MappingRule = Mapper.MappingRuleEnum.MapByDestination)]
    UserDto ToDto(UserDb source, UserDto destination);

}



