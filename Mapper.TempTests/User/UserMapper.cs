using Entity.User;

namespace Service.User;

[Mapper.AutoImplementation]
public partial class UserMapper : BaseUserMapper, IUserMapper
{
    //from interface
    public partial UserService ToService(UserDto source);

    //from partial
    [Mapper.MethodSettings(
        IgnoreFieldList = [nameof(UserService.Id)])]
    public partial UserService ToService(UserCreateDto source);

    [Mapper.MethodSettings(
        IgnoreFieldList = [nameof(UserService.Id)])]
    public static partial UserService ToService(UserCreateDto source, UserService destination);


    //from base class
    public override partial UserDto ToDto(UserService source);
}
