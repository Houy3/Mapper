using DumbFramework;
using Entity.User;

namespace Mapper.TempTests.User;

public abstract class BaseUserMapper : SimpleBaseMapper<UserDto, UserDb>
{
    public abstract UserDto ToDto(UserDb source, UserDto destination);
}
