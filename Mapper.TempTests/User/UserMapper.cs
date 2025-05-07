using Entity.User;
using Mapper.TempTests.User;
using System.Diagnostics.CodeAnalysis;

namespace Service.User;

[Mapper.AutoImplementation]
[Mapper.Settings(MappingRule = Mapper.MappingRuleEnum.MapOnlyPairs)]
public partial class UserMapper : BaseUserMapper, IUserMapper
{
    public partial UserDb ToDb(UserDto source);

    public UserDb ToDb(UserDto source, UserDb destination)
    {
        throw new NotImplementedException();
    }

    public partial UserDb ToDb2(UserDto source);

    public partial UserDb ToDb2(UserDto source, UserDb destination);

    public override partial UserDto ToDto(UserDb source);

    public override UserDto ToDto(UserDb source, UserDto destination)
    {
        throw new NotImplementedException();
    }
}

public partial class UserMapper 
{
    //public UserDb ToDb(UserDto source)
    //{
    //    //throw new NotImplementedException();


    //    //если соседний метод существует
    //    return ((IUserMapper) this).ToDb(source, new());

    //[return: NotNullIfNotNull(nameof(source))]
    public partial UserDb ToDb(UserDto source)
    {
        return null;
    }


    public override partial UserDto ToDto(UserDb source)
    {
        return null;
    }

    public partial UserDb ToDb2(UserDto source)
    {
        return null;
    }

    public partial UserDb ToDb2(UserDto source, UserDb destination)
    {
        return null;
    }
}
