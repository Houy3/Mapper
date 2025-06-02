using Entity.Role;

namespace Entity.User;


public class UserDto
{
    public Guid Id { get; set; }

    public string Login { get; set; } = default!;

    public DateOnly BirthdayDate { get; set; }

    public int Number { get; set; }

    public string Info { get; set; } = default!;

    public RoleDto Role { get; set; } = default!;
}

public class UserCreateDto
{
    public string Login { get; set; } = default!;

    public DateOnly BirthdayDate { get; set; }

    public int Number { get; set; }

    public string Info { get; set; } = default!;
}
