using Entity.Role;

namespace Service.Role;

[Mapper.AutoImplementation]
internal partial class RoleMapper
{
    public partial RoleDto ToService(RoleService source);

    public partial RoleService ToDto(RoleDto source);
}
