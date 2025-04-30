using Entity.User;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace Service.User;

public partial class UserMapper //: IUserMapper
{
    public partial UserDb? ToDb(UserDto? source);

    public partial UserDb ToDb(UserDto source);

    public partial UserDb ToDb(UserDto source, UserDb destination);


}

public partial class UserMapper //: IUserMapper
{
    //public UserDb ToDb(UserDto source)
    //{
    //    //throw new NotImplementedException();


    //    //если соседний метод существует
    //    return ((IUserMapper) this).ToDb(source, new());

    [return: NotNullIfNotNull(nameof(source))]
    public partial UserDb? ToDb(UserDto? source)
    {
        if (source is null)
            return null;
        return ToDbInternal(source);
    }

    public partial UserDb ToDb(UserDto source)
    {
        return ToDbInternal(source);
    }

    public partial UserDb ToDb(UserDto source, UserDb destination)
    {
        return ToDbInternal(source, destination);
    }



    ///////////////////////////////////////////////////////////////////////////////////////
    public static UserDb ToDbInternal(UserDto source)
        => ToDbInternal(source, NewUserDb());
    ///////////////////////////////////////////////////////////////////////////////////////

    public static UserDb ToDbInternal(UserDto source, UserDb destination)
    {
        //destination ??= NewUserDb();
        ToDbBeforeMapping(source, destination);
        destination.Id = source.Id ?? default;
        destination.Number = source.Number ?? default;
        return ToDbAfterMapping(source, destination);
    }


    //+-
    public static UserDb NewUserDb()
        => new Entity.User.UserDb();
    public static void ToDbBeforeMapping(UserDto source, UserDb destination)
    {
    }
    //+-
    public static UserDb ToDbAfterMapping(UserDto source, UserDb destination)
    {
        return destination;
    }

    //    //если соседний метод не существует
    //    var destination = new UserDb();
    //    //mapping
    //    return destination;
    //}

    //public UserDb ToDb(UserDto source, UserDb destination)
    //{
    //    //mapping
    //    throw new NotImplementedException();
    //}

    //public UserDto ToDto(UserDb source)
    //{
    //    throw new NotImplementedException();
    //}

    //public UserDto ToDto(UserDb source, UserDto destination)
    //{
    //    throw new NotImplementedException();
    //}
}
