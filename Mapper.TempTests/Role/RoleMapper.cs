using Entity.Role;

namespace Service.Role;

[Mapper.AutoImplementation]
public partial class RoleMapper
{

    public static partial RoleDto ToService(RoleService source);

    public static partial RoleService ToDto(RoleDto source);
}
