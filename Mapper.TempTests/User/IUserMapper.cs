using DumbFramework;
using Entity.User;

namespace Service.User;

partial interface IUserMapper : ISimpleMapper<UserDto, UserService>
{

    public UserService ToDb(UserDto source, UserService destination);
}



