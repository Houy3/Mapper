using Newtonsoft.Json.Linq;

namespace Entity.User;


public class UserDto
{
    public Guid Id { get; set; }

    public string Login { get; set; } = default!;

    public DateOnly BirthdayDate { get; set; }

    public int Number { get; set; }

    public JObject Info { get; set; } = default!;
}

public class UserCreateDto
{
    public string Login { get; set; } = default!;

    public DateOnly BirthdayDate { get; set; }

    public int Number { get; set; }

    public JObject Info { get; set; } = default!;
}
