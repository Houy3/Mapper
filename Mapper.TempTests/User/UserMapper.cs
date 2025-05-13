using Entity.User;

namespace Service.User;

[Mapper.AutoImplementation]
public partial class UserMapper //: BaseUserMapper, IUserMapper
{

    //public static partial UserService ToServiceWithoutBuilder(UserDto source);

    //public static partial UserService ToService(UserDto source);

    //public static partial UserService ToService(UserDto source, UserService destination);


    //public static partial UserService ToService(UserDto source);

    public static partial IEnumerable<UserService> ToServiceList1(IEnumerable<UserDto> source);
    public static partial IEnumerable<UserService> ToServiceList2(List<UserDto> source);
    public static partial IEnumerable<UserService> ToServiceList3(UserDto[] source);


    public static UserService ToServiceBuilder(UserDto source)
    {
        //...
        return new();
    }

    public static UserService ToServiceAfterMapping(UserDto source, UserService destination)
    {
        //...
        return destination;
    }

    ////from interface
    //public partial UserService ToService(UserDto source);
    ////from partial
    //[Mapper.MethodSettings(
    //    IgnoreFieldList = [nameof(UserService.Id), nameof(UserService.Role)])]
    //public partial UserService ToServiceModified(UserCreateDto source);

    //[Mapper.MethodSettings(
    //    IgnoreFieldList = [nameof(UserService.Id), nameof(UserService.Role)])]
    //public static partial UserService ToService(UserCreateDto source, UserService destination);


    ////from base class
    //public override partial UserDto ToDto(UserService source);
}
