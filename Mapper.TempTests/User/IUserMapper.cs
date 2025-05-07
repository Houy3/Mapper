using DumbFramework;
using Entity.User;

namespace Service.User;

partial interface IUserMapper : ISimpleMapper<UserDto, UserDb>
{

    public UserDb ToDb(UserDto source, UserDb destination);
    
}



