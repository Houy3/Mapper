using Entity.User;

namespace Service.User;

[Mapper.AutoImplementation]
internal partial class UserMapperFromStaticPartial
{
    public static partial UserService ToService(UserDto source);

    public static partial UserDto ToDto(UserService source);
}
