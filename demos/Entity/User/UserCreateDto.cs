namespace Entity.User;

public class UserCreateDto
{
    public string Login { get; set; } = default!;

    public DateOnly BirthdayDate { get; set; }

    public int Number { get; set; }

    public string Info { get; set; } = default!;
}
