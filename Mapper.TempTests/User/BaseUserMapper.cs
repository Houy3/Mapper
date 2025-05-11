using DumbFramework;
using Entity.User;

namespace Service.User;

public abstract class BaseUserMapper : SimpleBaseMapper<UserDto, UserService>
{
    public abstract UserDto ToDto(UserService source, UserDto destination);
}
