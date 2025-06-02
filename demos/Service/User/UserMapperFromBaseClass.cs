using DumbFramework;
using Entity.User;
using Mapper;

namespace Service.User;

[AutoImplementation]
internal partial class UserMapperFromBaseClass : SimpleBaseMapper<UserDto, UserService>;
