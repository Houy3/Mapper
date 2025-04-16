using Entity.User;

namespace Service.User;

[Mapper.AutoImplementation]
interface IUserMapper
{
    UserDb ToDb(UserDto source);

    UserDb ToDb(UserDto source, UserDb destination);


    UserDto ToDto(UserDb source);

    UserDto ToDto(UserDb source, UserDto destination);

}
