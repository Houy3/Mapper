using DumbFramework;
using Entity.User;
using Mapper;

namespace Service.User;

[AutoImplementation]
public partial class UserMapperFromInterface : ISimpleMapper<UserDto, UserService>
{
    //переопределяем настройки для метода интерфейса/базового класса
    [MethodSettings(IgnoreFieldList = [nameof(UserDto.Number)])]
    public partial UserService ToService(UserDto source);

    //задаем дополнительный маппинг
    public static UserService ToServiceAfterMapping(UserDto source, UserService destination)
    {
        destination.Number = source.Number * 2;
        return destination;
    }


    //переопределяем настройки для метода интерфейса/базового класса
    [MethodSettings(IgnoreFieldList = [nameof(UserDto.Number)])]
    public partial UserDto ToDto(UserService source);

    //задаем конструктор для конечного типа
    public static UserDto ToDtoBuilder(UserService source)
        => new()
        {
            Number = source.Number * 2
        };
}
