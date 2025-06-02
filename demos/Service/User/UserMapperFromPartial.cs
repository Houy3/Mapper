using Entity.User;

namespace Service.User;

[Mapper.AutoImplementation]
internal partial class UserMapperFromPartial
{
    public partial UserService ToService(UserDto source);

    public partial UserDto ToDto(UserService source);
}
