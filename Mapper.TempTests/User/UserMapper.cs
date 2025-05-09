using Entity.User;
using Mapper.TempTests.User;

namespace Service.User;

[Mapper.AutoImplementation]
[Mapper.Settings(MappingRule = Mapper.MappingRuleEnum.MapOnlyPairs)]
public partial class UserMapper : BaseUserMapper, IUserMapper
{
    public partial UserDb ToDb(UserDto source);


    public partial UserDb ToDb2(UserDto source);

    public static partial UserDb ToDb2(UserDto source, UserDb destination);

    public override partial UserDto ToDto(UserDb source);

}
